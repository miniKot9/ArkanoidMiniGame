using UnityEngine;

[CreateAssetMenu(fileName = "LevelsSettings", menuName = "Scriptable Objects/LevelsSettings")]
public class LevelsSettings : ScriptableObject
{
    public int MaxHealthBricks = 1;
    public int MaxBricksUnbreakeble = 0;
    public float RewardMod = 1;
}
