using UnityEngine;

public class ConsumeableBehaviour : MonoBehaviour
{
    [SerializeField] private float timeTillDespawn;
    private float timeSinceSpawn;
    // defines the root of the object, allows this do be a decendant and still keen up the parent
    protected GameObject rootObj;
    protected virtual void Start()
    {
        GameModeObject.Register(this);
        timeSinceSpawn = 0f;
        rootObj = transform.gameObject;
    }

    protected virtual void Update()
    {
        timeSinceSpawn += Time.deltaTime;
        if (timeSinceSpawn >= timeTillDespawn)
        {
            Destroy(this, 1f);
        }
    }

    protected virtual void OnPickupEffect()
    {   
    }

    protected void OnCollisionEnter(Collision other)
    {
        Debug.LogWarning($"\"{gameObject.tag}\" hit \"{other.gameObject.tag}\"");
        if(other.gameObject.tag == "Player"){
            OnPickupEffect();
            Destroy(rootObj, 1f);
        }
    }

    protected virtual void OnDestroyed()
    {
        GameModeObject.Unregister(this); 
    }
}