using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicFloater : MonoBehaviour {

    public float AirDrag;
    public float WaterDrag;
    public float AngularDrag;
    public Rigidbody RB;
    public Transform[] FloatPoints;

    protected Waves Waves;

    private void Awake() {
        Waves = FindObjectOfType<Waves>();
        //RB = GetComponent<Rigidbody>();
    }

    private void Start() {
        RB.angularDrag = AngularDrag;
    }

    private void FixedUpdate() {
        //RB.centerOfMass = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        for (int i = 0; i < FloatPoints.Length; i++) {
            if (FloatPoints[i].position.y > Waves.GetHeight(FloatPoints[i].position)) {
                RB.drag = AirDrag;
                RB.AddForceAtPosition(new Vector3(0f, Physics.gravity.y,0), FloatPoints[i].position);
                RB.AddTorque(RB.angularVelocity * WaterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            } else {
                RB.drag = WaterDrag;
                RB.AddForceAtPosition(new Vector3(0f, -1 * Physics.gravity.y, 0), FloatPoints[i].position);
                RB.AddTorque(RB.angularVelocity * WaterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, Vector3.one * 1.2f);
        if (FloatPoints == null)
            return;

        for (int i = 0; i < FloatPoints.Length; i++) {
            if (FloatPoints[i] == null)
                continue;
            //Draw Sphere
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(FloatPoints[i].position, 0.1f);
        }
        if (Application.isPlaying) {

        }
    }
}
