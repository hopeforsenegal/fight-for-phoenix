using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] Transform world;
    [SerializeField] float speed = 100f;

    // Update is called once per frame
    void FixedUpdate()
    {
        RotateAroundPlanet();
    }

    void RotateAroundPlanet(){
        int direction = 0;
        if(Input.GetKey(KeyCode.A)) direction = 1;
        if(Input.GetKey(KeyCode.D)) direction = -1;
        transform.RotateAround(world.transform.position, 
        Vector3.forward, direction * speed * Time.deltaTime);
    }
}
