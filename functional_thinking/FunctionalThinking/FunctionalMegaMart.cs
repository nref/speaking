namespace FunctionalThinking;

public class FunctionalMegaMart(MegaMartDom dom, MegaMartDatabase db) : IMegaMart
{
    private List<Item> _shoppingCart  = [];
    
    public void AddItemToCart(string name)
    {
        if (!db.ItemsForSale.TryGetValue(name, out Item? item))
        {
            return;
        }
        
        List<Item> newCart = AddItem(_shoppingCart, item);
        _shoppingCart = newCart;
        UpdateDom(newCart);
    }
   
    private void UpdateDom(List<Item> shoppingCart)
    {
        decimal total = CalcTotal(shoppingCart);
        dom.ShowCartTotal(total);
        dom.ShowTax(CalcTax(total));
        UpdateShippingIcons(total);
    }
    
    private void UpdateShippingIcons(decimal shoppingCartTotal)
    {
        var buttons = dom.GetBuyButtons();
        foreach (var kvp in buttons)
        {
            Button button = kvp.Key;
            string itemName = kvp.Value;
            
            UpdateShippingIcon(shoppingCartTotal, itemName, button);
        }
    }

    private void UpdateShippingIcon(decimal shoppingCartTotal, string itemName, Button button)
    {
        if (!db.ItemsForSale.TryGetValue(itemName, out Item? item))
        {
            return;
        }

        if (GetsFreeShipping(shoppingCartTotal, item.Price))
            dom.ShowFreeShippingIcon(button);
        else
            dom.HideFreeShippingIcon(button);
    }

    private static List<Item> AddItem(List<Item> cart, Item item)
    {
        var newCart = cart.ToList();
        newCart.Add(item);
        return newCart;
    }

    public static decimal CalcTotal(List<Item> cart) => cart.Sum(item => item.Price);
    public static decimal CalcTax(decimal amount) => amount * 0.10M;
    public static bool GetsFreeShipping(decimal total, decimal itemPrice) => itemPrice + total >= 20;
}