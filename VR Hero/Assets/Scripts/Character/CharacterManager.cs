using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    Stats stats;

    Animator animator;
    public bool isAlive = true;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    [HideInInspector]
    public bool isAttack = false;

    public bool canMove = true;

    private CharacterController characterController;
    private CapsuleCollider capsuleCollider;

    public delegate void EventHandlerGetHit(float hp, float maxHp);
    public event EventHandlerGetHit OnGetHit;

    public delegate void EventHandlerOnDie();
    public event EventHandlerOnDie OnDieEvent;
    private void Awake()
    {
        stats = GetComponent<Stats>();
        animator = GetComponent<Animator>();

        characterController = GetComponent<CharacterController>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        capsuleCollider.center = characterController.center;
        capsuleCollider.radius = characterController.radius;
        capsuleCollider.height = characterController.height;

        stats.OnDieCallBack += Die;
    }
    private void Start() {
        if(speciallAttackPrefab!=null)
            mySpecialAttack = Instantiate(speciallAttackPrefab).GetComponent<SpecialAttack>();
    }

    public void InitPlayerOnMe()
    {
        OnGetHit?.Invoke(stats.health, stats.maxHealth);
    }

    #region  Combat Methods

    public void Attack()
    {
        if (!isAttack)
        {
            isAttack = true;
            canMove = false;
            animator.SetTrigger("Attack");
            StartCoroutine("AttackRate");
        }
    }

    IEnumerator AttackRate()
    {
        yield return new WaitForSeconds(stats.attackRate);
        isAttack = false;
        canMove = true;
    }

    public void CheckAttack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.GetComponent<CharacterManager>())
            {
                enemy.GetComponent<CharacterManager>().GetHit(stats.attack);
            }
        }
    }

    #endregion

    #region  Speciall Attack

    private SpecialAttack mySpecialAttack;

    public Transform speciallAttackPrefab;
    public void SpecialAttack()
    {
        if (mySpecialAttack != null)
        {
            if (mySpecialAttack.Cast(attackPoint.position, new Vector3(transform.forward.x * 100f, transform.forward.y + attackPoint.position.y, transform.forward.z * 100f), enemyLayers))
            {
                SpeciallAttackAnimation();
            }
        }
    }

    private void SpeciallAttackAnimation()
    {

    }
    #endregion

    #region  Get Hit Methods
    public void GetHit(float dmg)
    {
        if (isAlive)
        {
            canMove = false;
            isAttack = false;
            StopCoroutine("AttackRate");
            StartCoroutine("HurtDelay");
            animator.SetTrigger("Hurt");
            stats.GetHit(dmg);
            OnGetHit?.Invoke(stats.health, stats.maxHealth);
        }
    }
    IEnumerator HurtDelay()
    {
        yield return new WaitForSeconds(0.4f);
        canMove = true;
    }
    private void Die()
    {
        OnDieEvent?.Invoke();
        animator.SetBool("Death", true);
        isAlive = false;
        StartCoroutine("Decompose");
    }
    private IEnumerator Decompose()
    {
        characterController.enabled = false;
        capsuleCollider.enabled = false;
        yield return new WaitForSeconds(2.0f);
        float timer = 5.0f;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            transform.position += new Vector3(0, -Time.deltaTime * 1.5f, 0);
            yield return null;
        }
        Destroy(gameObject);
    }

    private void OnDestroy() {
        if(mySpecialAttack!=null)
            Destroy(mySpecialAttack.gameObject, 10f);
    }

    #endregion
    public void SelectInteract()
    {
        if (isAlive)
        {
            FindObjectOfType<ChangeCharacter>().ChangeToNewCharacter(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    #region Jump Methods
    [HideInInspector]
    public bool isJumping;

    internal void Jump()
    {
        isJumping = true;
        animator.SetTrigger("Jump");
        StartCoroutine("EndJump");
    }

    IEnumerator EndJump()
    {
        yield return new WaitForSeconds(1.2f);
        isJumping = false;
    }
    #endregion
}
