namespace FunctionalThinking;

public class StatefulMegaMart(MegaMartDom dom, MegaMartDatabase db) : IMegaMart
{
    private readonly List<Item> _shoppingCart  = [];
    private decimal _cartTotal;
    
    public void AddItemToCart(string name)
    {
        if (!db.ItemsForSale.TryGetValue(name, out Item? item))
        {
            return;
        }
        
        _shoppingCart.Add(item);
        UpdateDom();
    }
    
    private void UpdateDom()
    {
        _cartTotal = 0M;
        foreach (var item in _shoppingCart)
        {
            _cartTotal += item.Price;
        }
        dom.ShowCartTotal(_cartTotal);
        dom.ShowTax(_cartTotal * 0.10M);
        UpdateShippingIcons();
    }
    
    private void UpdateShippingIcons()
    {
        var buttons = dom.GetBuyButtons();
        foreach (var kvp in buttons)
        {
            Button button = kvp.Key;
            string itemName = kvp.Value;
            
            UpdateShippingIcon(itemName, button);
        }
    }

    private void UpdateShippingIcon(string itemName, Button button)
    {
        if (!db.ItemsForSale.TryGetValue(itemName, out Item? item))
        {
            return;
        }

        if (item.Price + _cartTotal >= 20)
            dom.ShowFreeShippingIcon(button);
        else
            dom.HideFreeShippingIcon(button);
    }
}