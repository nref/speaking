namespace FunctionalThinking;

public class MegaMartDatabase
{
    public readonly Dictionary<string, Item> ItemsForSale = new()
    {
        ["Grokking Simplicity"] =  new Item("Grokking Simplicity", 29.99M),
    };
}