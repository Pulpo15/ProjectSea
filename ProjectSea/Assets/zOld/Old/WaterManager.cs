using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class WaterManager : MonoBehaviour {

    private MeshFilter meshFilter;
    private float randomOffset;

    private void Awake() {
        meshFilter = GetComponent<MeshFilter>();
    }

    private void Start() {
        randomOffset = Random.Range(1.1f, 5f);    
    }

    private void Update() {
        Vector3[] vertices = meshFilter.mesh.vertices;
        for (int i = 0; i < vertices.Length; i++) {
            vertices[i].y = WaveManager.instance.GetWaveHeight(transform.position.x + vertices[i].x, transform.position.z + vertices[i].z);
        }
        meshFilter.mesh.vertices = vertices ;
        meshFilter.mesh.RecalculateNormals();
    }
}
