using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

public class PathFinder : MonoBehaviour
{
    [SerializeField] Transform locator;
    [SerializeField] Vector2Int startCoordinates;
    [SerializeField] Vector2Int destinationCoordinates;

    Node startNode;
    Node destinationNode;
    Node currentSearchNode;

    Queue<Node> frontier = new Queue<Node>();
    Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>(); // 경로가 이미 탐험되었는지 확인용

    
    // BFS 방향 순서 설정
    Vector2Int[] directions = {Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left};
    GridManager gridManager;
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        if(gridManager != null)
        {
            grid = gridManager.Grid;
        }
    }

    void Start()
    {
        // 시작시 startNode, destinationNode 만들어주기
        startNode = gridManager.Grid[startCoordinates];
        destinationNode = gridManager.Grid[destinationCoordinates];

        StartCoroutine(BFS());

        
    }

    private void ExploreNeighbors()
    {
        //1. neighbors라는 empty list를 만든다.
        List<Node> neighbors = new List<Node>();
        //2. 모든 방향을 순회한다
        foreach(Vector2Int direction in directions)
        {
            //3. currentSearchNode르 부터 그 방향에 있는 노드의 좌표를 찾는다.
            Vector2Int neighborCoords = currentSearchNode.coordinates + direction; // ex) 최근 좌표가 2,2라면 오른쪽 vector인 1.0을 더해서 해당 방향의 좌표를 찾는다.

            //4. GridManager의 그리드를 확인해서 이웃 좌표가 있는지 확인한다.
            if(grid.ContainsKey(neighborCoords))
            {
                neighbors.Add(grid[neighborCoords]);

                //TODO:  테스트 후 지워주기
                //grid[neighborCoords].isExplored = true;
                //grid[currentSearchNode.coordinates].isPath = true;
            }

            //5. 인접 좌표가 존재한다면 그 노드를 가져와서 이웃 목록에 저장한다.
            foreach(Node neighbor in neighbors)
            {
                // 5-1. reached 노드의 딕셔너리가 현재 인접좌표를 담고있는지 확인
                if (!reached.ContainsKey(neighbor.coordinates) && neighbor.isWalkable)
                {
                    neighbor.connectedTo = currentSearchNode; // 트리 구성
                    reached.Add(neighbor.coordinates, neighbor);
                    frontier.Enqueue(neighbor);
                }
            }
        }
    }

    IEnumerator BFS() // BreadthFirstSearch
    {
        bool isRunning = true; // 루프에서 나가기 위한 변수

        frontier.Enqueue(startNode);
        reached.Add(startCoordinates, startNode);

        while(frontier.Count > 0 && isRunning) // 탐험할 노드가 트리에 남아 있을 때 까지 반복
        {
            currentSearchNode = frontier.Dequeue();
            currentSearchNode.isExplored = true;

            ExploreNeighbors();

            locator.position = new Vector3(currentSearchNode.coordinates.x * 10, 0, currentSearchNode.coordinates.y * 10);

            if (currentSearchNode.coordinates == destinationCoordinates)
            {
                isRunning = false;
            }

            yield return new WaitForSeconds(1);
        }

        BuildPath();
    }

    List<Node> BuildPath()
    {
        List<Node> path = new List<Node>();
        Node currentNode = destinationNode;

        path.Add(currentNode);
        currentNode.isPath = true;

        while(currentNode.connectedTo != null)
        {
            currentNode = currentNode.connectedTo;
            path.Add(currentNode);
            currentNode.isPath = true;
        }

        path.Reverse();

        return path;
    }
}
