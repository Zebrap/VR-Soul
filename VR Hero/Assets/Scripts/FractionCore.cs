using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Stats))]
[RequireComponent(typeof(BoxCollider))]
public class FractionCore : GetHitObject
{
    Stats stats;
    BoxCollider boxCollider;

    private void Start() {
        stats = GetComponent<Stats>();
        boxCollider= GetComponent<BoxCollider>();
        stats.OnDieCallBack += OnDie;
    }
    public override void GetHit(float dmg)
    {
        stats.GetHit(dmg);
    }

    public void OnDie(){
        boxCollider.enabled = false;
        isAlive = false;
    }
}
