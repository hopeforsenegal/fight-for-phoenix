using UnityEngine;

public class OnEnemyCollision : MonoBehaviour
{
    protected void Awake()
    {
        var c = GetComponent<Collider2D>();
        Debug.Assert(c != null, "missing collider");
    }

    protected void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
        Debug.Log($"enemy named '{name}' blew up!");
    }
}
