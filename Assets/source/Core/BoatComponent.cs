using UnityEngine;
using Unity.Mathematics;

// [RequireComponent(typeof(Mesh), typeof(Collider))]
public class BoatComponent : MonoBehaviour
{
    public static int TURRET_COUNT_MAX = 9;
    [SerializeField] public Transform turretChildenRoot;
    [SerializeField] public float turnSpeed = 20f;
    [SerializeField] private float moveSpeedMax = 100f;
    [SerializeField] private float moveSpeedInit = 5f;
    [SerializeField] private float moveSpeedAcc = 7f;
    [SerializeField] private float moveSpeedDec = -15f;
    private float moveSpeedCurr = 0.0f;

    private TurretManager turretManager;
    public Vector2 position2D {
        get { return new Vector2(transform.position.z, transform.position.x);}
    }

    public Vector2 forward2D {
        get { return new Vector2(transform.forward.z, transform.forward.x);}
    }

    /// <summary>
    /// The runtime list of turrents this ship manages
    /// </summary>
    public TurretSlot[] turretSlots;
    public int turretSlotCount {
        get { return turretSlots.Length; }
    }

    // Start is called before the first frame update
    void Start()
    {
        turretChildenRoot = transform.GetChild(0);
        turretManager = new TurretManager(this.transform);
    }

    void OnEnable() 
    {
        turretChildenRoot = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        turretManager.Update(turretSlots);
    }

    public GameObject GetTurretChild(int childIndex) {
        return turretChildenRoot.GetChild(childIndex).gameObject;
    }
    
    public void InstallTurret(int index, GameObject turretVariant)
    {
        Destroy(turretChildenRoot.GetChild(index).gameObject);
        GameObject new_turret = Instantiate(turretVariant);
        new_turret.transform.SetParent(turretChildenRoot);
        new_turret.transform.SetSiblingIndex(index);
        TurretSlot slot = turretSlots[index];
        slot.turretComp = new_turret.GetComponent<TurretComponent>();
        new_turret.transform.localPosition = slot.localPosition;
        Vector3 turret_forward = new_turret.transform.localEulerAngles;
        turret_forward.y = slot.initLocalAngle;
        new_turret.transform.localEulerAngles = turret_forward;
    }
}
