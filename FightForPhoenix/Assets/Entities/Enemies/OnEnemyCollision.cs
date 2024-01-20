using UnityEngine;

public class OnEnemyCollision : MonoBehaviour
{
    public float dropRate;

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

        //if (Random.value <= dropRate) {
        { 
            Debug.Log("We should drop a power up");

            var powerup = PowerUpDrop.CreatePowerup("Test");
            powerup.transform.position = other.transform.position;
            //GameObject.cr
        }
    }
}
