using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    public Vector2Int coordinates; // 좌표
    public bool isWalkable; // 검색 가능 유무
    public bool isExplored; // pathfinding에 의해 탐색 되었는지
    public bool isPath; // 노드가 경로에 있는지
    public Node connectedTo; // 노드가 뻗어져 나온 붐 노드를 포함

    public Node(Vector2Int coordinate, bool isWalkable)
    {
        this.coordinates = coordinate;
        this.isWalkable = isWalkable;
    }
}
