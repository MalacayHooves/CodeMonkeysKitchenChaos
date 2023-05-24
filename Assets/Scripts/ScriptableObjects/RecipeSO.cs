using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RecipeSO : ScriptableObject
{
    [SerializeField] private List<KitchenObjectSO> kitchenObjectSOList;
    public List<KitchenObjectSO> GetKitchenObjectSOList() { return kitchenObjectSOList; }
    [SerializeField] private string recipeName;
    public string GetRecipeName() { return recipeName; }
}
