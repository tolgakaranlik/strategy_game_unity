using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FakeTorch : MonoBehaviour
{
    public Light LightSource;
    public float Interval = 0.5f;

    float originalIntensity = 1;

    // Start is called before the first frame update
    void Start()
    {
        originalIntensity = LightSource.intensity;
        InvokeRepeating("AdjustLight", 0, Interval);
    }

    // Update is called once per frame
    void AdjustLight()
    {
        LightSource.DOIntensity(originalIntensity + (Random.Range(0, 2) == 1 ? +1 : -1) * Random.Range(0.1f, originalIntensity * 0.45f), Interval);
    }
}
