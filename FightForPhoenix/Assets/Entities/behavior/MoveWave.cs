using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWave : MonoBehaviour
{
    [SerializeField] float waveSize = 2f;
    [SerializeField] bool useSine = true;
    GameManager _gm;
    float counterMax;
    float counter = 0f;

    void Start() {
        _gm = FindObjectOfType<GameManager>();
        counterMax = Mathf.PI * 2; //ensure complete revolution up and down
        //GetComponent<Rigidbody2D>().useFullKinematicContacts = true;
    }

    void FixedUpdate() {
        if(counter > counterMax) counter = 0f;
        counter += Time.deltaTime;
        
        transform.localPosition += new Vector3(1f,0f,0f) * (_gm.config.WaveMove(counter) * waveSize);
    }
}
