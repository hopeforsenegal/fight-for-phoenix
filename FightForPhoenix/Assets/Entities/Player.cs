using UnityEngine;

public class Player : MonoBehaviour
{
    public TrailRenderer TrailRenderer { get; private set; }
    public Transform BulletSpawn;
    public GameObject laser;
    public GameObject shotgun;


    void Awake()
    {
        TrailRenderer = GetComponentInChildren<TrailRenderer>();
        laser.SetActive(false);
        shotgun.SetActive(false);
    }
}