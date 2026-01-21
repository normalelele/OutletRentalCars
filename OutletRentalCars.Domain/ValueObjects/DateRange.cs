namespace OutletRentalCars.Domain.ValueObjects;

public class DateRange
{
    public DateTime Start { get; private set; }
    public DateTime End { get; private set; }

    public DateRange(DateTime start, DateTime end)
    {
        if (start >= end)
            throw new ArgumentException("Start date must be before end date");

        Start = start;
        End = end;
    }
    
    public bool OverlapsWith(DateRange other)
    {
        return Start < other.End && End > other.Start;
    }

    public int TotalDays => (End - Start).Days;
    
    public override bool Equals(object? obj)
    {
        if (obj is not DateRange other) return false;
        return Start == other.Start && End == other.End;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Start, End);
    }
}