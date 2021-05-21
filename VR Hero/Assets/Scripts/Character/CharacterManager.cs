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

    private void Awake() {
        stats = GetComponent<Stats>();
        animator = GetComponent<Animator>();

        characterController = GetComponent<CharacterController>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        capsuleCollider.center = characterController.center;
        capsuleCollider.radius = characterController.radius;
        capsuleCollider.height = characterController.height;

        stats.OnDieCallBack += Die;
    }

    public void GetHit(float dmg)
    {
        if (isAlive)
        {
            canMove = false;
            isAttack= false;
            StopCoroutine("AttackRate");
            StartCoroutine("HurtDelay");
            animator.SetTrigger("Hurt");
            stats.GetHit(dmg);
        }
    }

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

    IEnumerator HurtDelay(){
        yield return new WaitForSeconds(0.4f);
        canMove = true;
    }

    IEnumerator AttackRate(){
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

    private void Die(){
        animator.SetBool("Death",true);
        isAlive = false;
        StartCoroutine(Decompose());
    }
    private IEnumerator Decompose()
    {
        characterController.enabled = false;
        yield return new WaitForSeconds(2.0f);
        float timer = 5.0f;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            transform.position += new Vector3(0, -Time.deltaTime * 0.6f, 0);
            yield return null;
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
