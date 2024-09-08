using System;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoundlessWorldManager : ManagerObject<BoundlessWorldManager>
{
    [SerializeField] private GameObject WaterTileReference;
    [SerializeField] private GameObject playerSceneReference;
    private const int TILEPOOL_LEN = 25;
    private const int TILEPOOL_SQRT = 5;
    private const float TILEPOOL_OFFSET = 2;
    private GameObject[] tilePool;
    private Vector3 tileExtents;
    private bool bPlayerNearEdge;
    private Vector3Int nearestTileCenter;
    protected new virtual void Awake()
    {
        base.Awake();
        Debug.Log("BoundlessWorldManager Awake()");

        if (playerSceneReference == null) 
        {
            Debug.LogError("BoundlessWorldManager needs `playerSceneReference` to be set.");
            return;
        }
        
        if (WaterTileReference == null) Debug.LogError("BoundlessWorldManager needs `WaterTile` to be set.");
        var meshfilter_comp = WaterTileReference.GetComponent<MeshFilter>();
        if (meshfilter_comp == null) Debug.LogError("`WaterTile` attribute of `BoundlessWorldManager` needs to have a `MeshFilter` component.");
        else if (meshfilter_comp.sharedMesh == null) Debug.LogError("`WaterTile` attribute of `BoundlessWorldManager` needs to have a valid `sharedMesh` reference in its `MeshFilter` component.");
        else
        {
            tileExtents = meshfilter_comp.sharedMesh.bounds.extents;
            var scale = WaterTileReference.transform.localScale;
            tileExtents.x *= scale.x;
            tileExtents.y *= scale.y;
            tileExtents.z *= scale.z;
            Debug.Log($"tileExtents = {tileExtents}");
        }
        // Debug, let's see if this works
        tilePool = new GameObject[TILEPOOL_LEN];
        for (int idx = 0; idx < TILEPOOL_LEN; ++idx)
        {
            var tile = Instantiate(WaterTileReference, transform);
            tilePool[idx] = tile;
        }
        nearestTileCenter = GetNearestExtents(playerSceneReference);
        CenterTiles();
    }

    // Start is called before the first frame update
    void Start()
    {
        var mode = GameModeObject.Get();
        mode.pauseEvent += OnPauseEvent;
    }

    private Vector3Int GetNearestExtents(GameObject obj)
    {
        var pos = obj.transform.position;
        int x_extents = Mathf.RoundToInt(pos.x / (tileExtents.x / 3f));
        int z_extents = Mathf.RoundToInt(pos.z / (tileExtents.z / 3f));
        bool x_edge = ((x_extents-3) % 6) == 0;
        bool z_edge = ((z_extents-3) % 6) == 0;
        bPlayerNearEdge = x_edge || z_edge;
        x_extents /= 3;
        z_extents /= 3;
        if (x_edge) x_extents += x_extents < 0 ? -1 : 1;
        if (z_edge) z_extents += z_extents < 0 ? -1 : 1;
        return new(x_extents, -1, z_extents);
    }
    void Update()
    {
        var extents = GetNearestExtents(playerSceneReference);
        if (bPlayerNearEdge && extents != nearestTileCenter)
        {
            nearestTileCenter = extents;
            CenterTiles();
        }
    }   

    private void CenterTiles()
    {
        Debug.Log("Tiles are being Centered");
        var new_center = new Vector3(nearestTileCenter.x * tileExtents.x, -tileExtents.y, nearestTileCenter.z * tileExtents.z);
        for (int idx = 0; idx < TILEPOOL_LEN; ++idx)
        {
            Vector3 pos = new_center;
            pos.x += (idx % TILEPOOL_SQRT - TILEPOOL_OFFSET) * tileExtents.x * 2f;
            pos.z += (idx / TILEPOOL_SQRT - TILEPOOL_OFFSET) * tileExtents.z * 2f;
            tilePool[idx].transform.position = pos;
        }
    }
    void OnPauseEvent(bool bPaused)
    {
        enabled = !bPaused;
    }
}