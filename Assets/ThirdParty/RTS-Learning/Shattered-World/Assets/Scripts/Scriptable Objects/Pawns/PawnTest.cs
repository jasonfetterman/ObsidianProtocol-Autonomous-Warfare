using System.ComponentModel;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PawnTest", menuName = "Shattered-World/PawnTest", order = 0)]
public class PawnTest : ScriptableObject 
{
    [Header("Pawn's Descriptions")]
    public string pawnName;
    public string pawnDescription;
    public string pawnType;
    [Header("Pawn's Stats")]
    public float pawnBaseHealth;
    public float pawnBaseAttack;
    public float pawnBaseDefense;
    public float pawnBaseSpeed;
    public float pawnBaseResistance;
    public float pawnMovement;
    [Header("Pawn's Checkers")]
    public bool pawnIsDead;
    public bool pawnIsColonist;
    public bool pawnIsEnemy;
    public bool pawnIsNeutral;
    [Header("RandomizerCheckers")]
    public float minRangeNumber;
    public float maxRangeNumber;

    public float pawnCurrentHealth, pawnTotalHealth, pawnCurrentAttack, pawnTotalAttack, pawnCurrentDefense, pawnTotalDefense, pawnCurrentSpeed, pawnTotalSpeed, pawnCurrentResistance, pawnTotalResistance, pawnCurrentMovement, pawnTotalMovement;
    public enum pawnBodyParts
    {
        Head, Torso, LeftForeLimb, RightForeLimb, LeftBackLimb, RightBackLimb
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void CalculationOfPawnStats()
    {
        pawnTotalHealth = pawnBaseHealth + Random.Range(minRangeNumber, maxRangeNumber);
        pawnTotalAttack = pawnBaseAttack + Random.Range(minRangeNumber, maxRangeNumber);
        pawnTotalDefense = pawnBaseDefense + Random.Range(minRangeNumber, maxRangeNumber);
        pawnTotalSpeed = pawnBaseSpeed + Random.Range(minRangeNumber, maxRangeNumber);
        pawnTotalResistance = pawnBaseResistance + Random.Range(minRangeNumber, maxRangeNumber);
        pawnTotalMovement = pawnMovement + Random.Range(minRangeNumber, maxRangeNumber);
    }   
}
