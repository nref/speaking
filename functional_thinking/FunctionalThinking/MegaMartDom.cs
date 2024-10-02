namespace FunctionalThinking;

public class MegaMartDom
{
    private readonly Dictionary<Button, string> _buyButtons = new()
    {
        [ new Button("Buy Grokking Simplicity", "GrokkingSimplicity.png")] =  "Grokking Simplicity",
    };
    
    public string Tax { get; private set; } = "$0.00";
    
    public Dictionary<Button, string> GetBuyButtons() => _buyButtons;
    
    public void ShowTax(decimal tax) { Tax = $"${tax:0.00}"; }
    public void ShowCartTotal(decimal total) { /* ... */ }
    public void ShowFreeShippingIcon(Button button) { /* ... */ }
    public void HideFreeShippingIcon(Button button) { /* ... */ }
}