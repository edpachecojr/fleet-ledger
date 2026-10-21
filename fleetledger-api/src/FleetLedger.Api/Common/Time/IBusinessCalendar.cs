namespace FleetLedger.Api.Common.Time;

public interface IBusinessCalendar
{
    // Data de hoje no fuso da empresa (Calendar:TimeZone).
    DateOnly Today();
}
