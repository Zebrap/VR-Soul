using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aura : SpecialAttack
{
    public ParticleSystem particle;
    public float spellPower;

    private Transform targetTransform;
    private bool isCasted = false;
    private float healInterval = 0.5f;
    private float effect_duration = 3f;

    private GetHitObject hitTarget;
    public override bool Cast(Vector3 myStartPos, Vector3 endPosition, LayerMask layerTarget, Transform selfTransform)
    {
        if (!isCasted)
        {
            isCasted = true;
            transform.position = selfTransform.position;
            targetTransform = selfTransform;
            hitTarget = selfTransform.GetComponent<GetHitObject>();
            StartCoroutine("SpellUse");
            particle.gameObject.SetActive(true);
            particle.Clear();
            particle.Play();
            return true;
        }
        return false;
    }

    IEnumerator SpellUse()
    {
        StartCoroutine("ChangePosition");
        for (float i = 0; i < effect_duration / healInterval; i++) // Time/inteval = numbersOfHeal
        {
            hitTarget.HealHP(spellPower/(effect_duration / healInterval));
            yield return new WaitForSeconds(healInterval);
        }
        DisableSpell();
    }

    IEnumerator ChangePosition(){
        while(isCasted){
            yield return Time.fixedDeltaTime;
            transform.position = targetTransform.position;
        }
    }

    private void DisableSpell()
    {
        particle.Stop();
        particle.Clear();
        isCasted = false;
    }
}
