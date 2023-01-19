using UnityEngine;

namespace PlateUpEmotesApi.Anim;

public class BonePair
{
    public Transform original, newiginal;
    public BonePair(Transform n, Transform o)
    {
        newiginal = n;
        original = o;
    }
}