using UnityEngine;

public class EnemyResourceManager : MonoBehaviour
{
    public int wood = 0;
    public int stone = 0;
    public int gold = 0;

    public int woodIncome = 3;
    public int stoneIncome = 2;
    public int goldIncome = 1;

    public void GatherTick()
    {
        wood += woodIncome;
        stone += stoneIncome;
        gold += goldIncome;
    }

    public bool Spend(int w, int s, int g)
    {
        if (wood < w || stone < s || gold < g)
            return false;

        wood -= w;
        stone -= s;
        gold -= g;
        return true;
    }
}
