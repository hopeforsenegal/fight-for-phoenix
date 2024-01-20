using UnityEngine;
using UnityEngine.Tilemaps;

public class Bullet : MonoBehaviour
{
    // This will break once there are multple tile maps
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
        if (tilemap.gameObject == collision.gameObject) {
            Debug.Log("Hit tile");
            foreach (var hit in collision.contacts) {
                Vector3 hitWorldPosition = Vector3.zero;
                hitWorldPosition.x = hit.point.x + 0.01f * hit.normal.x;
                hitWorldPosition.y = hit.point.y + 0.01f * hit.normal.y;
                Debug.Log($"hit {hitWorldPosition}");

                Vector3Int cellPosition = tilemap.WorldToCell(hitWorldPosition - new Vector3Int(0, 1, 0));
                Debug.Log($"cell {cellPosition}");

                BoundsInt bounds = tilemap.cellBounds;
                TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

                Debug.Log($"bounds {bounds}");

                for (int x = 0; x < bounds.size.x; x++) {
                    for (int y = 0; y < bounds.size.y; y++) {
                        TileBase tile = allTiles[x + y * bounds.size.x];
                        if (tile != null) {
                            Debug.Log($"x:{x} y:{y} tile:{tile.name} ");
                        } else {
                            Debug.Log($"x:{x} y:{y} tile: (null)");
                        }
                    }
                }

                if (tilemap.HasTile(cellPosition)) {
                    Debug.Log($"Had tile at position {cellPosition}");
                }
                tilemap.SetTile(cellPosition, null);
            }
        }
    }
}