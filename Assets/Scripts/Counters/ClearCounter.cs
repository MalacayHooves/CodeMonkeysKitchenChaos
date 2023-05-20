using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject()) 
        {
            //there is no KitchenObject here
            if (player.HasKitchenObject())
            {
                //if player is carrying something
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                //if player is not carrying anything
            }
        }
        else
        {
            //there is KitchenObject here
            if(player.HasKitchenObject())
            {
                //if player is carrying something

            }
            else
            {
                //player is not carying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
