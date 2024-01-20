using UnityEngine;

public class Player : MonoBehaviour
{
    public TrailRenderer TrailRenderer { get; private set; }

    void Awake()
    {
        TrailRenderer = GetComponentInChildren<TrailRenderer>();
    }
}