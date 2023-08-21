using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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

    protected override void Awake()
    {
        base.Awake();
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList() { return kitchenObjectSOList; }

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
            AddIngridientServerRpc(
                KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(kitchenObjectSO));

            return true;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void AddIngridientServerRpc(int kitchenObjectSOIndex)
    {
        AddIngridientClientRpc(kitchenObjectSOIndex);
    }

    [ClientRpc]
    private void AddIngridientClientRpc(int kitchenObjectSOIndex)
    {
        KitchenObjectSO kitchenObjectSO = KitchenGameMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);

        kitchenObjectSOList.Add(kitchenObjectSO);

        OnIngridientAdded?.Invoke(this, new OnIngridientAddedEventsArgs
        {
            kitchenObjectSO = kitchenObjectSO
        });
    }
}
