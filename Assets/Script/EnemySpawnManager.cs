using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawnManager : ManagerObject<EnemySpawnManager>
{
    [SerializeField] private GameObject playerSceneReference;

    [SerializeField] private GameObject[] enemiesToSpawn;
    [SerializeField] private int[] oddsToSpawn;
    [SerializeField] private float secondsBetweenSpawns = 2f;
    private GameObject enemiesRoot;
    private int totalOdds;
    private float elapsedBetweenSpawns;
    private float SpawnRadius;

    void OnEnable()
    {
        if (enemiesToSpawn == null || oddsToSpawn == null) 
        {
            Debug.LogError("enemiesToSpawn and oddsToSpawn Must Be Instantiated");
        }
        if (enemiesToSpawn.Length != oddsToSpawn.Length) Debug.LogError("enemiesToSpawn and oddsToSpawn must have the same number of entries!");
    }

    protected new virtual void Awake()
    {
        base.Awake();
        Debug.Log("EnemySpawneManager Awake()");

        if (playerSceneReference == null) 
        {
            Debug.LogError("BoundlessWorldManager needs `playerSceneReference` to be set.");
            return;
        }
        totalOdds = 0;
        foreach (var odds in oddsToSpawn)
        {
            totalOdds += odds;
        }
        SpawnRadius = 0f;
        enemiesRoot = new GameObject("enemiesRoot");
        enemiesRoot.transform.SetParent(transform);
    }

    // Start is called before the first frame update
    void Start()
    {
        var mode = GameModeObject.Get();
        mode.pauseEvent += OnPauseEvent;
        var extents = BoundlessWorldManager.Get().GetTileExtents();
        SpawnRadius = 9f * Mathf.Max(extents.x, extents.z);
    }

    void Update()
    {
        elapsedBetweenSpawns += Time.deltaTime;
        if (elapsedBetweenSpawns >= secondsBetweenSpawns)
        {
            float winning_odds = UnityEngine.Random.value * totalOdds;
            float curr_odds = 0;
            for (int idx = 0; idx < enemiesToSpawn.Length; ++idx)
            {
                curr_odds += oddsToSpawn[idx];
                if (curr_odds >= winning_odds)
                {
                    Debug.Log("Spawning Enemy");
                    var dir_2d = UnityEngine.Random.insideUnitCircle;
                    var dir = new Vector3(dir_2d.x, 0f, dir_2d.y);
                    var player_pos = playerSceneReference.transform.position;
                    player_pos.y = 0f;
                    var spawn_pos = player_pos + dir * SpawnRadius;
                    var instance = Instantiate(enemiesToSpawn[idx], enemiesRoot.transform);
                    instance.transform.position = spawn_pos;
                    instance.transform.LookAt(player_pos, Vector3.up);
                    break;
                }
            }
            elapsedBetweenSpawns -= secondsBetweenSpawns;
        }
    }   

    void OnPauseEvent(bool bPaused)
    {
        enabled = !bPaused;
    }

    public override void DestroyOnSceneUnload(Scene _old)
    {
        var mode = GameModeObject.Get();
        if (mode != null) mode.pauseEvent -= OnPauseEvent;
        base.DestroyOnSceneUnload(_old);
    }
}
