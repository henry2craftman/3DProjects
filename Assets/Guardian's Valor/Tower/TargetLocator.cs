using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerEnemy = TowerDefense.Enemy;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform weapon;
    [SerializeField] ParticleSystem projectileParticle;
    [SerializeField] float range = 15f;
    Transform target;

    private void Update()
    {
        FindClosestTarget();
        AimWeapon();
    }

    private void FindClosestTarget()
    {
        TowerEnemy[] enemies = FindObjectsOfType<TowerEnemy>();
        Transform closestTarget = null;
        float maxDistance = Mathf.Infinity;

        foreach (TowerEnemy enemy in enemies)
        {
            float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);

            if(targetDistance < maxDistance)
            {
                closestTarget = enemy.transform;
                maxDistance = targetDistance;
            }
        }

        target = closestTarget;
    }

    private void AimWeapon()
    {
        float targetDistance = Vector3.Distance(transform.position, target.position);

        weapon.LookAt(target);

        if(targetDistance < range)
        {
            Attack(true);
        }
        else
        {
            Attack(false);
        }
    }

    void Attack(bool isActive)
    {
        var emissionModule = projectileParticle.emission;
        emissionModule.enabled = isActive;
    }
}
