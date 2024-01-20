using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRotation : MonoBehaviour
{
    public Transform target;
    public float speed = 50f;

    // Update is called once per frame
    void FixedUpdate()
    {
        var direction = 0;
        if (Input.GetKey(KeyCode.A)) direction = 1;
        if (Input.GetKey(KeyCode.D)) direction = -1;
        var angle = direction * speed * Time.fixedDeltaTime;
        transform.RotateAround(target.position, Vector3.forward, angle);
        Debug.Log("rot:" + transform.rotation);
    }
}
