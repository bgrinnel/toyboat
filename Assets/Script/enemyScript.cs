using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class enemyScript : MonoBehaviour
{
    [SerializeField] private float enemyHP;
    [SerializeField] private float speed;
    [SerializeField] private float rotSpeed;
    [SerializeField] private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("shipParent");
    }

    // Update is called once per frame
    void Update()
    {
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
    private void OnCollisionEnter(Collision OtherObject){
        Debug.Log("hit" + OtherObject.gameObject.tag);
        if(OtherObject.gameObject.tag == "shell"){
            Destroy(OtherObject.gameObject);
            Destroy(this.gameObject);
        }
        else if(OtherObject.gameObject.tag == "Player"){
           SceneManager.LoadScene("GameOver");
        }
    }
}
