
using System;
using System.Collections.Generic;

public static class ToyBoat
{
    private static Dictionary<Type, ManagerBehavior> _Managers = new Dictionary<Type, ManagerBehavior>();

    public static void RegisterManager(Type derivedType, ManagerBehavior manager)
    {
        _Managers[derivedType] = manager;
    }

    // Returns the instance of the given manager type, null if none was instantiated
    public static DerivedManager Get<DerivedManager>() where DerivedManager : ManagerBehavior
    {
        return (DerivedManager)_Managers[typeof(DerivedManager)];
    }

    public static ManagerBehavior UnregisterManager(Type derivedType)
    {
        var manager = _Managers[derivedType];
        _Managers.Remove(derivedType);
        return manager;
    }
}