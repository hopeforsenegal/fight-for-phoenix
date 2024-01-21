using UnityEngine;

public class PowerUpDrop : MonoBehaviour
{
    GameManager m_GameManager;

    void Awake()
    {
        m_GameManager = FindObjectOfType<GameManager>();
        transform.rotation = m_GameManager.config.LookAtTarget(m_GameManager.phoenix.transform, gameObject.transform);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        m_GameManager.OnPowerupCollision(this, other);
    }
}