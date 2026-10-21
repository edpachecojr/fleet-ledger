using FleetLedger.Api.Data.Seed;
using FleetLedger.Core.Models;

namespace FleetLedger.Api.Tests.Data.Seed;

public class ChartOfAccountsCatalogTests
{
    [Fact]
    public void Groups_FollowTheIncomeStatementOrder()
        => ChartOfAccountsCatalog.Groups.Select(group => group.Name).Should().Equal(
            "Receitas",
            "Deduções e ajustes de receita",
            "Despesas variáveis de viagem",
            "Remuneração de motoristas",
            "Manutenção",
            "Custos fixos",
            "Movimentações não-resultado");

    [Fact]
    public void Accounts_EachBelongToAnExistingGroupWhoseCodePrefixesTheirs()
    {
        Dictionary<Guid, string> groupCodes = ChartOfAccountsCatalog.Groups.ToDictionary(group => group.Id, group => group.Code);

        ChartOfAccountsCatalog.Accounts.Should().OnlyContain(
            account => groupCodes.ContainsKey(account.GroupId) && account.Code.StartsWith(groupCodes[account.GroupId] + "."));
    }

    [Fact]
    public void CodesAndIds_AreUnique()
    {
        ChartOfAccountsCatalog.Accounts.Select(account => account.Code).Should().OnlyHaveUniqueItems();
        ChartOfAccountsCatalog.Accounts.Select(account => account.Id)
            .Concat(ChartOfAccountsCatalog.Groups.Select(group => group.Id)).Should().OnlyHaveUniqueItems();
    }

    [Theory]
    [InlineData(ELedgerEntryContext.Trip, new[] { "Combustível", "Pedágio", "Chapas/Descarga", "Alimentação/Diárias", "Outras despesas de viagem" })]
    [InlineData(ELedgerEntryContext.FixedExpense, new[] { "Seguro", "IPVA/Licenciamento", "Aluguel", "Rastreamento", "Salários fixos", "Outros custos fixos" })]
    [InlineData(ELedgerEntryContext.Maintenance, new[] { "Manutenção realizada" })]
    [InlineData(ELedgerEntryContext.Billing, new[] { "Frete", "Descontos concedidos", "Acréscimos cobrados" })]
    public void Accounts_AcceptManualEntryOnlyInTheApprovedContext(ELedgerEntryContext entryContext, string[] expectedNames)
        => ChartOfAccountsCatalog.Accounts.Where(account => account.EntryContext == entryContext)
            .Select(account => account.Name).Should().Equal(expectedNames);

    [Fact]
    public void SystemAccounts_HaveNoManualEntryContext()
        => ChartOfAccountsCatalog.Accounts.Where(account => account.EntryContext is null)
            .Select(account => account.Name).Should()
            .Equal("Comissão/Remuneração de viagem", "Provisão de manutenção", "Adiantamento a motorista");
}
