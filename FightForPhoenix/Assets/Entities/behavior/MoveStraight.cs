using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStraight : MonoBehaviour
{
    Rigidbody2D _rb;
    public float MoveSpeed = 1000f;

    void Start() {
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        _rb.AddForce(transform.up * MoveSpeed);
    }
}
