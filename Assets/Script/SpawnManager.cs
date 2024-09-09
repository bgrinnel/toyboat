using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : ManagerObject<SpawnManager>
{
    [SerializeField] private GameObject player;

    [SerializeField] private GameObject[] enemiesToSpawn;

    [Tooltip("the odds vs the sum of the odds in this list that the coorisponding enemy index will spawn.")]
    [SerializeField] private int[] enemyOdds;

    [SerializeField] private GameObject[] consumablesToSpawn;

    [Tooltip("the decimal odds that the coorisponding consumable index will spawn every {secondsBetweenSpawns} seconds")]
    [SerializeField] private float[] consumableOdds;
    [SerializeField] private float secondsBetweenSpawns = 2f;
    private GameObject enemiesRoot;
    private GameObject consumablesRoot;
    private int totalEnemyOdds;
    private float elapsedBetweenSpawns;
    private float spawnRadius;

    void OnEnable()
    {
        if (enemiesToSpawn == null || enemyOdds == null) 
        {
            Debug.LogError("enemiesToSpawn and oddsToSpawn Must Be Instantiated");
        }
        if (enemiesToSpawn.Length != enemyOdds.Length) Debug.LogError("enemiesToSpawn and enemyOdds must have the same number of entries!");
        if (consumablesToSpawn == null || consumableOdds == null) 
        {
            Debug.LogError("consumablesToSpawn and consumableOdds Must Be Instantiated");
        }
        if (consumablesToSpawn.Length != consumableOdds.Length) Debug.LogError("consumablesToSpawn and consumableOdds must have the same number of entries!");
    }

    protected override void Awake()
    {
        base.Awake();
        Debug.Log("EnemySpawneManager Awake()");

        if (player == null) 
        {
            Debug.LogError("BoundlessWorldManager needs `playerSceneReference` to be set.");
            return;
        }

        totalEnemyOdds = 0;
        foreach (var odds in enemyOdds)
        {
            totalEnemyOdds += odds;
        }
        spawnRadius = 0f;
        enemiesRoot = new GameObject("enemiesRoot");

        enemiesRoot.transform.SetParent(transform);
        consumablesRoot = new GameObject("consumablesRoot");
        consumablesRoot.transform.SetParent(transform);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        var mode = GameModeObject.Get();
        mode.pauseEvent += OnPauseEvent;
        var extents = BoundlessWorldManager.Get().GetTileExtents();
        spawnRadius = 9f * Mathf.Max(extents.x, extents.z);
        Debug.LogWarning($"SpawnRadius: {spawnRadius}");
    }

    void Update()
    {
        elapsedBetweenSpawns += Time.deltaTime;
        if (elapsedBetweenSpawns >= secondsBetweenSpawns)
        {
            float winning_odds = UnityEngine.Random.value * totalEnemyOdds;
            float curr_odds = 0;
            for (int idx = 0; idx < enemiesToSpawn.Length; ++idx)
            {
                curr_odds += enemyOdds[idx];
                if (curr_odds >= winning_odds)
                {
                    Debug.Log($"Spawning Enemy: {enemiesToSpawn[idx].name}");
                    var dir_2d = UnityEngine.Random.insideUnitCircle.normalized;
                    if (dir_2d == Vector2.zero) dir_2d = player.transform.forward;
                    var dir = new Vector3(dir_2d.x, 0f, dir_2d.y);
                    var player_pos = player.transform.position;
                    player_pos.y = 0f;
                    var spawn_pos = player_pos + dir * spawnRadius;
                    var instance = Instantiate(enemiesToSpawn[idx], spawn_pos, Quaternion.identity, enemiesRoot.transform);
                    // instance.transform.position = spawn_pos;
                    // print($"SpawnDir={dir} - distFromPlayer={(spawn_pos-player_pos).magnitude}");
                    instance.transform.LookAt(player_pos, Vector3.up);
                    break;
                }
            }
            
            for (int idx = 0; idx < consumablesToSpawn.Length; ++idx)
            {
                float odds = consumableOdds[idx];
                if (odds >= UnityEngine.Random.value)
                {
                    Debug.Log($"Spawning Consumable: {consumablesToSpawn[idx].name}");
                    var dir_2d = UnityEngine.Random.insideUnitCircle.normalized;
                    if (dir_2d == Vector2.zero) dir_2d = player.transform.forward;
                    var dir = new Vector3(dir_2d.x, 0f, dir_2d.y);
                    var player_pos = player.transform.position;
                    player_pos.y = 1.15f;
                    var spawn_pos = player_pos + dir * Mathf.Lerp(spawnRadius / 10f, spawnRadius, UnityEngine.Random.value);
                    // var spawn_pos = player_pos + dir * 10f;
                    var instance = Instantiate(consumablesToSpawn[idx], consumablesRoot.transform);
                    instance.transform.position = spawn_pos;
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
