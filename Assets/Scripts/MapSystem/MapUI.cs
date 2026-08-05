using UnityEngine;

public class MapUI : MonoBehaviour
{
    public MapManager manager;

    public void LoadMap0() => manager.LoadMap(0);
    public void LoadMap1() => manager.LoadMap(1);
    public void LoadMap2() => manager.LoadMap(2);
}
