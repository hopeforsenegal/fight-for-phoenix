using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    GameManager _gm;

    void Start() {
        GameManager gm = FindObjectOfType<GameManager>();
        transform.rotation = gm.config.LookAtTarget(gm.phoenix.transform, gameObject.transform);
    }
}
