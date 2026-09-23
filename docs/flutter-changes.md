# Flutter App Changes Tracker

Running checklist of backend changes that the Flutter app needs to adopt to complete a feature's integration, or that are being deliberately held back as breaking changes pending approval. Updated incrementally as each backend feature lands — **`api.txt` (repo root, currently v3.18) is the exact, authoritative request/response contract of every endpoint referenced below**; read the cited `api.txt` section before implementing, since this file only summarizes.

Sections are removed once the Flutter app has fully adopted them — this file tracks *pending/active* work, not a history of everything ever shipped. Completed feature history lives in git log and `api.txt`'s own version notes, not here.

**Standing constraint (2026-09-21):** the Flutter app is live on the Play Store and App Store, both of which have review/approval lag, while the backend/API can be updated instantly. Every backend change in this project is therefore built to be **additive and optional** — a currently-published app build must keep working completely unchanged against the updated backend, with zero risk of breakage while store approval for the new app version is pending. New fields on existing endpoints are nullable/optional with a legacy fallback; new endpoints are new routes an old app simply never calls. Nothing here is a hard cutover.

Legend:
- **Available now** — backend is live, app can adopt whenever convenient (non-breaking, optional).
- **Held for approval** — a genuinely breaking change to a live endpoint contract (not just optional-field additions) that hasn't been implemented at all yet; listed here so the scope is visible ahead of time. Per the constraint above, when these are eventually implemented they should also default to an optional/additive interim contract rather than a hard break, unless explicitly decided otherwise at that time.
- **TODO(remove after old app retired)** — inline code/doc comments marking legacy-fallback branches that exist ONLY to support currently-published app builds. Once the new app version is confirmed live on both stores (i.e. no meaningfully active install base still hits these code paths), these branches can be deleted — grep the codebase for this exact marker to find all of them. Do not remove any of these until that confirmation, even if it looks safe.

---

## Client Address GPS Pin-Drop

**Status: backend ready and live; Flutter app has adopted it (2026-09-23).**

Adopted in-app:
- `pubspec.yaml`: added `google_maps_flutter` and `geolocator`. Google Maps API keys wired into
  `android/app/src/main/AndroidManifest.xml` (`com.google.android.geo.API_KEY` meta-data) and
  `ios/Runner/AppDelegate.swift` (`GMSServices.provideAPIKey`), each restricted per-platform to
  this app's package name / bundle ID. `ACCESS_FINE_LOCATION`/`ACCESS_COARSE_LOCATION` added to
  the Android manifest; `NSLocationWhenInUseUsageDescription` added to `Info.plist`.
- `lib/services/location_permission_service.dart` (`LocationPermissionService`) — mirrors
  `camera_permission_service.dart`'s shape, wraps `Permission.locationWhenInUse`.
- `lib/models/reverse_geocode_result.dart` + `lib/services/geocoding_api_service.dart`
  (`GeocodingApiService.reverseGeocode`) — thin call to `GET /api/geocoding/reverse`, called
  directly from the screen (no repository/provider layer — a single one-off call, not shared
  app state, consistent with this app's screen-scoped-fetch convention).
- `lib/screens/add_address_screen.dart` reworked per the confirmed UX flow: `GoogleMap` at the
  top (tap-to-place/drag pin), defaults to device GPS position when location permission is
  granted else Karachi, explicit "Save Pin" button that calls reverse-geocode and pre-fills
  Area/Full Address/City (still editable), and "Save Address" submits the pin + text fields
  together in one call.
- `lib/models/client_address.dart`: `latitude`/`longitude` are now nullable `double?` (was
  non-nullable with a `?? 0` fallback), plus a new `hasLocation` field read straight from the
  API. `lib/services/client_address_api_service.dart` / `lib/data/repositories/
  client_address_repository.dart` / `lib/providers/client_address_provider.dart`: `latitude`/
  `longitude` params are now nullable and omitted from the request body entirely when unset,
  instead of defaulting to `(0, 0)` — `hasLocation` now stays meaningful. Editing an address
  without touching the map still preserves the existing pin unchanged.
- `lib/screens/profile_screen.dart`'s Addresses section shows a small location-pin icon next to
  the address title when `hasLocation` is true, and a tappable "Set location" chip (opens Edit)
  when false — this also covers addresses created before this feature existed.
- Inline pin-drop map enlarged (220px -> 320px), plus a full-screen expand button
  (`lib/screens/pin_location_fullscreen_screen.dart`, `lib/widgets/address_pin_map_view.dart`
  shared between both) for placing a more precise pin. The map's built-in "my location" button
  (`myLocationButtonEnabled`/`myLocationEnabled`) is now shown once location permission is
  granted, in both the inline and full-screen map.

**Deferred idea — map search box (not implemented):** a text search field inside the pin-drop
map (type a city/area, map jumps to it, then the user places the pin) would be a nice addition,
but Google Places Autocomplete is billed (pay-per-session beyond the shared $200/month Maps
Platform free credit), unlike Maps SDK rendering which is free. To keep this feature $0-cost like
the rest of it, the plan if/when this gets picked up is to reuse the same free OpenStreetMap
Nominatim service the backend already calls for `GET /api/geocoding/reverse` — but for **forward**
search-by-text this time. That needs a new backend endpoint (e.g. `GET /api/geocoding/search?q=`)
mirroring the existing reverse one; out of scope for a Flutter-only change, so this is parked here
for the backend agent to pick up, not started.

Backend contract reference (already live, no further backend work needed for this feature):
- `POST`/`PUT /api/client-addresses` accept optional `latitude`/`longitude` (both nullable decimals) — see `api.txt`'s `POST Client Address (Create)` / `PUT Client Address (Update)` sections under `CLIENT ADDRESSES APIs`. If you send one, you must send both (a validation `Fail (400)` catches a one-sided payload — see that section's Notes).
- `GET /api/geocoding/reverse?lat=&lng=` resolves a coordinate pair into a human-readable address (road, area, city, state, postcode) via free OpenStreetMap Nominatim — no API key needed. See `api.txt`'s `GEOCODING APIs` section.
- Every `ClientAddress` response (`GET`/`POST`/`PUT /api/client-addresses`) includes `hasLocation: bool` — `true` only when a real pin is set (treats `(0, 0)` the same as "no pin").
