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

    void OnTriggerEnter2D(Collider2D other)
    {
        // Convert the hit position to a cell position
        Vector3 hitPosition3 = other.transform.position;
        Vector3Int cellPosition3 = tilemap.WorldToCell(hitPosition3);

        // Get the tile at the cell position
        TileBase tile = tilemap.GetTile(cellPosition3);
        if (tile != null) {
            Debug.Log("Hit tile: " + tile.name);
        }

        if (tilemap.gameObject == other.gameObject) {
            Debug.Log("Hit tile");

            Vector3 hitPosition = other.transform.position;
            Vector3Int cellPosition2 = tilemap.WorldToCell(hitPosition);
            Debug.Log($"cell2 {cellPosition2}");

            //foreach (var hit in other.contacts) {
            { 
                Vector3 hitWorldPosition = Vector3.zero;
               // hitWorldPosition.x = hit.point.x + 0.01f * hit.normal.x;
               // hitWorldPosition.y = hit.point.y + 0.01f * hit.normal.y;
                Debug.Log($"hit {hitWorldPosition}");

                Vector3Int cellPosition = tilemap.WorldToCell(hitWorldPosition - new Vector3Int(0, 1, 0));
                Debug.Log($"cell {cellPosition}");

                BoundsInt bounds = tilemap.cellBounds;
                TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

                Debug.Log($"bounds {bounds}");


                for (int x = tilemap.cellBounds.xMin; x < tilemap.cellBounds.xMax; x++) {
                    for (int y = tilemap.cellBounds.yMin; y < tilemap.cellBounds.yMax; y++) {
                        Vector3Int localPlace = new Vector3Int(x, y, (int)tilemap.transform.position.z);
                        //Vector3 place = tileMap.CellToWorld(localPlace);
                        //TileBase tile = allTiles[x + y * bounds.size.x];
                        Debug.Log($"localPlace {localPlace}");
                        if (cellPosition == localPlace) {
                            Debug.Log("Hey");
                        }
                        //if (tilemap.HasTile(localPlace))
                        //    if (tile != null) {
                        //    Debug.Log($"x:{x} y:{y} tile:{tile.name} ");
                        //} else {
                        //    Debug.Log($"x:{x} y:{y} tile: (null)");
                        //}
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