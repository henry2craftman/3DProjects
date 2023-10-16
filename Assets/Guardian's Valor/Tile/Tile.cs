using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] Tower towerPrefab;
    [SerializeField] bool isPlaceable;
    public bool IsPlaceable { get { return isPlaceable; } }

    GridManager gridManager;
    PathFinder pathFinder;
    Vector2Int coordinates = new Vector2Int();

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<PathFinder>();
    }

    private void Start()
    {
        // 좌표 저장 준비
        if(gridManager != null)
        {
            // tile의 포지션을 좌표로 변환
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);

            if(!isPlaceable)
            {
                // 타워를 놓을 수 없는 곳이 적이 이동할 수 없는 곳 -> But.. PathFinding에 문제가 생김
                gridManager.BlockNode(coordinates);   
            }
        }
    }

    public bool GetIsPlacable()
    {
        return isPlaceable;
    }

    private void OnMouseDown()
    {
        if (gridManager.GetNode(coordinates).isWalkable && !pathFinder.BlockPath(coordinates))
        {
            // 타워가 생성되었다면 생성불가로 타워 자체에서, gridManager에서 설정!
            bool isSuccessful = towerPrefab.CreateTower(towerPrefab, transform.position);
            if(isSuccessful)
            {
                gridManager.BlockNode(coordinates);
                pathFinder.NotifyReceivers();
            }
        }
    }
}
