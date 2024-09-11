using UnityEngine;

public interface ICombatEntity
{
    public float Health {get;set;}
    public void OnDefeated(ICombatEntity defeater, TCombatContext context);
}