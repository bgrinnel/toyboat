
using System;
using UnityEngine;

public class CombatManager : ManagerObject<CombatManager>
{
    // the most bare bones of damage events currently
    public static void DamageEvent(ICombatEntity damager, ICombatEntity damagee, TCombatContext context)
    {
        damagee.Health -= context.DamageBase;
        bool damagee_defeated = damagee.Health < 0;

        string name1 = damager is MonoBehaviour d1 ? d1.name : "NOT_GAMEOBJ";
        string name2 = damager is MonoBehaviour d2 ? d2.name : "NOT_GAMEOBJ";
        string health_action = context.DamageBase < 0 ? "dealt damage to" : "healed";
        var abs_health = Mathf.Abs(context.DamageBase);
        Console.WriteLine($"{name1} {health_action} {name2} for {abs_health :0.00} health");

        if (damagee_defeated)
            damagee.OnDefeated(damager, context);
        if (damager is IFragileEntity)
            damager.OnDefeated(damager, context);
    }
}