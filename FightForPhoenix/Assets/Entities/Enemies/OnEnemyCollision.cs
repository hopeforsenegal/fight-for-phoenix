using UnityEngine;

public class OnEnemyCollision : MonoBehaviour
{
    protected void Awake()
    {
        var c = GetComponent<Collider2D>();
        if (c == null) {
            Debug.LogWarning("Hey! add the appropiate collider!!!!!");
            gameObject.AddComponent<BoxCollider2D>();
        }
    }

    protected void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
        Debug.Log($"enemy named '{name}' blew up!");
    }
}
