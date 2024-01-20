using UnityEngine;

public class Player : MonoBehaviour
{
    public TrailRenderer TrailRenderer { get; private set; }
    public Transform BulletSpawn;
    public GameObject laser;

    void Awake()
    {
        TrailRenderer = GetComponentInChildren<TrailRenderer>();
    }
}