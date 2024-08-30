using UnityEngine;
using Unity.Mathematics;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class ToyBoat : MonoBehaviour
{
    public static int TURRET_COUNT_MAX = 9;

    [SerializeField]
    public Transform turretChildenRoot;

    public float turnSpeed = 20f;

    private float moveSpeedCurr = 0.0f;
    public float moveSpeedMax = 100f;
    public float moveSpeedInit = 5f;
    public float moveSpeedAcc = 7f;
    public float moveSpeedDec = -15f;

    public Vector2 position2D {
        get { return new Vector2(transform.position.x, transform.position.y);}
    }

    public Vector2 forward2D {
        get { return new Vector2(transform.forward.x, transform.forward.y);}
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
    }

    void OnEnable() 
    {
        turretChildenRoot = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        // var mouse_screen = Input.mousePosition;
        // var mouse_world = Camera.main.ScreenToWorldPoint(mouse_screen);
        // var target_pos = new Vector2(mouse_world.x, mouse_world.y);
        // Debug.Log(target_pos);
        // if (!position2D.Equals(target_pos))
        // {
        //     // Debug.Log($"{name}: Moving");
        //     MoveTo(target_pos);
        // }

        // foreach (TurretSlot slot in turretSlots) {
        //     // TODO: feed turret components relevant emeny information and other such stuffz
        // }
        var pos = position2D;
        pos += forward2D * moveSpeedInit;
        transform.SetPositionAndRotation(pos, transform.rotation);
    }

    /// returns the 
    private bool MoveTo(Vector2 pos) 
    {   
        Vector2 to_pos = pos - position2D;
        float angle_to_pos = Vector2.Angle(forward2D, to_pos.normalized);
        Vector2.Dot(forward2D, to_pos.normalized);
        Debug.Log($"Angle: {angle_to_pos} degrees");
        if (angle_to_pos > 0)
        {
            var turn_speed = turnSpeed;
            if (turnSpeed * Time.deltaTime > angle_to_pos)
            {
                transform.Rotate(Vector3.up, angle_to_pos);
            }
            else 
            {
                transform.Rotate(Vector3.up, turn_speed);
            }
        } 

        float dist_to_pos = to_pos.magnitude;
        Debug.Log($"Distance: {dist_to_pos} meters");
        if (dist_to_pos > 0)
        {
            // lerp between dec and acc based on time-to-intercept
            if (moveSpeedCurr == 0)
            {
                moveSpeedCurr = moveSpeedInit;
            }
            float acceleration_curr = math.lerp(moveSpeedDec, moveSpeedAcc, math.clamp((dist_to_pos/moveSpeedCurr - 2.5f)/2.5f, 0f, 1f));
            moveSpeedCurr += acceleration_curr;

            if (moveSpeedCurr * Time.deltaTime > dist_to_pos)
            {
                transform.Translate(transform.forward * dist_to_pos);
                moveSpeedCurr = 0f;
                return true;
            }
            else
            {
                transform.Translate(transform.forward * moveSpeedCurr);
            }
        }

        var pos_3d = transform.position;
        transform.position = new Vector3(pos_3d.x, pos_3d.y, 0f); // prevent movement in the z axis
        return false;
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
        slot.Turret = new_turret.GetComponent<TurretComponent>();
        new_turret.transform.localPosition = slot.LocalPosition;
        Vector3 turret_forward = new_turret.transform.localEulerAngles;
        turret_forward.z = slot.LocalEurlerZ;
        new_turret.transform.localEulerAngles = turret_forward;
    }
}
