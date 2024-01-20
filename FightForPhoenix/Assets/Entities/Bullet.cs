using UnityEngine;
using UnityEngine.Tilemaps;

public class Bullet : MonoBehaviour
{
    // This will break once there are multple tile maps
    public GameObject tilemapGameObject;
    Tilemap tilemap;

    void Start()
    {
        tilemap = FindObjectOfType<Tilemap>();
        Debug.Assert(tilemap != null, "You forget to add a Tiilemap to the scene");
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A)) {
            Vector3 v = -transform.right * 1;  // -transform.right = left
            GetComponent<Rigidbody2D>().velocity = v;
        }

        if (Input.GetKey(KeyCode.D)) {
            Vector3 v = transform.right * 1;  // -transform.right = left
            GetComponent<Rigidbody2D>().velocity = v;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 hitPosition = Vector3.zero;
        if (tilemap != null && tilemapGameObject == collision.gameObject) {
            foreach (ContactPoint2D hit in collision.contacts) {
                hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
            }
        }
    }
}