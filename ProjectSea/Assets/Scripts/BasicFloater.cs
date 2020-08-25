using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicFloater : MonoBehaviour {

    protected Waves Waves;

    private void Awake() {
        Waves = FindObjectOfType<Waves>();
    }

    private void Update() {
        transform.position = new Vector3(transform.position.x, Waves.GetHeight(transform.position), transform.position.z);
    }
}
