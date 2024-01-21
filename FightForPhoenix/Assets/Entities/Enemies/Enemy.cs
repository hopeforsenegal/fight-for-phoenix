using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy : MonoBehaviour
{
    public GameObject tilemapGameObject;
    Tilemap tilemap;
    GameManager _gm;

    void Start() {
        _gm = FindObjectOfType<GameManager>();
        transform.rotation = _gm.config.LookAtTarget(_gm.phoenix.transform, gameObject.transform);
        tilemap = FindObjectOfType<Tilemap>();
        tilemapGameObject = tilemap.gameObject;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 hitPosition = Vector3.zero;
        if (tilemap != null && tilemapGameObject == collision.gameObject) {
            foreach (ContactPoint2D hit in collision.contacts) {
                hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                _gm.OnPlanetCollision(tilemap, tilemap.WorldToCell(hitPosition));
            }
        }
    }
}
