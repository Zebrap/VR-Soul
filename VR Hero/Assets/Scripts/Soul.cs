using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
    private Transform targetTransform;
    public float yOffset = 0.2f;
    public float zOffset = -0.1f;
    private float step;
    [SerializeField] [Range(0f, 10f)] float lerpTime = 0.1f;

    ParticleSystem particle;
    public Gradient[] particleGradientsColorIndicator;
    CharacterManager characterManager;

    public ParticleSystem soulParticle;
    public ParticleSystem trailParticle;

    protected bool hasCharacter;

    private void Awake()
    {
        step = lerpTime * Time.fixedDeltaTime;
        particle = GetComponent<ParticleSystem>();
    }
    public void NewTarget(Transform target)
    {
        if (characterManager != null)
        {
            CancelEventsOnCharacter();
        }
        targetTransform = target;
        characterManager = target.gameObject.GetComponent<CharacterManager>();
        characterManager.OnGetHit += ChangeHpIndicator;
        characterManager.OnDieEvent += OnLostCharacter;

        characterManager.InitPlayerOnMe();
        StopCoroutine("DeathTimer");
        hasCharacter = true;
    }
    Vector3 vector;
    private void FixedUpdate()
    {
        if (targetTransform != null && hasCharacter)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetTransform.rotation, step);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetTransform.position.x, targetTransform.position.y + yOffset, targetTransform.position.z), step);
        }
    }

    private void OnLostCharacter()
    {
        CancelEventsOnCharacter();
        targetTransform = transform;
        StartCoroutine("DeathTimer");
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(2f);
        hasCharacter = false;
    }

    private void CancelEventsOnCharacter()
    {
        characterManager.OnGetHit -= ChangeHpIndicator;
        characterManager.OnDieEvent -= OnLostCharacter;
    }

    private void ChangeHpIndicator(float hp, float maxHp)
    {
        float hpPercent = hp / maxHp;
        var colorOverTimeSoul = soulParticle.colorOverLifetime;
        var colorOverTimeTrail = trailParticle.colorOverLifetime;
        colorOverTimeSoul.enabled = true;
        colorOverTimeTrail.enabled = true;
        if (hpPercent >= 0.8f)
        {
            colorOverTimeSoul.color = particleGradientsColorIndicator[0];
            colorOverTimeTrail.color = particleGradientsColorIndicator[0];
        }
        else if (hpPercent >= 0.4f)
        {
            colorOverTimeSoul.color = particleGradientsColorIndicator[1];
            colorOverTimeTrail.color = particleGradientsColorIndicator[1];
        }
        else
        {
            colorOverTimeSoul.color = particleGradientsColorIndicator[2];
            colorOverTimeTrail.color = particleGradientsColorIndicator[2];
        }
    }
}
