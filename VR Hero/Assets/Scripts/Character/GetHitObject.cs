using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GetHitObject : MonoBehaviour
{    public bool isAlive = true;
    public abstract void GetHit(float dmg);
}
