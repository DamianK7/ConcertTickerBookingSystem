using System;

interface IConcert
{
    string Name { get; set; }
    string Date { get; set; }
    string Location { get; set; }
    int AvailableSeats { get; set; }
    decimal TicketPrice { get; set; }
}

class Concert : IConcert
{
    public string Name { get; set; }
    public string Date { get; set; }
    public string Location { get; set; }
    public int AvailableSeats { get; set; }
    public decimal TicketPrice { get; set; }

    public virtual string GetDetails()
    {
        return $"{Name} - {Date} - {Location} - Miejsca: {AvailableSeats} - Cena: {TicketPrice} zł";
    }
}

class VIPConcert : Concert
{
    public string MeetingDetails { get; set; }

    public override string GetDetails()
    {
        return base.GetDetails() + $" - VIP Spotkanie: {MeetingDetails}";
    }
}

class OnlineConcert : Concert
{
    public string Platform { get; set; }

    public override string GetDetails()
    {
        return base.GetDetails() + $" - Platforma: {Platform}";
    }
}

class PrivateConcert : Concert
{
    public string InvitationOnly { get; set; }

    public override string GetDetails()
    {
        return base.GetDetails() + $" - Tylko na zaproszenie: {InvitationOnly}";
    }
}
