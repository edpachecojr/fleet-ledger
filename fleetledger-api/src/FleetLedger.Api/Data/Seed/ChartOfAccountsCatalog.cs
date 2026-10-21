using FleetLedger.Core.Models;

namespace FleetLedger.Api.Data.Seed;

// Plano de Contas fixo aprovado pelo gestor (TASK-003, Q9), gravado pela migration via
// HasData. Ids e datas são constantes: mudá-los gera migration que reescreve as linhas.
// Conta nova ou renomeada aqui exige nova migration; nunca há edição pela aplicação.
public static class ChartOfAccountsCatalog
{
    private static readonly DateTimeOffset SeededAt = new(2026, 10, 21, 0, 0, 0, TimeSpan.Zero);

    public static readonly Guid RevenueGroupId = new("019a0000-0000-7000-8000-000000000001");
    public static readonly Guid RevenueDeductionsGroupId = new("019a0000-0000-7000-8000-000000000002");
    public static readonly Guid TripExpensesGroupId = new("019a0000-0000-7000-8000-000000000003");
    public static readonly Guid DriverPayGroupId = new("019a0000-0000-7000-8000-000000000004");
    public static readonly Guid MaintenanceGroupId = new("019a0000-0000-7000-8000-000000000005");
    public static readonly Guid FixedCostsGroupId = new("019a0000-0000-7000-8000-000000000006");
    public static readonly Guid NonResultGroupId = new("019a0000-0000-7000-8000-000000000007");

    public static IReadOnlyList<LedgerAccountGroup> Groups { get; } =
    [
        Group(RevenueGroupId, "1", "Receitas"),
        Group(RevenueDeductionsGroupId, "2", "Deduções e ajustes de receita"),
        Group(TripExpensesGroupId, "3", "Despesas variáveis de viagem"),
        Group(DriverPayGroupId, "4", "Remuneração de motoristas"),
        Group(MaintenanceGroupId, "5", "Manutenção"),
        Group(FixedCostsGroupId, "6", "Custos fixos"),
        Group(NonResultGroupId, "7", "Movimentações não-resultado")
    ];

    public static IReadOnlyList<LedgerAccount> Accounts { get; } =
    [
        Account("0101", RevenueGroupId, "1.01", "Frete", ELedgerEntryContext.Billing),
        Account("0201", RevenueDeductionsGroupId, "2.01", "Descontos concedidos", ELedgerEntryContext.Billing),
        Account("0202", RevenueDeductionsGroupId, "2.02", "Acréscimos cobrados", ELedgerEntryContext.Billing),
        Account("0301", TripExpensesGroupId, "3.01", "Combustível", ELedgerEntryContext.Trip),
        Account("0302", TripExpensesGroupId, "3.02", "Pedágio", ELedgerEntryContext.Trip),
        Account("0303", TripExpensesGroupId, "3.03", "Chapas/Descarga", ELedgerEntryContext.Trip),
        Account("0304", TripExpensesGroupId, "3.04", "Alimentação/Diárias", ELedgerEntryContext.Trip),
        Account("0305", TripExpensesGroupId, "3.05", "Outras despesas de viagem", ELedgerEntryContext.Trip),
        Account("0401", DriverPayGroupId, "4.01", "Comissão/Remuneração de viagem", null),
        Account("0501", MaintenanceGroupId, "5.01", "Provisão de manutenção", null),
        Account("0502", MaintenanceGroupId, "5.02", "Manutenção realizada", ELedgerEntryContext.Maintenance),
        Account("0601", FixedCostsGroupId, "6.01", "Seguro", ELedgerEntryContext.FixedExpense),
        Account("0602", FixedCostsGroupId, "6.02", "IPVA/Licenciamento", ELedgerEntryContext.FixedExpense),
        Account("0603", FixedCostsGroupId, "6.03", "Aluguel", ELedgerEntryContext.FixedExpense),
        Account("0604", FixedCostsGroupId, "6.04", "Rastreamento", ELedgerEntryContext.FixedExpense),
        Account("0605", FixedCostsGroupId, "6.05", "Salários fixos", ELedgerEntryContext.FixedExpense),
        Account("0606", FixedCostsGroupId, "6.06", "Outros custos fixos", ELedgerEntryContext.FixedExpense),
        Account("0701", NonResultGroupId, "7.01", "Adiantamento a motorista", null)
    ];

    private static LedgerAccountGroup Group(Guid id, string code, string name)
        => new() { Id = id, Code = code, Name = name, CreatedAt = SeededAt, UpdatedAt = SeededAt };

    // O sufixo do id repete o código da conta (3.01 → ...0301): fácil de achar no banco.
    private static LedgerAccount Account(
        string idSuffix,
        Guid groupId,
        string code,
        string name,
        ELedgerEntryContext? entryContext)
        => new()
        {
            Id = new Guid($"019a0000-0000-7000-8000-00000001{idSuffix}"),
            GroupId = groupId,
            Code = code,
            Name = name,
            EntryContext = entryContext,
            CreatedAt = SeededAt,
            UpdatedAt = SeededAt
        };
}
