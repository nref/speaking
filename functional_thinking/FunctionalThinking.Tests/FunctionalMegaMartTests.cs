using FluentAssertions;
using Xunit;

namespace FunctionalThinking.Tests;

public class FunctionalMegaMartTests
{
    [Fact]
    public void CalcTotal_ShouldCalculateTotal()
    {
        List<Item> cart =
        [
            new("", 1.00M),
            new("", 2.00M),
            new("", 3.00M)
        ];
        
        FunctionalMegaMart.CalcTotal(cart).Should().Be(6.00M);
    }
    
    [Fact]
    public void CalcTax_ShouldCalculateTax()
    {
        List<decimal> prices = [ 5.00M, 10.00M, 15.00M, 20.00M ];
        List<decimal> taxes = prices.Select(FunctionalMegaMart.CalcTax).ToList();
        
        taxes.Should().BeEquivalentTo([0.5M, 1.0M, 1.5M, 2.0M]);
    }
    
    [Fact]
    public void GetsFreeShipping_ReturnsTrue_WhenTotalGeq20()
    {
        FunctionalMegaMart.GetsFreeShipping(19.99M, 0.01M).Should().BeTrue();
        FunctionalMegaMart.GetsFreeShipping(20.00M, 0.01M).Should().BeTrue();
    }
    
    [Fact]
    public void GetsFreeShipping_ReturnsFalse_WhenTotalLeq20()
    {
        FunctionalMegaMart.GetsFreeShipping(19.99M, 0.00M).Should().BeFalse();
    }
}
