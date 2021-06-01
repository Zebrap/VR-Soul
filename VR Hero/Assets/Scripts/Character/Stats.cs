using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public float health = 100.0f;
    public float maxHealth = 100.0f;

    public float attack = 50.0f;
    private float baseAttack = 50.0f;

    public float attackRate = 2f;

    public float defance = 10.0f;
    private float baseDefence = 10.0f;
    public delegate void EventHandler();
    public event EventHandler OnDieCallBack;
    public void GetHit(float dmg){
        health -= Mathf.Clamp(dmg - defance, 0, dmg);
        if(health <= 0){
            health = 0;
            OnDieCallBack?.Invoke();
        }
    }

}
