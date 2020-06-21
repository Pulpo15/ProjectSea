using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour {

    public Rigidbody RB;
    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 3f;
    public int floaterCount = 1;
    public float waterDrag = 0.99f;
    public float waterAngularDrag = 0.5f;

    private void FixedUpdate() {
        RB.AddForceAtPosition(Physics.gravity / floaterCount, transform.position, ForceMode.Acceleration);

        float waveHeight = WaveManager.instance.GetWaveHeight(transform.position.x);
        if (transform.position.y < waveHeight) {
            float displacementMultiplier = Mathf.Clamp01((waveHeight - transform.position.y) / depthBeforeSubmerged) * displacementAmount;
            RB.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementAmount, 0f), transform.position ,ForceMode.Acceleration);
            RB.AddForce(displacementMultiplier * -RB.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            RB.AddTorque(displacementMultiplier * -RB.angularVelocity * waterAngularDrag* Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }
}
