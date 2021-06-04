using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBall : SpecialAttack
{
    private Vector3 targetPos;

    public ParticleSystem particleBall;
    public float spellPower = 100;

    private LayerMask targetLayer;

    private bool isCasted = false;
    private float step;
    [SerializeField] [Range(1f, 50f)] float lerpTime = 2f;

    private float timeToEnd = 3f;
    private float nextTime;
    private void Start()
    {
        step = lerpTime * Time.fixedDeltaTime;
    }
    public override bool Cast(Vector3 myStartPos, Vector3 endPosition, LayerMask layerTarget, Transform selfTransform)
    {
        if (!isCasted)
        {
            isCasted = true;
            transform.position = myStartPos;
            targetPos = endPosition;
            targetLayer = layerTarget;
            particleBall.gameObject.SetActive(true);
            particleBall.Clear();
            particleBall.Play();
            StopCoroutine("ShootSpell");
            StartCoroutine("ShootSpell");
            return true;
        }
        return false;
    }

    IEnumerator ShootSpell()
    {
        nextTime = Time.time + timeToEnd;
        while (Time.time < nextTime)
        {
            yield return Time.fixedDeltaTime;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPos.x, targetPos.y, targetPos.z), step);
        }
        DisableSpell();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(IsInLayerMask(other.gameObject, targetLayer)){
            other.gameObject.GetComponent<GetHitObject>().GetHit(spellPower);
            DisableSpell();
        }
    }
    private bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return ((layerMask.value & (1 << obj.layer)) > 0);
    }

    private void DisableSpell()
    {
        particleBall.Stop();
        particleBall.Clear();
        isCasted = false;
    }
}
