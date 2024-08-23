using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class ToyBoat : MonoBehaviour
{
    
    [SerializeField]
    public static GameObject TurretBasePrefab;
    
    [SerializeField]
    public static GameObject EmptyTurretPrefab;
    
    [SerializeField]
    public Transform TurretChildenRoot;

    /// <summary>
    /// The runtime list of turrents this ship manages
    /// </summary>
    public TurretSlot[] TurretSlots;

    // Start is called before the first frame update
    void Start()
    {
        TurretChildenRoot = transform.GetChild(0);
    }

    void OnEnable() 
    {
        TurretChildenRoot = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (TurretSlot slot in TurretSlots) {
            // TODO: feed turret components relevant emeny information and other such stuffz
        }
    }


    public int TurretSlotsCount() {
        return TurretSlots.Length;
    }

    public GameObject GetTurretChild(int childIndex) {
        return TurretChildenRoot.GetChild(childIndex).gameObject;
    }

}
