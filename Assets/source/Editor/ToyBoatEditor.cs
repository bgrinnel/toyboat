using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(ToyBoat))]
public class ToyBoatEditor : Editor
{
    
    private List<GameObject> TurretVariants = new List<GameObject>();

    private string BoatPrefabBaseName = "ToyBoatPrefabBase";

    void OnEnable() 
    {
        ToyBoat toy_boat = target as ToyBoat;

        TurretVariants.Clear();

        var prefab_stage = PrefabStageUtility.GetCurrentPrefabStage();
        if (prefab_stage)
        {
            if (prefab_stage.name != "ToyBoatPrefabBase")
            {
                TurretVariants = GetTurretVariants();
            }
        }
        else 
        {
            TurretVariants = GetTurretVariants();
        }
    }

    private List<GameObject> GetTurretVariants() 
    {
        var turret_variants = new List<GameObject>();
        // Find all prefabs that are derived from the base prefab
        string[] guids = AssetDatabase.FindAssets("t:Prefab");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab != null && prefab.GetComponent<TurretComponent>() != null && prefab.name != BoatPrefabBaseName)
            {
                turret_variants.Add(prefab);
                Debug.Log($"Turret Variant: \"{prefab.name}\"");
            }
        }
        return turret_variants;
    }

    public void Editor_ModifyTurretSlots(int modVal) {
        var toy_boat = target as ToyBoat;

        var new_slots = new TurretSlot[toy_boat.TurretSlots.Length+modVal];
        for (int i = 0; i < new_slots.Length; ++i) 
        {
            if (i < toy_boat.TurretSlots.Length)
            {
                new_slots[i] = toy_boat.TurretSlots[i];
            }
        }
        toy_boat.TurretSlots = new_slots;
    }

    public void Editor_Validate() {
        ToyBoat toy_boat = target as ToyBoat;
        
        if (toy_boat.TurretChildenRoot == null) toy_boat.TurretChildenRoot = toy_boat.transform.GetChild(0);

        int num_turrets = toy_boat.TurretChildenRoot.childCount;
        if (toy_boat.TurretSlots == null) 
        {
            toy_boat.TurretChildenRoot = toy_boat.transform.GetChild(0);
            toy_boat.TurretSlots = new TurretSlot[num_turrets];

            for (int i = 0; i < num_turrets; ++i) {
                Transform child = toy_boat.TurretChildenRoot.GetChild(i);
                var new_slot = new TurretSlot(child.localPosition, child.localEulerAngles.z);
                new_slot.Turret = child.GetComponent<TurretComponent>();
                toy_boat.TurretSlots[i] = new_slot;
            }
        }
        else if (toy_boat.TurretSlotsCount() < num_turrets) 
        {
            int num_slots = toy_boat.TurretSlotsCount();
            Editor_ModifyTurretSlots(num_turrets - num_slots);
            for (int i = num_slots; i < num_turrets; ++i) 
            {
                Transform child = toy_boat.TurretChildenRoot.GetChild(i);
                var new_slot = new TurretSlot(child.localPosition, child.localEulerAngles.z);
                new_slot.Turret = child.GetComponent<TurretComponent>();
                toy_boat.TurretSlots[i] = new_slot;
            }
        }
        else if (toy_boat.TurretSlotsCount() == num_turrets)
        {
            for (int i = 0; i < num_turrets; ++i) {
                Transform child = toy_boat.TurretChildenRoot.GetChild(i);
                toy_boat.TurretSlots[i].LocalPosition = child.localPosition;
                toy_boat.TurretSlots[i].LocalEurlerZ = child.localEulerAngles.z;
                toy_boat.TurretSlots[i].Turret = child.GetComponent<TurretComponent>();
            }
        }
        else
        {
            Debug.LogWarning("Unhandled ToyBoat Asset State");
        }
    }
    
    public void Editor_AddTurretSlot() {
        ToyBoat toy_boat = target as ToyBoat;

        var new_slot = new TurretSlot(new Vector2(), 0);
        Editor_ModifyTurretSlots(1);
        GameObject empty_turret = Instantiate(ToyBoat.EmptyTurretPrefab, toy_boat.TurretChildenRoot);
        
        new_slot.Turret = empty_turret.GetComponent<TurretComponent>();
        toy_boat.TurretSlots[toy_boat.TurretSlots.Length-1] = new_slot;
    }
    
    public void Editor_RemoveTurretSlot() {
        ToyBoat toy_boat = target as ToyBoat;

        if (toy_boat.TurretSlotsCount() > 0)
        {
            GameObject last_child = toy_boat.TurretChildenRoot.GetChild(toy_boat.TurretChildenRoot.childCount-1).gameObject;
            Editor_ModifyTurretSlots(-1);
            DestroyImmediate(last_child);
        }
    }

    public void Editor_RemoveAllTurretSlots() {
        ToyBoat toy_boat = target as ToyBoat;

        while (toy_boat.TurretChildenRoot.childCount > 0) {
            DestroyImmediate(toy_boat.TurretChildenRoot.GetChild(0).gameObject);
        }
        Editor_ModifyTurretSlots(-toy_boat.TurretSlots.Length);
    }


    public void Editor_InstallTurret(int index, GameObject turretVariant) {
        ToyBoat toy_boat = target as ToyBoat;

        DestroyImmediate(toy_boat.TurretChildenRoot.GetChild(index).gameObject, true);
        GameObject new_turret = Instantiate(turretVariant);
        new_turret.transform.SetParent(toy_boat.TurretChildenRoot);
        new_turret.transform.SetSiblingIndex(index);
        TurretSlot slot = toy_boat.TurretSlots[index];
        slot.Turret = new_turret.GetComponent<TurretComponent>();
        new_turret.transform.localPosition = slot.LocalPosition;
        Vector3 turret_forward = new_turret.transform.localEulerAngles;
        turret_forward.z = slot.LocalEurlerZ;
        new_turret.transform.localEulerAngles = turret_forward;

    }

    private void DrawCustomInspector() 
    {
        ToyBoat toy_boat = target as ToyBoat;

        if (GUILayout.Button(new GUIContent("Remove All Turret Slots")))
        {
            Editor_RemoveAllTurretSlots();
        }
        if (GUILayout.Button(new GUIContent("Add Turret Slot")))
        {
            Editor_AddTurretSlot();
        }

        for (int slot = 0; slot < toy_boat.TurretSlotsCount(); ++slot) 
        {
            string variant_name = toy_boat.GetTurretChild(slot).gameObject.name.Split('(')[0];
            int start_idx = TurretVariants.FindIndex(v => v.name == variant_name);
            int index = EditorGUILayout.Popup("Select Turret Variant", start_idx, TurretVariants.Select(v => v.name).ToArray());
            if (index >= 0 && index < TurretVariants.Count) {
                Editor_InstallTurret(slot, TurretVariants[index]);
            }
        }

        if (GUILayout.Button(new GUIContent("Remove Turret Slot")))
        {
            Editor_RemoveTurretSlot();
        }
    }
    public override void OnInspectorGUI()
    {
        ToyBoat toy_boat = target as ToyBoat;
            
        var prefab_stage = PrefabStageUtility.GetCurrentPrefabStage();
        if (prefab_stage != null) 
        {
            string prefab_name = prefab_stage.prefabContentsRoot.name;
            EditorGUILayout.LabelField($"PrefabStage : {prefab_stage.prefabContentsRoot.name}");
            EditorGUI.BeginChangeCheck();

            if (prefab_name == "ToyBoatPrefabBase")
            {
                EditorGUILayout.ObjectField("Empty Turret Prefab", ToyBoat.EmptyTurretPrefab, typeof(GameObject), false); 
                EditorGUILayout.ObjectField("Turret Children Root", toy_boat.TurretChildenRoot, typeof(GameObject), true);
            }
            else
            {
                Editor_Validate();
                DrawCustomInspector();
            }

            Editor_Validate();

            if (EditorGUI.EndChangeCheck()) 
            {
                PrefabUtility.SaveAsPrefabAsset(prefab_stage.prefabContentsRoot, prefab_stage.assetPath);
                EditorSceneManager.MarkSceneDirty(prefab_stage.scene);
            }
        }
        else 
        {
            Editor_Validate();

            EditorGUILayout.LabelField($"Not In PrefabStage");
            DrawCustomInspector();

            Editor_Validate();
        }
    }   
}
