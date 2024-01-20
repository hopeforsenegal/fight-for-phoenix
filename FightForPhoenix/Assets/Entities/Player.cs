using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    //commented out consistent speed
    [SerializeField] Transform world;
    //[SerializeField] float speed = 100f;
    [SerializeField] float speedIncrease = 20f;
    float _curSpeed = 0;
    int _direction = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        /*int direction = 0;
        if(Input.GetKey(KeyCode.A)) direction = 1;
        if(Input.GetKey(KeyCode.D)) direction = -1; */
        if(Input.GetKey(KeyCode.A)) _direction = 1;
        if(Input.GetKey(KeyCode.D)) _direction = -1;
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) _curSpeed += speedIncrease;
        _curSpeed = Mathf.Clamp(_curSpeed, 0f, 200f);
        transform.RotateAround(world.transform.position, 
        Vector3.forward, _direction * _curSpeed * Time.deltaTime);
        Debug.Log(_curSpeed);
        //transform.RotateAround(world.transform.position, Vector3.forward, direction * speed * Time.deltaTime);
    }
}
