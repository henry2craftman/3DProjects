using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] Tower towerPrefab;
    [SerializeField] bool isPlaceable;
    public bool IsPlaceable { get { return isPlaceable; } }

    public bool GetIsPlacable()
    {
        return isPlaceable;
    }

    private void OnMouseDown()
    {
        if (isPlaceable)
        {
            // 타워가 생성되었다면 생성불가로!
            bool isPlaced = towerPrefab.CreateTower(towerPrefab, transform.position);
            //Instantiate(towerPrefab, transform.position, Quaternion.identity);
            isPlaceable = !isPlaced;
        }
    }
}
