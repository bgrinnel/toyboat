using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Diagnostics;

[CustomEditor(typeof(ToyBoat))]
public class ToyBoatEditor : Editor
{   
    private List<GameObject> turretVariants = new List<GameObject>();

    private const string baseBoatPrefabName = "BaseToyBoatPrefab";
    private const string baseTurretPrefabName = "BaseTurretPrefab";
    private const string emptyTurretPrefabName = "EmptyTurretPrefab";
    private GameObject emptyTurretPrefab;
    private bool bPrefabStageOpen;
    private bool bInstanceInPrefabStage {
        get { return PrefabStageUtility.GetCurrentPrefabStage() != null; }
    }
    
    private Rect[] FieldRects = new Rect[0];
    private int focusedTurret = -1;

    void OnEnable() 
    {
        var toy_boat = target as ToyBoat;
        focusedTurret = -1;

        // Debug.Log($"Enabling Editor for: \"{toy_boat.name}.InPrefabStage({bInstanceInPrefabStage})\"");
        PrefabStage.prefabStageOpened += this.OnPrefabStageOpen;
        PrefabStage.prefabStageClosing += this.OnPrefabStageClosing;
        bPrefabStageOpen = bInstanceInPrefabStage;      // OnEnable we set ourself to whatever the instance of enabling was
        
        
        var prefab_stage = PrefabStageUtility.GetCurrentPrefabStage();
        if (prefab_stage)
        {
            if (prefab_stage.prefabContentsRoot.name != baseBoatPrefabName)
            {
                turretVariants = GetTurretVariants();
            }
        }
        else 
        {
            turretVariants = GetTurretVariants();
        }
    }

    void OnDisable()
    {
        var toy_boat = target as ToyBoat;

        string obj_name = toy_boat.IsDestroyed() ? "Destroyed" : toy_boat.name;
        // Debug.Log($"Disabling Editor for: \"{obj_name}.InPrefabStage({bInstanceInPrefabStage})\"");
        PrefabStage.prefabStageOpened -= this.OnPrefabStageOpen;
        PrefabStage.prefabStageClosing -= this.OnPrefabStageClosing;
    }

    void OnPrefabStageOpen(PrefabStage stage)
    {
        bPrefabStageOpen = true;
        // Debug.Log("Prefab Opened");
    }

    void OnPrefabStageClosing(PrefabStage stage)
    {
        bPrefabStageOpen = false;
        // Debug.Log("Prefab Closed");
    }

    /// <summary>
    /// Constructs the list of all TurretBase-derived prefab variants
    /// </summary>
    /// <returns>list of prefab-variant GameObjets</returns>
    private List<GameObject> GetTurretVariants() 
    {
        var turret_variants = new List<GameObject>();
        // Find all prefabs that are derived from the base prefab
        string[] guids = AssetDatabase.FindAssets("t:Prefab");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab != null && prefab.GetComponent<TurretComponent>() != null && prefab.name != baseTurretPrefabName)
            {
                turret_variants.Add(prefab);
                if (prefab.name == emptyTurretPrefabName) emptyTurretPrefab = prefab;
            }
        }
        return turret_variants;
    }

    /// <summary>
    /// An Editor only function for modifying a ToyBoat variant's TurretSlots array; i.e. TurretSlots.Length += modVal
    /// </summary>
    /// <param name="modVal">Added to the length of the TurretSlots array; </param>
    public void Editor_ModifyTurretSlots(int modVal) {
        var toy_boat = target as ToyBoat;

        var new_slots = new TurretSlot[toy_boat.turretSlots.Length+modVal];
        for (int i = 0; i < new_slots.Length; ++i) 
        {
            if (i < toy_boat.turretSlots.Length)
            {
                new_slots[i] = toy_boat.turretSlots[i];
            }
        }
        toy_boat.turretSlots = new_slots;
    }

    /// <summary>
    /// Makes sure assets are valid prefabs
    /// </summary>
    public void Editor_Validate() {
        ToyBoat toy_boat = target as ToyBoat;
        
        if (toy_boat.turretChildenRoot == null) toy_boat.turretChildenRoot = toy_boat.transform.GetChild(0);

        int num_turrets = toy_boat.turretChildenRoot.childCount;
        // my oopsie daisy code
        if (num_turrets > ToyBoat.TURRET_COUNT_MAX)
        {
            int num_slots = toy_boat.turretSlotCount;
            if (num_slots >= ToyBoat.TURRET_COUNT_MAX)
            {
                Editor_ModifyTurretSlots(ToyBoat.TURRET_COUNT_MAX - num_slots);
            }
            for (int i = 0; i < num_turrets - num_slots; ++i) 
            {
                GameObject turret = toy_boat.turretChildenRoot.GetChild(toy_boat.turretChildenRoot.childCount - 1).gameObject;
                DestroyTurretObject(turret);
            }
        }

        if (toy_boat.turretSlots == null) 
        {
            toy_boat.turretChildenRoot = toy_boat.transform.GetChild(0);
            toy_boat.turretSlots = new TurretSlot[num_turrets];

            for (int i = 0; i < num_turrets; ++i) {
                Transform child = toy_boat.turretChildenRoot.GetChild(i);
                var new_slot = new TurretSlot(child.localPosition, child.localEulerAngles.z);
                new_slot.Turret = child.GetComponent<TurretComponent>();
                toy_boat.turretSlots[i] = new_slot;
            }
        }
        else if (toy_boat.turretSlotCount < num_turrets) 
        {
            int num_slots = toy_boat.turretSlotCount;
            Editor_ModifyTurretSlots(num_turrets - num_slots);
            for (int i = num_slots; i < num_turrets; ++i) 
            {
                Transform child = toy_boat.turretChildenRoot.GetChild(i);
                var new_slot = new TurretSlot(child.localPosition, child.localEulerAngles.z);
                new_slot.Turret = child.GetComponent<TurretComponent>();
                toy_boat.turretSlots[i] = new_slot;
            }
        }
        else if (toy_boat.turretSlotCount == num_turrets)
        {
            for (int i = 0; i < num_turrets; ++i) {
                Transform child = toy_boat.turretChildenRoot.GetChild(i);
                toy_boat.turretSlots[i].LocalPosition = child.localPosition;
                toy_boat.turretSlots[i].LocalEurlerZ = child.localEulerAngles.z;
                toy_boat.turretSlots[i].Turret = child.GetComponent<TurretComponent>();
            }
        }
    }
    
    public GameObject MakeTurretObject(GameObject gameObject)
    {
        if (bPrefabStageOpen)
        {
            var stage = PrefabStageUtility.GetCurrentPrefabStage();
            var stage_boat_script = stage.prefabContentsRoot.GetComponent<ToyBoat>();
            var turret = PrefabUtility.InstantiatePrefab(gameObject, stage.prefabContentsRoot.transform) as GameObject;
            turret.transform.SetParent(stage_boat_script.turretChildenRoot);
            return turret;
        }
        else
        {
            var toy_boat = target as ToyBoat;
            return Instantiate(gameObject, toy_boat.turretChildenRoot);
        }
    }

    public void DestroyTurretObject(GameObject gameObject)
    {
        if (bPrefabStageOpen)
        {
            DestroyImmediate(gameObject, true);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }
    
    /// <summary>
    /// Editor Only turret slot adder (adds to the end of array)
    /// </summary>
    public void Editor_AddTurretSlot() {
        ToyBoat toy_boat = target as ToyBoat;

        var new_slot = new TurretSlot(new Vector2(), 0);
        Editor_ModifyTurretSlots(1);
        GameObject empty_turret = MakeTurretObject(emptyTurretPrefab);
        
        new_slot.Turret = empty_turret.GetComponent<TurretComponent>();
        toy_boat.turretSlots[toy_boat.turretSlots.Length-1] = new_slot;
    }
    /// <summary>
    /// Editor only turret slot remover (removes from end of array; FILO)
    /// </summary>
    public void Editor_RemoveTurretSlot() {
        ToyBoat toy_boat = target as ToyBoat;

        if (toy_boat.turretSlotCount > 0)
        {
            GameObject last_child = toy_boat.turretChildenRoot.GetChild(toy_boat.turretChildenRoot.childCount-1).gameObject;
            Editor_ModifyTurretSlots(-1);
            DestroyTurretObject(last_child);
        }
    }

    /// <summary>
    /// Removes all turret slots from a prefab
    /// </summary>
    public void Editor_RemoveAllTurretSlots() {
        ToyBoat toy_boat = target as ToyBoat;

        while (toy_boat.turretChildenRoot.childCount > 0) {
            DestroyTurretObject(toy_boat.turretChildenRoot.GetChild(0).gameObject);
        }
        Editor_ModifyTurretSlots(-toy_boat.turretSlots.Length);
    }

    /// <summary>
    /// Installs a given turret prefab into a turret slot
    /// </summary>
    /// <param name="index">the index of TurretSlots to modify</param>
    /// <param name="turretVariant">the turret variant to be installed</param>
    public void Editor_InstallTurret(int index, GameObject turretVariant) {
        ToyBoat toy_boat = target as ToyBoat;

        DestroyTurretObject(toy_boat.turretChildenRoot.GetChild(index).gameObject);
        GameObject new_turret = MakeTurretObject(turretVariant);
        new_turret.transform.SetSiblingIndex(index);
        TurretSlot slot = toy_boat.turretSlots[index];
        slot.Turret = new_turret.GetComponent<TurretComponent>();
        new_turret.transform.localPosition = slot.LocalPosition;
        Vector3 turret_forward = new_turret.transform.localEulerAngles;
        turret_forward.z = slot.LocalEurlerZ;
        new_turret.transform.localEulerAngles = turret_forward;

    }
    private void DrawBaseInspector()
    {
        ToyBoat toy_boat = target as ToyBoat;
        ToyBoat.TURRET_COUNT_MAX = EditorGUILayout.IntField(new GUIContent("Turret Count Max"),  ToyBoat.TURRET_COUNT_MAX);
        
        var empty_turret_prefab = EditorGUILayout.ObjectField("Empty Turret Prefab", emptyTurretPrefab, typeof(GameObject), false);
        if (empty_turret_prefab)
        {
            emptyTurretPrefab = empty_turret_prefab as GameObject;
        }
        EditorGUILayout.LabelField($"Turret Slots #: {toy_boat.turretSlotCount} | Turret Chilren #: {toy_boat.turretChildenRoot.childCount}");
    }

    /// <summary>
    /// A wrapper for drawing the custom GUI
    /// </summary>
    private void DrawVariantInspector() 
    {
        ToyBoat toy_boat = target as ToyBoat;

        if (GUILayout.Button(new GUIContent("Remove All Turret Slots")))
        {
            Editor_RemoveAllTurretSlots();
        }
        EditorGUILayout.LabelField($"Turret Slots #: {toy_boat.turretSlotCount} | Turret Chilren #: {toy_boat.turretChildenRoot.childCount}");
        if (GUILayout.Button(new GUIContent("Add Turret Slot")))
        {
            Editor_AddTurretSlot();
        }
        if (GUILayout.Button(new GUIContent("Remove Turret Slot")))
        {
            Editor_RemoveTurretSlot();
        }
        if (FieldRects.Length != toy_boat.turretSlotCount)
        {
            FieldRects = new Rect[toy_boat.turretSlotCount];
        }
        for (int slot = 0; slot < toy_boat.turretSlotCount; ++slot) 
        {
            var turret =  toy_boat.GetTurretChild(slot);
            string variant_name = turret.gameObject.name.Split('(')[0];
            int start_idx = turretVariants.ToListPooled().FindIndex(v => v.name == variant_name);
            FieldRects[slot] = EditorGUILayout.GetControlRect();
            int index = EditorGUI.Popup(FieldRects[slot], "Select Turret Variant", start_idx, turretVariants.Select(v => v.name).ToArray());
            if (index >= 0) {
                if (variant_name != turretVariants[index].name)
                {
                    Editor_InstallTurret(slot, turretVariants[index]);
                }
            }
            if (slot == focusedTurret)
            {
                Vector3 angles = turret.transform.localRotation.eulerAngles;
                float z = EditorGUILayout.Slider(angles.z, 0.0f, 360f);
                if (z != angles.z)
                {
                    turret.transform.SetLocalPositionAndRotation(turret.transform.localPosition, Quaternion.Euler(0, 0, z));
                    toy_boat.turretSlots[slot].LocalEurlerZ = z;
                }
                Vector3 pos3 = turret.transform.localPosition;
                Vector3 pos = new Vector3(pos3.x, pos3.y);
                Vector2 new_pos = EditorGUILayout.Vector2Field("Local", pos);
                if (!new_pos.Equals(pos))
                {
                    turret.transform.SetLocalPositionAndRotation(new Vector3(new_pos.x, new_pos.y, 0), turret.transform.localRotation);
                    toy_boat.turretSlots[slot].LocalPosition = new_pos;
                }
            }
        }
        HandleGUIInputEvent();
    }

    public void HandleGUIInputEvent()
    {
        Event e = Event.current;

        if (e.type == EventType.MouseDown && e.button == 0) // Check if left mouse button was clicked
        {
            for (int i = 0; i < FieldRects.Length; ++i)
            {
                var rect = FieldRects[i];
                if (rect.Contains(e.mousePosition))
                {
                    focusedTurret = i;
                }
            }
        }
    }

    public override void OnInspectorGUI()
    {
        ToyBoat toy_boat = target as ToyBoat;

        if (toy_boat.isActiveAndEnabled)
        {
            Editor_Validate();

            var prefab_stage = PrefabStageUtility.GetCurrentPrefabStage();
            if (bPrefabStageOpen) 
            {
                string prefab_name = prefab_stage.prefabContentsRoot.name;
                EditorGUILayout.LabelField($"PrefabStage : {prefab_name}");
                EditorGUI.BeginChangeCheck();

                if (prefab_name == "ToyBoatPrefabBase")
                {
                    DrawBaseInspector();
                }
                else
                {
                    DrawVariantInspector();
                }

                Editor_Validate();

                if (EditorGUI.EndChangeCheck()) 
                {
                    PrefabUtility.SaveAsPrefabAsset(prefab_stage.prefabContentsRoot, prefab_stage.assetPath);
                }
            }
            else 
            {
                Editor_Validate();

                EditorGUILayout.LabelField($"Not In PrefabStage");
                DrawVariantInspector();

                Editor_Validate();
            }
        }
        else
        {
            EditorGUILayout.LabelField("Asset is Enabling and/or Activating");
        }
    } 

    // TODO: pretened I didn't spend so much time on this
    // void OnSceneGUI() 
    // { 
    //     if (bEnabled && bInPrefabStage)
    //     {  
    //         Event e = Event.current;

    //         if (e.type == EventType.MouseDown && e.button == 0) // Left mouse button click
    //         {
    //             // Create a ray from the mouse position
    //             Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
    //             Handles.color = Color.green;
    //             Debug.DrawLine(ray.origin, ray.origin + ray.direction * 10);
    //             if (Physics.Raycast(ray, out RaycastHit hit))
    //             {
    //                 Vector3 new_point = PrefabStageUtility.GetCurrentPrefabStage().prefabContentsRoot.transform.InverseTransformPoint(hit.point);
    //                 // TODO: fix the hit
    //                 Debug.Log($"Hit location: {new_point}");
    //                 // If we hit something, get the reference to the GameObject
    //                 GameObject hitObject = hit.collider.gameObject;
    //                 var turret = hitObject.GetComponent<TurretComponent>();
    //                 var editor_obj = hitObject.GetComponent<EditorObject>();
    //                 if (turret != null) 
    //                 {
    //                     Debug.Log("Handling Turret Hit Event");
    //                     focusedTurret = turret;
    //                     e.Use();        
    //                 }
    //                 else if (editor_obj != null && !foucsedEditorObj) 
    //                 {
    //                     Debug.Log("Handling editor obj Hit Event");
    //                     foucsedEditorObj = editor_obj;
    //                     e.Use();
    //                 }
    //                 else if (!editor_obj && !turret)
    //                 {
    //                     Debug.Log("Handling Miss Hit Event");
    //                     focusedTurret = null;
    //                 }
    //             }
    //         }
    //         else if (e.type == EventType.MouseUp && e.button == 0) // left mouse button up
    //         {
    //             foucsedEditorObj = null;
    //         }
    //         else if (e.type == EventType.MouseDrag && e.button == 0 && foucsedEditorObj != null)
    //         {
    //             Camera cam;
    //             if (bInPrefabStage)
    //             {
    //                 cam = SceneView.lastActiveSceneView.camera;
    //             }
    //             else
    //             {
    //                 cam = Camera.main;
    //             }
                
    //             switch (foucsedEditorObj.GetEditorType())
    //             {
    //                 case EditorObject.Editor.Rotator:
    //                     var screen_pos = HandleUtility.GUIPointToScreenPixelCoordinate(Event.current.mousePosition);
    //                     var world_pos = cam.ScreenToWorldPoint(new Vector3(screen_pos.x, screen_pos.y,  0f));
    //                     foucsedEditorObj.EditorMove(world_pos);
    //                     break;
    //                 case EditorObject.Editor.Translator:
    //                     Vector2 screen_delta = HandleUtility.GUIPointToScreenPixelCoordinate(e.delta);
    //                     Vector3 world_delta = cam.ScreenToWorldPoint(new Vector3(screen_delta.x, screen_delta.y, 0f));
    //                     foucsedEditorObj.EditorMove(world_delta);
    //                     break;
    //                 default:
    //                     Debug.LogWarning("A base class EditorObject exists in the scene");
    //                     break;
    //             }
    //         }

    //     }
    // }  
}
