using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class RTSController : MonoBehaviour
{
    public RectTransform selectionBox;
    public LayerMask unitLayerMask;
    public float clickThreshold = 0.5f; // To distinguish between click and drag

    private Vector2 startPos;
    private bool isDragging = false;

    void Update()
    {
        MouseInput();
        ShiftInput();
        ControlGroupsInput();
    }
    void MouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // CheckButtonPressed
            startPos = Input.mousePosition;
            isDragging = false;
            selectionBox.gameObject.SetActive(false); // Hide Selection box initially
        }

        if (Input.GetMouseButton(0))
        {
            // Check if dragging or clicking
            if (Vector2.Distance(startPos, Input.mousePosition) > clickThreshold)
            {
                isDragging = true;
                selectionBox.gameObject.SetActive(true);
                UpdateSelectionBox(startPos, Input.mousePosition);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                // Finish drag selection
                selectionBox.gameObject.SetActive(false);
                SelectUnitsWithinBox();
            }
            else
            {
                // Perform single click selection
                SelectSingleUnit();
            }
        }
    }
    void UpdateSelectionBox(Vector2 start, Vector2 end)
    {
        Vector2 center = (start + end) / 2;
        selectionBox.position = center;

        Vector2 size = new Vector2(Mathf.Abs(start.x - end.x), Mathf.Abs(start.y - end.y));
        selectionBox.sizeDelta = size;
    }

    void SelectUnitsWithinBox()
    {
        Vector2 min = selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2);
        Vector2 max = selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2);

        var unitsBoxed = Physics2D.OverlapAreaAll(min, max);
        var unitsObject = new GameObject[unitsBoxed.Length];
        for (int i = 0; i <= unitsBoxed.Length; i++)
        {
            unitsObject[i] = unitsBoxed[i].gameObject;
        }
        UnitController.instance.DragSelect(unitsObject);
    }

    void SelectSingleUnit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, unitLayerMask);

        if (hit.collider != null)
        {
            GameObject clickedUnit = hit.collider.gameObject;
            UnitController.instance.DeSelectAll();
            UnitController.instance.Select(clickedUnit);
            // Add visual feedback or other logic for selection
        }
        else
        {
            UnitController.instance.DeSelectAll();
            // Handle deselection if needed
        }
    }

    void ShiftInput()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            UnitController.instance._shiftPressed = true;
        }
        else
        {
            UnitController.instance._shiftPressed = false;
        }
    }

    void ControlGroupsInput()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            for (int i = 0; i <= 9; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                {
                    UnitController.instance.AssignControlGroups(i);
                }
            }
        }

        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                UnitController.instance.CallControlGroups(i);
            }
        }
    }
    
}
