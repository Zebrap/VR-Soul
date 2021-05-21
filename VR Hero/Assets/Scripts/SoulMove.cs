using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulMove : MonoBehaviour
{
    private Transform targetPos;
    public float yOffset = 0.2f;
    public float zOffset = -0.1f;
    private float step;
    [SerializeField] [Range(0f,10f)]float lerpTime = 0.1f;

    ParticleSystem particle;

    private void Awake() {
        step = lerpTime*Time.fixedDeltaTime;
        particle= GetComponent<ParticleSystem>();
    }
    public void NewTarget(Transform target){
        targetPos = target;
    }

    private void FixedUpdate() {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPos.position.x, targetPos.position.y+yOffset, targetPos.position.z + zOffset), step);
    }
}
