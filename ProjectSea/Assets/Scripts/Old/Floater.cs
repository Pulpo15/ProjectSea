using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour {

    public Rigidbody RB;
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
    public int floaterCount = 1;
    public float waterDrag = 0.99f;
    public float waterAngularDrag = 0.5f;

    private void FixedUpdate() {
        RB.AddForceAtPosition(Physics.gravity / floaterCount, transform.position, ForceMode.Acceleration);

        float waveHeight = WaveManager.instance.GetWaveHeight(transform.position.x, transform.position.z);
        if (transform.position.y < waveHeight) {
            float displacementMultiplier = Mathf.Clamp01((waveHeight - transform.position.y) / depthBeforeSubmerged) * displacementAmount;
            RB.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementAmount, 0f), transform.position ,ForceMode.Acceleration);
            RB.AddForce(displacementMultiplier * -RB.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            RB.AddTorque(displacementMultiplier * -RB.angularVelocity * waterAngularDrag* Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }

    private void Update() {
        if (WaveManager.instance.amplitude == 0)
            displacementAmount = 0.2f;
        else if (WaveManager.instance.amplitude >= 0 && WaveManager.instance.amplitude <= 0.5f)
            displacementAmount = 0.4f;
        else if (WaveManager.instance.amplitude > 0.5f)
            displacementAmount = 0.8f;
    }
}
