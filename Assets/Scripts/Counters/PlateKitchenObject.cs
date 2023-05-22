using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngridientAddedEventsArgs> OnIngridientAdded;
    public class OnIngridientAddedEventsArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }

    [SerializeField] private List<KitchenObjectSO> validKitchenObjectsSOList;

    private List<KitchenObjectSO> kitchenObjectSOList;

    private void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngridient(KitchenObjectSO kitchenObjectSO)
    {
        if (!validKitchenObjectsSOList.Contains(kitchenObjectSO))
        {
            return false;
        }
        if (kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            //already has this type
            return false;
        }
        else
        {
            kitchenObjectSOList.Add(kitchenObjectSO);

            OnIngridientAdded?.Invoke(this, new OnIngridientAddedEventsArgs
            {
                kitchenObjectSO = kitchenObjectSO
            });

            return true;
        }
    }
}
