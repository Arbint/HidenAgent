using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPuchaseLisener
{
    bool ItemPurchased(Object newPurchase);
}

public class CreditComponent : MonoBehaviour
{
    [SerializeField] int credits;

    IPuchaseLisener[] purchaseListeners;

    private void Awake()
    {
        purchaseListeners = GetComponents<IPuchaseLisener>();
    }

    public int Credits { get { return credits; } }

    public delegate void OnCreditChanged(int newCredit);
    public event OnCreditChanged onCreditChanged;
    public bool TryPurchaseItem(ShopItem item)
    {
        if (credits < item.Price) 
            return false;

        credits -= item.Price;
        onCreditChanged?.Invoke(credits);
        foreach(IPuchaseLisener listener in purchaseListeners)
        {
            if (listener.ItemPurchased(item.Item))
                break;
        }

        return true;
    }

    internal int GetCredits()
    {
        return credits;
    }
}
