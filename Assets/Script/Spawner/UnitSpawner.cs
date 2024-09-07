using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public bool autoSpawn;
    public float SpawnTime;
    public GameObject SpawnPrefab;

    private bool coolDown = false;
    // Start is called before the first frame update
    void Start()
    {
        GameModeObject.Register(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (autoSpawn)
        {
            AutoSpawn(SpawnPrefab);
        }
    }

    private void AutoSpawn(GameObject prefab)
    {
        if (coolDown) { return; }
        Instantiate(prefab, transform.position, transform.rotation);
        StartCoroutine(CoolDown());
    }

    private IEnumerator CoolDown()
    {
        coolDown = true;
        yield return new WaitForSeconds(SpawnTime);
        coolDown = false ;
    }

    private void SpawnUnit(GameObject prefab)
    {
        Instantiate(prefab, transform.position, transform.rotation);
    }
    void OnDestroyed()
    {
        GameModeObject.Unregister(this);
    }
}
