using System;
using UnityEngine;

[Serializable]
public class BuildingData
{
    public string prefabName;

    public float x;
    public float y;
    public float z;

    public int health;

    public Vector3 position;
    public Quaternion rotation;
}
