using PhysicsHelp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour {

    [Header("MotorProperties")]
    public Transform Motor;
    public float steerPower = 500f;
    public float power = 5f;
    public float maxSpeed = 10f;
    public float drag = 0.1f;

    protected Rigidbody RB;
    protected Quaternion StartRotation;

    private void Awake() {
        RB = GetComponent<Rigidbody>();
        StartRotation = Motor.localRotation;
    }

    private void FixedUpdate() {
        Vector3 forceDirection = transform.forward;
        float steer = 0;

        if (Input.GetKey(KeyCode.A))
            steer = 1;
        if (Input.GetKey(KeyCode.D))
            steer = -1;

        //RB.AddForceAtPosition(steer * transform.right * steerPower, Motor.position);
        RB.AddTorque(steer * transform.up * steerPower);
        //transform.rotation = new Quaternion(transform.rotation.x * steerPower * steer, transform.rotation.y, transform.rotation.z, transform.rotation.w);

        Vector3 forward = Vector3.Scale(new Vector3(1, 0, 1), transform.forward);

        if (Input.GetKey(KeyCode.W))
            PhysicsHelper.ApplyForceToReachVelocity(RB, forward * maxSpeed, power);
        if (Input.GetKey(KeyCode.S))
            PhysicsHelper.ApplyForceToReachVelocity(RB, forward * -maxSpeed, power);


        //transform.Translate(Vector3.forward + Vector3.right * 0.1f);
        //RB.AddForce(transform.forward * Time.deltaTime * speed);
    }
}

