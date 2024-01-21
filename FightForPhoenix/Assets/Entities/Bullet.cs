using UnityEngine;

public class Bullet : MonoBehaviour
{
    GameManager m_GameManager;

    void Awake()
    {
        m_GameManager = FindObjectOfType<GameManager>();
        if (gameObject.GetComponent<MoveStraight>() == null) {
            gameObject.AddComponent<MoveStraight>();
        }
    }
    void Start()
    {
        Destroy(gameObject, m_GameManager.config.BulletLifetime);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        m_GameManager.OnBulletCollision(this, other);
    }
}