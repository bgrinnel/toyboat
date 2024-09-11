
public struct TCombatContext
{
    public EDamageType DamageType;
    public float DamageBase;

    public TCombatContext(float damageBase)
    {
        DamageType = EDamageType.Normal;
        DamageBase = damageBase;
    }
}