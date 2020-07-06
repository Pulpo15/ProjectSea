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

    [Header("Jump")]
    public LayerMask groundLayers;
    public CapsuleCollider col;

    public float jumpForce = 7;

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

        transform.localRotation = Quaternion.Euler(xRotation, yRotation / mouseSensitivity, 0);

        PlayerBody.localRotation = Quaternion.Euler(0, yRotation, 0);

        //Movement
        float x = Input.GetAxis("Horizontal") * speed;
        float z = Input.GetAxis("Vertical") * speed;

        Vector3 move = transform.parent.right * x + transform.parent.forward * z;

        //if (RB.velocity.magnitude >= 12f) {
        //    RB.velocity += move - move;
        //} else {
        //}
            RB.velocity += move;

        //if (RB.velocity.magnitude <= 10) {
        //    //RB.AddForce(move);
        //}

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
            RB.velocity = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded()) {
            RB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private bool IsGrounded() {
        return Physics.CheckCapsule(col.bounds.center, new Vector3(col.bounds.center.x, col.bounds.min.y, col.bounds.center.z), col.radius * .9f, groundLayers);
    }
}
