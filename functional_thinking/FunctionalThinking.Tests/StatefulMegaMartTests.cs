using FluentAssertions;
using Xunit;

namespace FunctionalThinking.Tests;

public class StatefulMegaMartTests
{
    [Fact]
    public void AddItemsToCart_UpdatesTaxDom()
    {
        var dom = new MegaMartDom();
        IMegaMart mart = new StatefulMegaMart(dom, new MegaMartDatabase());
        
        mart.AddItemToCart("Grokking Simplicity");
        dom.Tax.Should().Be("$3.00");
    }
}