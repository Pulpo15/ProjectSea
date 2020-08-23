using PhysicsHelp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFloat : MonoBehaviour {

    public float AirDrag = 1;
    public float WaterDrag = 10;
    public Transform[] FloatPoints;
    public bool AttachToSurface;

    protected Rigidbody RB;
    protected Waves Waves;

    protected float WaterLine;
    protected Vector3[] WaterLinePoints;

    protected Vector3 centerOffset;
    protected Vector3 smoothVectorRotation;
    protected Vector3 TargetUp;

    public Vector3 Center { get { return transform.position + centerOffset; } }

    private void Awake() {
        Waves = FindObjectOfType<Waves>();
        RB = GetComponent<Rigidbody>();
        RB.useGravity = false;

        WaterLinePoints = new Vector3[FloatPoints.Length];
        for (int i = 0; i < FloatPoints.Length; i++)
            WaterLinePoints[i] = FloatPoints[i].position;
        centerOffset = PhysicsHelper.GetCenter(WaterLinePoints) - transform.position;
    }

    private void Update() {
        
        //Default Water Surface
        var newWaterLine = 0f;
        var pointUnderWater = false;

        //Set WaterLinePoints and WaterLine
        for (int i = 0; i < FloatPoints.Length; i++) {
            //Height
            WaterLinePoints[i] = FloatPoints[i].position;
            WaterLinePoints[i].y = Waves.GetHeight(FloatPoints[i].position);
            newWaterLine += WaterLinePoints[i].y / FloatPoints.Length;
            if (WaterLinePoints[i].y > FloatPoints[i].position.y)
                pointUnderWater = true;
        }

        var waterLineDelta = newWaterLine - WaterLine;
        WaterLine = newWaterLine;

        //Gravity

        var gravity = Physics.gravity;
        RB.drag = AirDrag;
        if (WaterLine > Center.y) {
            RB.drag = WaterDrag;
            if (AttachToSurface) {
                RB.position = new Vector3(RB.position.x, WaterLine - centerOffset.y, RB.position.z);
            } else {
                gravity = -Physics.gravity;
                transform.Translate(Vector3.up * waterLineDelta * 0.9f);
            }
            
        }
        RB.AddForce(gravity * Mathf.Clamp(Mathf.Abs(WaterLine - Center.y), 0, 1));

        TargetUp = PhysicsHelper.GetNormal(WaterLinePoints);

        if (pointUnderWater) {
            TargetUp = Vector3.SmoothDamp(transform.up, TargetUp, ref smoothVectorRotation, 0, 2f);
            RB.rotation = Quaternion.FromToRotation(transform.up, TargetUp) * RB.rotation;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        if (FloatPoints == null)
            return;

        for (int i = 0; i < FloatPoints.Length; i++) {
            if (FloatPoints[i] == null)
                continue;

            if (Waves != null) {
                //Draw Cube
                Gizmos.color = Color.red;
                Gizmos.DrawCube(WaterLinePoints[i], Vector3.one * 0.3f);
            }
            //Draw Sphere
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(FloatPoints[i].position, 0.1f);
        }

        //Draw Center
        if (Application.isPlaying) {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(new Vector3(Center.x, WaterLine, Center.z), Vector3.one * 1f);
        }
    }
}
