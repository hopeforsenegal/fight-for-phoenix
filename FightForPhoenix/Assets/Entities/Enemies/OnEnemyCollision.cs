using UnityEngine;

public class OnEnemyCollision : MonoBehaviour
{
    private GameManager m_GameManager;

    protected void Awake()
    {
        m_GameManager = FindObjectOfType<GameManager>();
    }
    protected void OnCollisionEnter2D(Collision2D other)
    {
        m_GameManager.OnEnemyCollision(this, other);
    }
}
