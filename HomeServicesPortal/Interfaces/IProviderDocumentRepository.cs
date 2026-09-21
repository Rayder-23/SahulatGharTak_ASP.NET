using HomeServicesPortal.Entities;

namespace HomeServicesPortal.Interfaces;

/// <summary>Data access for dbo.ProviderDocuments (one row per provider).</summary>
public interface IProviderDocumentRepository
{
    Task<bool> ProviderExistsAsync(int providerUid, CancellationToken cancellationToken = default);

    Task<string?> GetProviderMobileNoAsync(int providerUid, CancellationToken cancellationToken = default);

    Task<bool?> GetProviderIsVerifiedAsync(int providerUid, CancellationToken cancellationToken = default);

    Task SetProviderIsVerifiedAsync(int providerUid, bool isVerified, CancellationToken cancellationToken = default);

    Task<ProviderDocument?> GetByProviderUidAsync(int providerUid, CancellationToken cancellationToken = default);

    Task<ProviderDocument> AddAsync(ProviderDocument document, CancellationToken cancellationToken = default);

    Task UpdateAsync(ProviderDocument document, CancellationToken cancellationToken = default);

    Task DeleteAsync(ProviderDocument document, CancellationToken cancellationToken = default);
}
