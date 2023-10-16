using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using TMPro;
    using System;

    [ExecuteAlways]
    [RequireComponent(typeof(TextMeshPro))]
    public class CoordinateLabeler : MonoBehaviour
    {
        [SerializeField] Color defaultColor = Color.white;
        [SerializeField] Color blockColor = Color.gray;
        [SerializeField] Color exploredColor = Color.yellow;
        [SerializeField] Color pathColor = new Color(1f, 0.5f, 0f);
        TextMeshPro label;
        Vector2Int coordinates = new Vector2Int();
        GridManager gridManager;

        private void Awake()
        {
            gridManager = FindObjectOfType<GridManager>();

            label = GetComponent<TextMeshPro>();
            label.enabled = true;

            DisplayCoordinates();
        }

        void Update()
        {
            if (!Application.isPlaying)
            {
                DisplayCoordinates();
                UpdateOIbjectName();
            }

            SetLabelColor();

            ToggleLabels();
        }

        void ToggleLabels()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                label.enabled = !label.enabled;
            }
        }

        private void SetLabelColor()
        {
            if (gridManager == null) return;

            Node node = gridManager.GetNode(coordinates);

            if(node == null) return;

            if (!node.isWalkable)
            {
                label.color = blockColor;
            }
            else if(node.isPath) 
            {
                label.color = pathColor;
            }
            else if (node.isExplored)
            {
                label.color = exploredColor;
            }
            else
            {
                label.color = defaultColor;
            }

        }

        private void DisplayCoordinates()
        {
            if (gridManager == null) return;
            // 유니티 에디터 관련 코드는 빌드시 들어갈 수 없음
            coordinates.x = Mathf.RoundToInt(transform.parent.position.x / gridManager.UnityGridSize);
            coordinates.y = Mathf.RoundToInt(transform.parent.position.z / gridManager.UnityGridSize);

            label.text = $"{coordinates.x},{coordinates.y}";
        }

        void UpdateOIbjectName()
        {
            transform.parent.name = coordinates.ToString();
        }


    }
}

