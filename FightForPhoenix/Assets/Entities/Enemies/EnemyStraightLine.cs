using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStraightLine : MonoBehaviour
{
    public Transform Target{ set; get;}
    public float MoveSpeed{ set; get;}
    Collider2D _col;
    Vector3 moveDirection;

    void Start()
    {
        _col = GetComponent<Collider2D>();
        LookAtPlanet();
    }

    void FixedUpdate()
    {
        transform.position += MoveSpeed * moveDirection * Time.deltaTime;
    }

    void LookAtPlanet() {
        var offset = 90f;
        Vector2 facingDirection = Target.position - transform.position;
        facingDirection.Normalize();
        moveDirection = new Vector3(facingDirection.x, facingDirection.y, 0f);
        float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;       
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
    }

    void OnCollisionEnter2D(Collision2D other) {
        Destroy(gameObject);    
    }
}
