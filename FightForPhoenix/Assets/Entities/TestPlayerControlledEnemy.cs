using UnityEngine;
using UnityEngine.Tilemaps;

public class TestPlayerControlledEnemy : MonoBehaviour
{
    public GameObject tilemapGameObject;
    public Rigidbody2D Rigidbody { get; private set; }
    Tilemap tilemap;

    void Start()
    {
        tilemap = tilemapGameObject.GetComponent<Tilemap>();
        Rigidbody = GetComponent<Rigidbody2D>();
        Debug.Assert(tilemap != null, "You forget to add a Tiilemap to the scene");
        Debug.Assert(Rigidbody != null, "You forget to add a rigidbody to this game object");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 hitPosition = Vector3.zero;
        if (tilemap != null && tilemapGameObject == collision.gameObject) {
            foreach (ContactPoint2D hit in collision.contacts) {
                hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
                GameManager.NumberOfHits++;
                Debug.Log($"Got hit!!");
            }
        }
    }
}