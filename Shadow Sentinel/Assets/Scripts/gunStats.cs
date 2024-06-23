using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class gunStats : ScriptableObject
{
    public GameObject gunModel;
    [Range(1, 10)] public int shootDamage;
    [Range(15, 1000)] public int shootDist;
    [Range(0.1f, 3)] public float shootRate;
    public int ammoCur;
    public int ammoMax;
    public float reloadSpeed;

    public ParticleSystem hitEffect;
    public AudioClip shootSound;
    [Range(0, 1)] public float shootVol;
}
