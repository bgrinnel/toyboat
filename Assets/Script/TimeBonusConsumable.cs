using Unity;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TimeBonusComsumable : ConsumeableBehaviour
{
    public SurvivalModeManager.TimeEvent onPickup;
    [SerializeField] private float timeBonus = 5f;

    protected override void Start()
    {
        base.Start();
        var mode = GameModeObject.Get() as SurvivalModeManager;
        if (mode)
        {
            onPickup += mode.OnTimeEvent;
        }
        rootObj = transform.parent.gameObject;
    }

    protected override void OnPickupEffect()
    {
        Debug.LogWarning("I'm being picked up");
        onPickup?.Invoke(timeBonus);
    }

}