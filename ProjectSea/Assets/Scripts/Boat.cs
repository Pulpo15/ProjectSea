using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour {
    public Rigidbody RB;

    private void Start() {
        
    }

    private void FixedUpdate() {
        RB.AddForce(new Vector3(0.5f, 0, 0));
    }
}

