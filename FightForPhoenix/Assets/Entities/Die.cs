using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
    void OnCollisionEnter2D(Collider2D other) {
        Destroy(gameObject)
    }
}
