using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class enemyScript : MonoBehaviour, ICombatEntity
{
    public SurvivalModeManager.ScoreEvent shipDefeated;
    [SerializeField] private EnemyType _base;
    public float Health {get {return _base.initialHealth; } set { _base.initialHealth = value; }}
    private GameObject player;
    private bool sinking;

    // Start is called before the first frame update
    void Start()
    {
        GameModeObject.Register(this);
        var mode = (SurvivalModeManager)SurvivalModeManager.Get();
        shipDefeated += mode.OnScoreEvent;

        player = GameObject.Find("shipParent");
    }

    // Update is called once per frame
    void Update()
    {
        if(!sinking){
            float speedTick = Time.deltaTime * _base.moveSpeed;
            Vector3 targetDir = new Vector3(player.transform.position.x - transform.position.x,transform.position.y, player.transform.position.z - transform.position.z);

            if(Quaternion.LookRotation(targetDir) != Quaternion.identity ){
                Quaternion lookDir = Quaternion.LookRotation(targetDir,Vector3.up);
                transform.rotation =  Quaternion.RotateTowards(transform.rotation, lookDir, (_base.rotSpeed * Time.deltaTime));
                transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.localEulerAngles.z);
                transform.position += transform.forward* speedTick;
            }
            else{
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speedTick);
            }
        }
        else{
            transform.Translate(Vector3.down * Time.deltaTime*10f);
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * _base.moveSpeed);
        }
        
    }
    private void OnCollisionEnter(Collision other){
        var game_obj = other.gameObject;
        if(game_obj.TryGetComponent(out Shell shell)){
            CombatManager.DamageEvent(shell, this, new(shell.GetShellDamage()));
            if(Health > 0)
                Instantiate(_base.hitEffect, transform.position, Quaternion.identity, transform);
        }
        else if(game_obj.TryGetComponent(out Ship_Follow_Script player)){
            CombatManager.DamageEvent(this, player, new(_base.collisionDamage));
            sinking = true;
        }
    }

    public void OnDefeated(ICombatEntity defeater, TCombatContext context)
    {
        sinking = true;
        Destroy(GetComponent<Rigidbody>());
        shipDefeated?.Invoke(_base.defeatedScore, _base.comboScoreMod);
        GameModeObject.Unregister(this);
        var mode = GameModeObject.Get() as SurvivalModeManager;
        if (mode)
        {
            shipDefeated += mode.OnScoreEvent;
        }
        Destroy(gameObject, 1.5f);
    }

}
