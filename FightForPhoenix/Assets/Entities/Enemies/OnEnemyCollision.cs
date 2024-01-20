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
        Destroy(gameObject);
        Debug.Log($"enemy named '{name}' blew up!");

        if (Random.value <= dropRate) {
            m_GameManager.DropPowerup(other.transform.position);
        }
    }
}
