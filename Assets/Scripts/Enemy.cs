using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log($"{name}이 {other.gameObject.name}에 충돌");
        Destroy(gameObject);
    }
}
