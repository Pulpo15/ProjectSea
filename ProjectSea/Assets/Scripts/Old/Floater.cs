using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour {

    public float depthBeforeSubmerged = 1f;
    #region displacementAmount
    //Remember to modify displacementAmount acording to the Sea Amplitude
    //if Sea Amplitude == 0 DisplacementAmount has to be 0.2f to lower 
    //the Torque bug on the Ship, Modify this value acording to the Sea
    //Amplitude but never below 0.1f and only use this value for sinking Ships
    public float displacementAmount = 3f;
    //Remember to modify displacementAmount acording to the Sea Amplitude
    //if Sea Amplitude == 0 DisplacementAmount has to be 0.2f to lower 
    //the Torque bug on the Ship, Modify this value acording to the Sea
    //Amplitude but never below 0.1f and only use this value for sinking Ships
    #endregion
    public float waterDrag = 0.99f;
    public float waterAngularDrag = 0.5f;
    public Transform[] FloatPoints;

    public Rigidbody RB;
    protected Waves Waves;

    private void Start() {
        Waves = FindObjectOfType<Waves>();
        //RB = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        RB.AddForceAtPosition(Physics.gravity / FloatPoints.Length, transform.position, ForceMode.Acceleration);

        float waveHeight = Waves.GetHeight(transform.position);
        if (transform.position.y < waveHeight) {
            float displacementMultiplier = Mathf.Clamp01((waveHeight - transform.position.y) / depthBeforeSubmerged) * displacementAmount;
            RB.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementAmount, 0f), transform.position ,ForceMode.Acceleration);
            RB.AddForce(displacementMultiplier * -RB.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            RB.AddTorque(displacementMultiplier * -RB.angularVelocity * waterAngularDrag* Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        if (FloatPoints == null)
            return;

        for (int i = 0; i < FloatPoints.Length; i++) {
            if (FloatPoints[i] == null)
                continue;

            //if (Waves != null) {
            //    //Draw Cube
            //    Gizmos.color = Color.red;
            //    Gizmos.DrawCube(WaterLinePoints[i], Vector3.one * 0.3f);
            //}
            //Draw Sphere
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(FloatPoints[i].position, 0.1f);
        }

        //Draw Center
        //if (Application.isPlaying) {
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawCube(new Vector3(Center.x, WaterLine, Center.z), Vector3.one * 1f);
        //    Gizmos.DrawRay(new Vector3(Center.x, WaterLine, Center.z), TargetUp * 1f);
        //}
    }

    private void Update() {
        //if (WaveManager.instance.amplitude == 0)
        //    displacementAmount = 0.2f;
        //else if (WaveManager.instance.amplitude >= 0 && WaveManager.instance.amplitude <= 0.5f)
        //    displacementAmount = 0.4f;
        //else if (WaveManager.instance.amplitude > 0.5f)
        //    displacementAmount = 0.8f;
    }
}
