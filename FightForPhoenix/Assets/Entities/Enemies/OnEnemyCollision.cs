using UnityEngine;

public class OnEnemyCollision : MonoBehaviour
{
    public float dropRate;

    private GameManager m_GameManager;

    protected void Awake()
    {
        var c = GetComponent<Collider2D>();
        if (c == null) {
            Debug.LogWarning("Hey! add the appropiate collider!!!!!");
            gameObject.AddComponent<BoxCollider2D>();
        }
        m_GameManager = FindObjectOfType<GameManager>();
    }

    protected void OnCollisionEnter2D(Collision2D other)
    {
        m_GameManager.OnEnemyCollision(this, other);
    }
}
