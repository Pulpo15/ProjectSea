using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    [Header("Rotation")]
    public float mouseSensitivity = 100f;

    public Transform PlayerBody;

    float xRotation = 0f;
    float yRotation = 0f;

    [Header("Movement")]
    public Rigidbody RB;

    public float speed = 12f;

    public float gravity = 9.8f;
    public float fallVelocity = 0;

    Vector3 move;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update() {
        //Rotation
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        yRotation -= mouseX;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);

        PlayerBody.localRotation = Quaternion.Euler(0, xRotation, 0);

        //PlayerBody.transform.position = transform.position;

        //Movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        move = transform.right * x + transform.forward * z;

        RB.velocity = move * speed;

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
            RB.velocity = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.Space)) {
            RB.AddForce(new Vector3(0, 10, 0));
        }
    }
}
