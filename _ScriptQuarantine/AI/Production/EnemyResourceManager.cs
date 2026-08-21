using UnityEngine;

public class EnemyResourceManager : MonoBehaviour
{
    [Header("Enemy Production Resources")]
    [SerializeField] private int primaryResource = 500;
    [SerializeField] private int secondaryResource = 250;
    [SerializeField] private int tertiaryResource = 100;

    public int PrimaryResource => primaryResource;
    public int SecondaryResource => secondaryResource;
    public int TertiaryResource => tertiaryResource;

    public bool Spend(int primaryCost, int secondaryCost, int tertiaryCost)
    {
        if (primaryResource < primaryCost)
            return false;

        if (secondaryResource < secondaryCost)
            return false;

        if (tertiaryResource < tertiaryCost)
            return false;

        primaryResource -= primaryCost;
        secondaryResource -= secondaryCost;
        tertiaryResource -= tertiaryCost;

        return true;
    }

    public void Add(int primaryAmount, int secondaryAmount, int tertiaryAmount)
    {
        primaryResource += primaryAmount;
        secondaryResource += secondaryAmount;
        tertiaryResource += tertiaryAmount;
    }

    public void SetResources(int primaryAmount, int secondaryAmount, int tertiaryAmount)
    {
        primaryResource = Mathf.Max(0, primaryAmount);
        secondaryResource = Mathf.Max(0, secondaryAmount);
        tertiaryResource = Mathf.Max(0, tertiaryAmount);
    }
}