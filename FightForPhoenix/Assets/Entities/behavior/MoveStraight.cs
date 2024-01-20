using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStraight : MonoBehaviour
{
    public Vector3 Direction = Vector3.left;
    public float MoveSpeed = 2f;

    void FixedUpdate()
    {
        transform.position += MoveSpeed * Direction * Time.deltaTime;
    }
}
