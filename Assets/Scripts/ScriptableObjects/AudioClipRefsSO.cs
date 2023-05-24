using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu]
public class AudioClipRefsSO : ScriptableObject
{
    [SerializeField] private AudioClip[] chop;
    public AudioClip[] Chop { get => chop; }
    [SerializeField] private AudioClip[] deliveryFailed;
    public AudioClip[] DeliveryFailed { get => deliveryFailed; }
    [SerializeField] private AudioClip[] deliverySuccess;
    public AudioClip[] DeliverySuccess { get => deliverySuccess; }
    [SerializeField] private AudioClip[] footstep;
    public AudioClip[] Footstep { get => footstep; }
    [SerializeField] private AudioClip[] objectDrop;
    public AudioClip[] ObjectDrop { get => objectDrop; }
    [SerializeField] private AudioClip[] objectPickup;
    public AudioClip[] ObjectPickup { get => objectPickup; }
    [SerializeField] private AudioClip stoveSizzle;
    public AudioClip StoveSizzle { get => stoveSizzle; }
    [SerializeField] private AudioClip[] trash;
    public AudioClip[] Trash { get => trash; }
    [SerializeField] private AudioClip[] warning;
    public AudioClip[] Warning { get => warning; }
}
