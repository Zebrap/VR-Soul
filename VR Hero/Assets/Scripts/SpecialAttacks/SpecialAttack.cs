using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpecialAttack : MonoBehaviour
{
    public abstract bool Cast(Vector3 myStartPos, Vector3 target, LayerMask layerTarget);
}
