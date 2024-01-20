using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStraight : MonoBehaviour
{
    Rigidbody2D _rb;
    public float MoveSpeed = 2f;

    void Start() {
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        _rb.velocity = transform.up * MoveSpeed;
    }
}
