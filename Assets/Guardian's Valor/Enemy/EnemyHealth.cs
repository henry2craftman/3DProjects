using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerEnemy = TowerDefense.Enemy;

[RequireComponent(typeof(TowerEnemy))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHitPoints = 3;
    [Tooltip("적이 죽을 때 maxHitPoints에 추가할 값")]
    [SerializeField] int difficultyIncrease = 1;
    int currentHitPoints = 0;

    TowerEnemy enemy;

    void OnEnable()
    {
        currentHitPoints = maxHitPoints;
    }

    private void Start()
    {
        enemy = GetComponent<TowerEnemy>();
    }

    private void OnParticleCollision(GameObject other)
    {
        ProcessHit();
    }

    private void ProcessHit()
    {
        currentHitPoints--;

        if(currentHitPoints <= 0)
        {
            gameObject.SetActive(false);
            maxHitPoints += difficultyIncrease;
            enemy.RewardGold();
        }
    }
}
