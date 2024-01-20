using UnityEngine;

public class Player : MonoBehaviour
{
    public TrailRenderer TrailRenderer { get; private set; }
    public Transform BulletSpawn;

    void Awake()
    {
        TrailRenderer = GetComponentInChildren<TrailRenderer>();
    }
}