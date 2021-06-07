using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    [HideInInspector]
    public NavMeshAgent agent;
    private Animator animator;

    public Transform targetTransform;
    private Vector3 targetPos;

    public bool isTargetInReach = false;
    public bool isApprochingTarget = false;
    public float appprochingRange = 5f;
    private Transform player;

    private float speedSmoothTime = 0.1f;
    public bool isAttack;

    private CharacterManager characterManager;
    public float speedFaceTargetRotation = 2f;

    public float timeToAttack = 0.3f;

    private NavMeshPath path;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        characterManager = GetComponent<CharacterManager>();
        path = new NavMeshPath();
        if (targetTransform != null)
        {
            targetPos = targetTransform.position;
        }
        else
        {
            targetPos = transform.position;
        }
    }

    public void SetMyTargetTransform(Transform target)
    {
        targetTransform = target;
        targetPos = targetTransform.position;
    }

    private void OnEnable()
    {
        InvokeRepeating("CheckApproching", 1f, 0.3f);
    }

    private void Update()
    {
        if (characterManager.isAlive && isTargetInReach && characterManager.canMove && player != null)
        {
            if (player.GetComponent<GetHitObject>().isAlive)
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
        yield return new WaitForSeconds(timeToAttack);
        characterManager.Attack();
    }

    public void GoBackToStartPosition()
    {
        if (characterManager.isAlive)
        {
            agent.enabled = true;
            agent.isStopped = false;
            isApprochingTarget = false;
            agent.SetDestination(targetPos);
        }
    }
    public LineRenderer lineRenderer;

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
                if (agent.pathPending == false)
                {
                    agent.isStopped = false;
                    agent.CalculatePath(player.position, path);
                    Vector3[] corners = path.corners;
                    lineRenderer.positionCount = corners.Length;
                    lineRenderer.SetPositions(corners);
                    if (path.status == NavMeshPathStatus.PathPartial && path.status == NavMeshPathStatus.PathInvalid)
                    {
                      //  agent.SetDestination(player.position);
                    }else if(path.status == NavMeshPathStatus.PathComplete){
                        agent.SetDestination(player.position);
                    }
                     //   agent.SetPath(path);
                }
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
            if (characterManager.FindTargetInRange().Count > 0)
            {
                return true;
            }
            else
            {
                return false;
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
                if (character.GetComponent<GetHitObject>().isAlive && Vector3.Distance(transform.position, character.transform.position) <= closeDistance)
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
                if (player.GetComponent<GetHitObject>().isAlive)
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
            agent.enabled = false;
            Vector3 lookDir = (player.transform.position - transform.position).normalized;
            lookDir.y = 0;
            Quaternion _lookRotation = Quaternion.LookRotation(lookDir);

            transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * speedFaceTargetRotation);
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
