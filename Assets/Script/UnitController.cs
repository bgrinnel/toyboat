using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _selectedUnits = new List<GameObject>();
    private static UnitController _instance;
    public static UnitController instance { get { return _instance; } }
    public bool _shiftPressed;
    public bool _holdingCtrl;
    public List<GameObject>[] unitGroups = new List<GameObject>[10];

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
        for (int i = 1; i < unitGroups.Length; i++) 
        {
            unitGroups[i] = new List<GameObject>();
        }
    }

    private void Update()
    {
        
    }
    // Start is called before the first frame update
    public void Select(GameObject unit)
    {
        if (!_shiftPressed) { _selectedUnits.Clear(); }
        _selectedUnits.Add(unit);
    }
    public List<GameObject> Selected()
    {
        return(_selectedUnits);
    }

    public void DeSelect(GameObject unit)
    {
        if (_selectedUnits.Contains(unit))
        {
            _selectedUnits.Remove(unit);
        }
    }

    public void DeSelectAll()
    {
        _selectedUnits.Clear();
    }

    public void DragSelect(Collider2D[] units)
    {
        if (!_shiftPressed) { _selectedUnits.Clear(); }
        foreach (var unit in units)
        {
            if (!_selectedUnits.Contains(unit.gameObject))
            {
                _selectedUnits.Add(unit.gameObject);
            }
        }
    }

    public void AssignControlGroups(int groupnumber)
    {
        foreach (var unit in _selectedUnits)
        {
            unitGroups[groupnumber].Add(unit);
        }
        Debug.Log("unit assigned to group " + groupnumber);
    }

    public void CallControlGroups(int grounpnumber)
    {
        _selectedUnits.Clear();

        foreach (var unit in unitGroups[grounpnumber])
        {
            _selectedUnits.Add(unit);
        }
    }
}
