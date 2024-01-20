using UnityEngine;

public class PowerUpDrop : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer { get; private set; }

    void Awake()
    {
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public static PowerUpDrop CreatePowerup(string name)
    {
        var drop = new GameObject(name, typeof(PowerUpDrop));
        return drop.GetComponent<PowerUpDrop>();
    }
}