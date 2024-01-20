using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "ScriptableObjects/Config", order = 1)]
public class Config : ScriptableObject
{
    // Planet
    public int MaxNumberOfPlanetHealth = 10;

    // Player
    public int Speed = 100;
}
