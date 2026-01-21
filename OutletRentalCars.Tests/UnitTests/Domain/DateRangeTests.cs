using FluentAssertions;
using OutletRentalCars.Domain.ValueObjects;

namespace OutletRentalCars.Tests.UnitTests.Domain;

public class DateRangeTests
{
    [Fact]
    public void Constructor_WhenStartDateIsAfterEndDate_ShouldThrowArgumentException()
    {        
        var start = new DateTime(2026, 2, 10);
        var end = new DateTime(2026, 2, 5);
        
        Action act = () => new DateRange(start, end);
        
        act.Should().Throw<ArgumentException>()
            .WithMessage("La fecha de inicio debe ser anterior a la fecha de finalización");
    }

    [Fact]
    public void Constructor_WhenStartDateEqualsEndDate_ShouldThrowArgumentException()
    {        
        var date = new DateTime(2026, 2, 10);
        
        Action act = () => new DateRange(date, date);
        
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void OverlapsWith_WhenRangesOverlap_ShouldReturnTrue()
    {        
        var range1 = new DateRange(new DateTime(2026, 2, 1), new DateTime(2026, 2, 10));
        var range2 = new DateRange(new DateTime(2026, 2, 5), new DateTime(2026, 2, 15));
        
        var result = range1.OverlapsWith(range2);
        
        result.Should().BeTrue();
    }

    [Fact]
    public void OverlapsWith_WhenRangesDoNotOverlap_ShouldReturnFalse()
    {        
        var range1 = new DateRange(new DateTime(2026, 2, 1), new DateTime(2026, 2, 10));
        var range2 = new DateRange(new DateTime(2026, 2, 15), new DateTime(2026, 2, 20));
        
        var result = range1.OverlapsWith(range2);
        
        result.Should().BeFalse();
    }

    [Fact]
    public void OverlapsWith_WhenRangesAreTouching_ShouldReturnFalse()    {
        
        var range1 = new DateRange(new DateTime(2026, 2, 1), new DateTime(2026, 2, 10));
        var range2 = new DateRange(new DateTime(2026, 2, 10), new DateTime(2026, 2, 20));
        
        var result = range1.OverlapsWith(range2)
        
        result.Should().BeFalse();
    }

    [Fact]
    public void TotalDays_ShouldReturnCorrectNumberOfDays()    {
        
        var range = new DateRange(new DateTime(2026, 2, 1), new DateTime(2026, 2, 6));
        
        var totalDays = range.TotalDays;
        
        totalDays.Should().Be(5);
    }
}