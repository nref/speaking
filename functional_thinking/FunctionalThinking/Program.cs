namespace FunctionalThinking;

public static class Program
{
    public static void Main(string[] args)
    {
        var dom = new MegaMartDom();
        var db = new MegaMartDatabase();
        
        IMegaMart mart = new StatefulMegaMart(dom, db);
            
        mart.AddItemToCart("Grokking Simplicity");
    }
}