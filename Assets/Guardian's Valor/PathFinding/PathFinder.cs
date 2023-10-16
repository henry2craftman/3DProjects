using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

public class PathFinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;
    public Vector2Int StartCoordinates { get { return startCoordinates; } }
    [SerializeField] Vector2Int destinationCoordinates;
    public Vector2Int DestinationCoordinates { get { return destinationCoordinates; } }

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

            // 시작시 startNode, destinationNode 만들어주기
            startNode = grid[startCoordinates];
            destinationNode = grid[destinationCoordinates];
        }
    }

    void Start()
    {
        GetNewPath();
    }

    public List<Node> GetNewPath()
    {
        return GetNewPath(startCoordinates);
    }

    public List<Node> GetNewPath(Vector2Int coordinates)
    {
        gridManager.ResetNode();
        BFS(coordinates);
        return BuildPath();
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

    void BFS(Vector2Int coordinates) // BreadthFirstSearch
    {
        startNode.isWalkable = true;
        destinationNode.isWalkable = true;

        frontier.Clear(); // 동적 BFS를 위해 초기화
        reached.Clear(); 

        bool isRunning = true; // 루프에서 나가기 위한 변수

        frontier.Enqueue(grid[coordinates]);
        reached.Add(coordinates, grid[coordinates]);

        while(frontier.Count > 0 && isRunning) // 탐험할 노드가 트리에 남아 있을 때 까지 반복
        {
            currentSearchNode = frontier.Dequeue();
            currentSearchNode.isExplored = true;

            ExploreNeighbors();

            if (currentSearchNode.coordinates == destinationCoordinates)
            {
                isRunning = false;
            }
        }
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

    public bool BlockPath(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            bool previousState = grid[coordinates].isWalkable;
            grid[coordinates].isWalkable = false;
            List<Node> newPath = GetNewPath(); // 다시 찾은 경로
            grid[coordinates].isWalkable = previousState;

            // 경로 탐색이 되지 않는 경우
            if(newPath.Count <= 1)
            {
                GetNewPath();
                return true;
            }
        }

        return false;
    }

    public void NotifyReceivers()
    {
        BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);
    }
}
