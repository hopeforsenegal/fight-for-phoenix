using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnem2 : MonoBehaviour
{
    [SerializeField] Config enemyBase;
    [SerializeField] Transform target;
    Vector3 direction;

    float test = 0f;

    void Start {
        var startingSet = enemyBase.LookAtPlanet(transform, target);
    }

    void Update()
    {
        test += Time.deltaTime;
        transform.position = enemyBase.WaveMove(transform.position, test);
        //sin
        //test += Time.deltaTime;
        //transform.position = new Vector3(Mathf.Sin(test), 0.0f, 0.0f);
        //cos
        //transform.position = new Vector3(Mathf.Cos(test), 0.0f, 0.0f);
    }
}
