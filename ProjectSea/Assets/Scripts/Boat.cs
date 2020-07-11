using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour {

    public Rigidbody RB;
    public float speed; 

    private void Start() {
        
    }

    private void FixedUpdate() {
        //transform.Translate(Vector3.forward + Vector3.right * 0.1f);
        RB.AddForce(transform.forward * Time.deltaTime * speed);
    }
}

