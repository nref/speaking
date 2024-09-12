using FluentAssertions;

namespace Put.Tests;

public class Calculator
{
    public int Divide(int x, int y) => x / y;
    public int DivideSafe(int x, int y) => y switch
    {
        0 => int.MaxValue,
        _ => x / y
    };
}

public class DivideMethod
{
    [Fact]
    public void ReturnsQuotient() =>
        new Calculator()
          .Divide(8, 2)
          .Should().Be(4);

    [Fact]
    public void DivideByZeroThrows() => new Action(() =>
        new Calculator()
          .Divide(8, 0))
          .Should().Throw<DivideByZeroException>();
}

public class DivideSafeMethod
{
    [Fact]
    public void DoesNotThrow() => new Action(() =>
        new Calculator()
          .DivideSafe(8, 0))
          .Should().NotThrow();
}

public interface IBar { int DoBar();  }
public class FakeBar : IBar { public int DoBar() => 42; }

// A unit with a dependency, hardcoded
public class Foo
{
    private readonly IBar bar = new FakeBar();
    public int DoFoo() => bar.DoBar();
}

public class DoFooMethod
{
    [Fact]
    public void Foos() => new Foo().DoFoo().Should().Be(42);
}

// A unit with a dependency, injected
public class Foo2(IBar bar)
{
    public int DoFoo() => bar.DoBar();
}


public class MysteryService
{
    public int Mystery(int d) => Math.Abs(d);
}

public class MysteryMethod
{
    [Theory]
    [InlineData(-2, 2)]
    [InlineData(-1, 1)]
    [InlineData(0, 0)]
    [InlineData(1, 1)] 
    [InlineData(2, 2)]
    [InlineData(2, 3)] // Fails
    public void Characterize(int input, int output) => 
        new MysteryService().Mystery(input).Should().Be(output);
}

// public class SomeService(ISomeDependency dep) { ... }

public class Effectful
{
    public int Count { get; private set; }
    public void Add(int i) => Count += i;
}

public class AddMethod
{
    private Effectful effectful = new();

    // These tests cannot run in parallel!
    [Fact]
    public void Adds()
    {
        effectful.Add(5);
        effectful.Add(3);
        effectful.Add(2);
        effectful.Count.Should().Be(10);
    }

    [Fact]
    public void Adds2()
    {
        effectful.Add(11);
        effectful.Count.Should().Be(11);
    }
}

public class Stateless
{
    public int Add(int x, int y) => x + y;
}

public class StatelessAddMethod
{
    // Can run in parallel
    [Fact]
    public void AddMethod()
    {
        var stateless = new Stateless();
        var sum = stateless.Add(5, 3);
        var sum2 = stateless.Add(sum, 2);
        sum2.Should().Be(10);
    }
}
