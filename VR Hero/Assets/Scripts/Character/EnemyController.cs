using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [HideInInspector]
    public NavMeshAgent agent;
    private Animator animator;
    private Vector3 startingPosition;

    public bool isTargetInReach = false;
    public bool isApprochingTarget = false;
    public float appprochingRange = 5f;

    private float distanceFromPlayer;
    private Transform player;

    private float speedSmoothTime = 0.1f;
    public bool isAttack;

    private CharacterManager characterManager;
    public float speedFaceTargetRotation = 2f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        characterManager = GetComponent<CharacterManager>();
        startingPosition = transform.position;
    }

    private void OnEnable()
    {
        InvokeRepeating("CheckApproching", 1f, 0.3f);
    }

    private void Update()
    {
        if (characterManager.isAlive && isTargetInReach && characterManager.canMove && player != null)
        {
            if (player.GetComponent<CharacterManager>().isAlive)
            {
                if (!isAttack && TargetInRange())
                {
                    StartCoroutine("AttackDelay");
                }
                else
                {
                    StopCoroutine("AttackDelay");
                    FollowPlayer();
                }
                FaceTarget();
            }
        }
        if (characterManager.isAlive)
        {
              animator.SetFloat("Move", agent.velocity.magnitude);
            //animator.SetFloat("Move", agent.velocity.magnitude, speedSmoothTime, Time.deltaTime);
        }
    }

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(0.3f);
        characterManager.Attack();
    }

    public void GoBackToStartPosition()
    {
        if (characterManager.isAlive)
        {
            agent.enabled = true;
            agent.isStopped = false;
            isApprochingTarget = false;
            agent.SetDestination(startingPosition);
        }
    }

    private void FollowPlayer()
    {
        if (!isApprochingTarget)
        {
            isApprochingTarget = true;
            ////
        }
        if (agent.enabled)
        {
            if (player != null && !TargetInRange())
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
            }
            else
            {
                agent.isStopped = true;
                agent.ResetPath();
            }
        }
    }

    public bool TargetInRange()
    {
        if (player != null)
        {
            distanceFromPlayer = Vector3.Distance(player.position, transform.position);
            if (distanceFromPlayer > characterManager.attackRange)
            {
                return false;
            }
            else
            {
                return true; ;
            }
        }
        return false;
    }


    private void CheckApproching()
    {
        if (characterManager.isAlive)
        {
            float closeDistance = 1000f;
            Collider[] characters = Physics.OverlapSphere(transform.position, appprochingRange, characterManager.enemyLayers);
            foreach (Collider character in characters)
            {
                if (character.GetComponent<CharacterManager>().isAlive && Vector3.Distance(transform.position, character.transform.position) <= closeDistance)
                {
                    closeDistance = Vector3.Distance(transform.position, character.transform.position);
                    player = character.transform;
                }
            }
            if (characters.Length == 0)
            {
                isTargetInReach = false;
                GoBackToStartPosition();
            }
            else if (player != null)
            {
                if (player.GetComponent<CharacterManager>().isAlive)
                {
                    isTargetInReach = true;
                }
            }
        }
    }

    private void FaceTarget()
    {
        if (TargetInRange())
        {
            Vector3 lookDir = (player.transform.position - transform.position).normalized;
            lookDir.y = 0;
            Quaternion _lookRotation = Quaternion.LookRotation(lookDir);

            transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * speedFaceTargetRotation);
            agent.enabled = false;
        }
        else
        {
            agent.enabled = true;
        }
    }

    private void OnDisable()
    {
        player = null;
        CancelInvoke("CheckApproching");
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, appprochingRange);
    }
}
