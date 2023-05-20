using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class KitchenObjectSO : ScriptableObject
{
    [SerializeField] private Transform prefab;
    public Transform GetPrefab() { return prefab; }
    [SerializeField] private Sprite sprite;
    public Sprite GetSprite() { return sprite; }
    [SerializeField] private string objectName;
    public string GetObjectName() { return objectName; }
}
