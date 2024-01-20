using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Transform world;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.A)){
            transform.RotateAround(world.transform.position, Vector3.forward, 20 * Time.deltaTime);
        }
    }
}
