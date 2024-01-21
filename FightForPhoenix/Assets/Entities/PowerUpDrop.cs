using UnityEngine;

public class PowerUpDrop : MonoBehaviour
{
    GameManager m_GameManager;

    void Awake()
    {
        m_GameManager = FindObjectOfType<GameManager>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        m_GameManager.OnPowerupCollision(this, other);
    }
}