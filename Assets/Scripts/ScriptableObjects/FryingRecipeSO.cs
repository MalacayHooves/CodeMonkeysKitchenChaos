using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class FryingRecipeSO : ScriptableObject
{
    [SerializeField] private KitchenObjectSO input;
    public KitchenObjectSO GetInput() { return input; }

    [SerializeField] private KitchenObjectSO output;
    public KitchenObjectSO GetOutput() { return output; }

    [SerializeField] private float fryingTimerMax;
    public float GetFryingTimerMax() { return fryingTimerMax; }
}
