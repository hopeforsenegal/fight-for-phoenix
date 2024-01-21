using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnem2 : MonoBehaviour
{
    [SerializeField] Config enemyBase;
    [SerializeField] Transform target;
    Vector3 direction;

    float test = 0f;
    Vector3 velocity;

    void Start() {
        transform.rotation = enemyBase.LookAtTarget(transform, target);
        direction = enemyBase.GetDirectionVector3(transform, target);
    }

    void FixedUpdate()
    {
        test += Time.deltaTime;
        //velocity = new Vector3(enemyBase.Speed * direction.x * Time.deltaTime, 
            //enemyBase.WaveMove(direction, test).y, 0f);
            
            /*
        transform.position += 
            new Vector3(enemyBase.Speed * direction.x * Time.deltaTime,transform.position.y,0f);
        transform.position = 
            new Vector3(transform.position.x,enemyBase.WaveMove(direction, test, false).y,0f);
            */

        //velocity = transform.position;
        //transform.position = enemyBase.WaveMove(direction, test);
        //velocity += enemyBase.Speed * direction * Time.deltaTime;
        //transform.position += velocity;
        //transform.position = enemyBase.WaveMove(transform.position, test);
        //sin
        //test += Time.deltaTime;
        //transform.position = new Vector3(Mathf.Sin(test), 0.0f, 0.0f);
        //cos
        //transform.position = new Vector3(Mathf.Cos(test), 0.0f, 0.0f);
    }
}
