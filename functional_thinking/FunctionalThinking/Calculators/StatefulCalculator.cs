namespace FunctionalThinking.Calculators;

public class StatefulCalculator
{
    public int Result { get; private set; }

    public void Divide(int x, int y) => Result = x / y;
  
    public static void RunDemo()
    {
        var calc = new StatefulCalculator();
        var nums = Enumerable.Range(1, 100);
    
        Parallel.ForEach(nums, num => 
        {
            calc.Divide(num, 2);
            Console.Write($"{calc.Result} ");
        });
    }
}