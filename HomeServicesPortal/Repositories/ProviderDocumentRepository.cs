using HomeServicesPortal.Data;
using HomeServicesPortal.Entities;
using HomeServicesPortal.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeServicesPortal.Repositories;

public class ProviderDocumentRepository : IProviderDocumentRepository
{
    private readonly AppDbContext _db;

    public ProviderDocumentRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<bool> ProviderExistsAsync(int providerUid, CancellationToken cancellationToken = default)
    {
        return _db.Providers.AsNoTracking().AnyAsync(p => p.Uid == providerUid, cancellationToken);
    }

    public Task<string?> GetProviderMobileNoAsync(int providerUid, CancellationToken cancellationToken = default)
    {
        return _db.Providers.AsNoTracking()
            .Where(p => p.Uid == providerUid)
            .Select(p => (string?)p.MobileNo)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<bool?> GetProviderIsVerifiedAsync(int providerUid, CancellationToken cancellationToken = default)
    {
        return _db.Providers.AsNoTracking()
            .Where(p => p.Uid == providerUid)
            .Select(p => (bool?)p.IsVerified)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task SetProviderIsVerifiedAsync(int providerUid, bool isVerified, CancellationToken cancellationToken = default)
    {
        var provider = await _db.Providers.FirstOrDefaultAsync(p => p.Uid == providerUid, cancellationToken);
        if (provider == null)
        {
            return;
        }

        provider.IsVerified = isVerified;
        await _db.SaveChangesAsync(cancellationToken);
    }

    public Task<ProviderDocument?> GetByProviderUidAsync(int providerUid, CancellationToken cancellationToken = default)
    {
        return _db.ProviderDocuments.FirstOrDefaultAsync(d => d.ProviderUid == providerUid, cancellationToken);
    }

    public async Task<ProviderDocument> AddAsync(ProviderDocument document, CancellationToken cancellationToken = default)
    {
        _db.ProviderDocuments.Add(document);
        await _db.SaveChangesAsync(cancellationToken);
        return document;
    }

    public async Task UpdateAsync(ProviderDocument document, CancellationToken cancellationToken = default)
    {
        _db.ProviderDocuments.Update(document);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(ProviderDocument document, CancellationToken cancellationToken = default)
    {
        _db.ProviderDocuments.Remove(document);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
