using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class enemyScript : MonoBehaviour
{
    public SurvivalModeManager.ScoreEvent shipDefeated;

    [SerializeField] private float enemyHP;
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private float rotSpeed;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject HitEffect;

    [SerializeField] private float DefeatedScore = 100f;
    [SerializeField] private float DefeatedScoreMod = 0.1f;
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
            float speedTick = Time.deltaTime * speed;
            Vector3 targetDir = new Vector3(player.transform.position.x - transform.position.x,transform.position.y, player.transform.position.z - transform.position.z);

            if(Quaternion.LookRotation(targetDir) != Quaternion.identity ){
                //Debug.Log(targetDir);
                Quaternion lookDir = Quaternion.LookRotation(targetDir,Vector3.up);
                transform.rotation =  Quaternion.RotateTowards(transform.rotation, lookDir, (rotSpeed * Time.deltaTime));
                transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.localEulerAngles.z);
                transform.position += transform.forward* speedTick;
            }
            else{
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speedTick);
            }
        }
        else{
            transform.Translate(Vector3.down * Time.deltaTime*10f);
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * speed);
        }
        
    }
    private void OnCollisionEnter(Collision OtherObject){
        Debug.Log("hit" + OtherObject.gameObject.tag);
        if(OtherObject.gameObject.tag == "shell"){
            
            Shell shellScript = OtherObject.gameObject.GetComponent<Shell>();
            enemyHP -=shellScript.GetShellDamage();
            if(enemyHP <= 0){
                sinking = true;
                Destroy(this.GetComponent<Rigidbody>());
                GameObject hitEff = Instantiate(HitEffect, this.transform.position,Quaternion.identity);
                Debug.Log(this.transform.rotation + " " + hitEff.transform.rotation);
                shipDefeated?.Invoke(DefeatedScore, DefeatedScoreMod);
                Destroy(this.gameObject, 1.5f);
            }
            else{
                GameObject hitEff = Instantiate(HitEffect, this.transform.position,Quaternion.identity);
            }
            Destroy(OtherObject.gameObject);
        }
        else if(OtherObject.gameObject.tag == "Player"){
            Ship_Follow_Script playerScript = OtherObject.gameObject.GetComponent<Ship_Follow_Script>();
            playerScript.DamagePlayer(damage);
            sinking = true;
            Destroy(this.gameObject,1.5f);
            
        }
    }

    void OnDestroyed()
    {
        GameModeObject.Unregister(this);
        var mode = GameModeObject.Get() as SurvivalModeManager;
        if (mode)
        {
            shipDefeated += mode.OnScoreEvent;
        }
    }

}
