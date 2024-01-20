using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStraight : MonoBehaviour
{
    public Vector3 Direction = Vector3.left;
    public float MoveSpeed = 2f;

    void FixedUpdate()
    {
        transform.position += Vector3.right * 2f * Time.deltaTime;
        //transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);
    }
}
