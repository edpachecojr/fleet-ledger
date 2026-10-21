using FleetLedger.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FleetLedger.Api.Data.Stores.LedgerAccounts;

// Sem ForOrganization: o Plano de Contas é global. As consultas ficam públicas para que
// os testes confiram o SQL gerado sem banco (docs/adr/0004).
public sealed class LedgerAccountStore(AppDbContext context) : ILedgerAccountStore
{
    public async Task<List<LedgerAccountGroup>> ListGroupsWithAccountsAsync(CancellationToken cancellationToken)
        => await GroupsWithAccountsQuery().ToListAsync(cancellationToken);

    public async Task<List<LedgerAccount>> ListByEntryContextAsync(
        ELedgerEntryContext entryContext,
        CancellationToken cancellationToken)
        => await ByEntryContextQuery(entryContext).ToListAsync(cancellationToken);

    public IQueryable<LedgerAccountGroup> GroupsWithAccountsQuery()
        => context.LedgerAccountGroups
            .AsNoTracking()
            .Include(group => group.Accounts.OrderBy(account => account.Code))
            .OrderBy(group => group.Code);

    public IQueryable<LedgerAccount> ByEntryContextQuery(ELedgerEntryContext entryContext)
        => context.LedgerAccounts
            .AsNoTracking()
            .Where(account => account.EntryContext == entryContext)
            .OrderBy(account => account.Code);
}
