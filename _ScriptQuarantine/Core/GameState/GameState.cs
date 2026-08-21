using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameState
{
    public List<UnitData> units = new();
    public List<BuildingData> buildings = new();

    public int wood;
    public int stone;
    public int gold;

    public float[] fogPixels;
}
