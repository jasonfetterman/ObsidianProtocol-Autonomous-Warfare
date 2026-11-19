using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public sealed class SCN22UnitInspectionHUDController : MonoBehaviour
{
    public Text unitName, serviceId, unitType, status, missions, victories;
    public Text losses, damage, repairs, liveHealth, liveArmor, liveEnergy;

    private float nextRefresh;

    private void Update()
    {
        if (Time.unscaledTime < nextRefresh) return;
        nextRefresh = Time.unscaledTime + 0.5f;
        Refresh();
    }

    private static Type FindType(string name)
    {
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            Type type = assembly.GetType(name);
            if (type != null) return type;
        }
        return null;
    }

    private static object Read(object source, string member)
    {
        if (source == null) return null;
        Type type = source.GetType();
        PropertyInfo property = type.GetProperty(member, BindingFlags.Public | BindingFlags.Instance);
        if (property != null) return property.GetValue(source);
        FieldInfo field = type.GetField(member, BindingFlags.Public | BindingFlags.Instance);
        return field != null ? field.GetValue(source) : null;
    }

    private static string Value(object source, string member)
    {
        object value = Read(source, member);
        return value == null || string.IsNullOrWhiteSpace(value.ToString())
            ? "UNAVAILABLE" : value.ToString();
    }

    private void Set(Text target, string value)
    {
        if (target != null) target.text = value;
    }

    private void Refresh()
    {
        Type contextType = FindType("ObsidianProtocol.Game.Deployment.BattlefieldDeploymentContext");
        object context = contextType == null ? null :
            Read(contextType, "Instance");

        string id = Value(context, "SelectedServiceRecordId");

        if (id == "UNAVAILABLE" || string.IsNullOrWhiteSpace(id))
        {
            Set(unitName, "NO UNIT SELECTED");
            Set(serviceId, "SERVICE RECORD: UNAVAILABLE");
            Set(unitType, "UNIT TYPE: UNAVAILABLE");
            Set(status, "STATUS: UNAVAILABLE");
            Set(missions, "UNAVAILABLE");
            Set(victories, "UNAVAILABLE");
            Set(losses, "UNAVAILABLE");
            Set(damage, "UNAVAILABLE");
            Set(repairs, "UNAVAILABLE");
            Set(liveHealth, "UNAVAILABLE");
            Set(liveArmor, "UNAVAILABLE");
            Set(liveEnergy, "UNAVAILABLE");
            return;
        }

        Type runtimeType = FindType("ObsidianProtocol.Game.PersistentMilitary.PersistentMilitaryRuntime");
        MethodInfo getUnit = runtimeType == null ? null :
            runtimeType.GetMethod("GetUnit", BindingFlags.Public | BindingFlags.Static);

        object record = getUnit == null ? null : getUnit.Invoke(null, new object[] { id });

        Set(serviceId, "SERVICE RECORD  /  " + id);
        Set(unitName, Value(record, "UnitId"));
        Set(unitType, "UNIT TYPE  /  " + Value(record, "UnitType"));
        Set(status, "SERVICE STATUS  /  " + Value(record, "Status"));
        Set(missions, Value(record, "Missions"));
        Set(victories, Value(record, "Victories"));
        Set(losses, Value(record, "Losses"));
        Set(damage, Value(record, "Damage"));
        Set(repairs, Value(record, "Repairs"));

        // No verified live health, armor, or energy component is attached to deployed units.
        Set(liveHealth, "UNAVAILABLE");
        Set(liveArmor, "UNAVAILABLE");
        Set(liveEnergy, "UNAVAILABLE");
    }
}
