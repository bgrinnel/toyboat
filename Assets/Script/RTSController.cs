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

    private void Start()
    {
        selectionBox.gameObject.SetActive(false);
    }
    void Update()
    {
        MouseInput();
        ShiftAndCtrlInput();
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
                isDragging = false;
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
        var start = Camera.main.ScreenToWorldPoint(startPos);
        var end = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var unitsBoxed = Physics2D.OverlapAreaAll(start, end);
        UnitController.instance.DragSelect(unitsBoxed);
    }

    void SelectSingleUnit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, unitLayerMask);

        if (hit.collider != null)
        {
            Debug.Log("hit");
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

    void ShiftAndCtrlInput()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            UnitController.instance._shiftPressed = true;
        }
        else
        {
            UnitController.instance._shiftPressed = false;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            UnitController.instance._holdingCtrl = true;
        }
        else
        {
            UnitController.instance._holdingCtrl = false;
        }
    }

    void ControlGroupsInput()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.Alpha0)) { UnitController.instance.AssignControlGroups(0); }
            else if (Input.GetKeyDown(KeyCode.Alpha1)) { UnitController.instance.AssignControlGroups(1); }
            else if (Input.GetKeyDown(KeyCode.Alpha2)) { UnitController.instance.AssignControlGroups(2); }
            else if (Input.GetKeyDown(KeyCode.Alpha3)) { UnitController.instance.AssignControlGroups(3); }
            else if (Input.GetKeyDown(KeyCode.Alpha4)) { UnitController.instance.AssignControlGroups(4); }
            else if (Input.GetKeyDown(KeyCode.Alpha5)) { UnitController.instance.AssignControlGroups(5); }
            else if (Input.GetKeyDown(KeyCode.Alpha6)) { UnitController.instance.AssignControlGroups(6); }
            else if (Input.GetKeyDown(KeyCode.Alpha7)) { UnitController.instance.AssignControlGroups(7); }
            else if (Input.GetKeyDown(KeyCode.Alpha8)) { UnitController.instance.AssignControlGroups(8); }
            else if (Input.GetKeyDown(KeyCode.Alpha9)) { UnitController.instance.AssignControlGroups(9); }
        }

        if (Input.GetKeyDown(KeyCode.Alpha0)) { UnitController.instance.CallControlGroups(0); }
        else if (Input.GetKeyDown(KeyCode.Alpha1)) { UnitController.instance.CallControlGroups(1); }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) { UnitController.instance.CallControlGroups(2); }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) { UnitController.instance.CallControlGroups(3); }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) { UnitController.instance.CallControlGroups(4); }
        else if (Input.GetKeyDown(KeyCode.Alpha5)) { UnitController.instance.CallControlGroups(5); }
        else if (Input.GetKeyDown(KeyCode.Alpha6)) { UnitController.instance.CallControlGroups(6); }
        else if (Input.GetKeyDown(KeyCode.Alpha7)) { UnitController.instance.CallControlGroups(7); }
        else if (Input.GetKeyDown(KeyCode.Alpha8)) { UnitController.instance.CallControlGroups(8); }
        else if (Input.GetKeyDown(KeyCode.Alpha9)) { UnitController.instance.CallControlGroups(9); }
    }
}
