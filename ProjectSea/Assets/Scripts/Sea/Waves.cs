using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour {
    public int dimension = 10;
    public float UVScale;
    public Octave[] Octaves;

    protected MeshFilter MeshFilter;
    public Mesh Mesh;

    protected void Start() {
        Mesh = new Mesh();
        Mesh.name = gameObject.name;

        Mesh.vertices = GenerateVerts();
        Mesh.triangles = GenerateTries();
        Mesh.uv = GenerateUV();
        Mesh.RecalculateBounds();
        Mesh.RecalculateNormals();

        MeshFilter = gameObject.AddComponent<MeshFilter>();
        MeshFilter.mesh = Mesh;
    }

    public void ReloadWaves(int _dimension) {
        dimension = _dimension;
        Mesh.vertices = GenerateVerts();
        Mesh.triangles = GenerateTries();
        Mesh.uv = GenerateUV();
        Mesh.RecalculateBounds();
        Mesh.RecalculateNormals();
    }

    private Vector2[] GenerateUV() {
        var uvs = new Vector2[Mesh.vertices.Length];

        for (int x = 0; x <= dimension; x++) {
            for (int z = 0; z <= dimension; z++) {
                var vec = new Vector2((x / UVScale) % 2, (z / UVScale) % 2);
                uvs[index(x, z)] = new Vector2(vec.x <= 1 ? vec.x : 2 - vec.x, vec.y <= 1 ? vec.y : 2 - vec.y);
            }
        }

        return uvs;
    }

    private Vector3[] GenerateVerts() {
        var verts = new Vector3[(dimension + 1) * (dimension + 1)];

        for (int x = 0; x <= dimension; x++) {
            for (int z = 0; z <= dimension; z++) {
                verts[index(x, z)] = new Vector3(x, 0, z);
            }
        }
        return verts;
    }

    private int index(float x, float z) {
        return (int)(x * (dimension + 1) + z);
    }

    private int[] GenerateTries() {
        var tries = new int[Mesh.vertices.Length * 6];

        for (int x = 0; x < dimension; x++) {
            for (int z = 0; z < dimension; z++) {
                tries[index(x, z) * 6 + 0] = index(x, z);
                tries[index(x, z) * 6 + 1] = index(x + 1, z + 1);
                tries[index(x, z) * 6 + 2] = index(x + 1, z);
                tries[index(x, z) * 6 + 3] = index(x, z);
                tries[index(x, z) * 6 + 4] = index(x, z + 1);
                tries[index(x, z) * 6 + 5] = index(x + 1, z + 1);
            }
        }
        return tries;
    }

    public float GetHeight(Vector3 position) {
        var scale = new Vector3(1 / transform.lossyScale.x, 0, 1 / transform.lossyScale.z);
        var localPos = Vector3.Scale((position - transform.position), scale);

        var p1 = new Vector3(Mathf.Floor(localPos.x), 0, Mathf.Floor(localPos.z));
        var p2 = new Vector3(Mathf.Floor(localPos.x), 0, Mathf.Ceil(localPos.z));
        var p3 = new Vector3(Mathf.Ceil(localPos.x), 0, Mathf.Floor(localPos.z));
        var p4 = new Vector3(Mathf.Ceil(localPos.x), 0, Mathf.Ceil(localPos.z));

        p1.x = Mathf.Clamp(p1.x, 0, dimension);
        p1.z = Mathf.Clamp(p1.z, 0, dimension);
        p2.x = Mathf.Clamp(p2.x, 0, dimension);
        p2.z = Mathf.Clamp(p2.z, 0, dimension);
        p3.x = Mathf.Clamp(p3.x, 0, dimension);
        p3.z = Mathf.Clamp(p3.z, 0, dimension);
        p4.x = Mathf.Clamp(p4.x, 0, dimension);
        p4.z = Mathf.Clamp(p4.z, 0, dimension);

        var max = Mathf.Max(Vector3.Distance(p1, localPos), Vector3.Distance(p2, localPos), Vector3.Distance(p3, localPos), Vector3.Distance(p4, localPos) + Mathf.Epsilon);
        var dist = (max - Vector3.Distance(p1, localPos))
            + (max - Vector3.Distance(p2, localPos))
            + (max - Vector3.Distance(p3, localPos))
            + (max - Vector3.Distance(p4, localPos) + Mathf.Epsilon);

        var height = Mesh.vertices[index(p1.x, p1.z)].y * (max - Vector3.Distance(p1, localPos))
            + Mesh.vertices[index(p2.x, p2.z)].y * (max - Vector3.Distance(p2, localPos))
            + Mesh.vertices[index(p3.x, p3.z)].y * (max - Vector3.Distance(p3, localPos))
            + Mesh.vertices[index(p4.x, p4.z)].y * (max - Vector3.Distance(p4, localPos));

        return height / dist;
    }

    protected virtual void Update() {
        var verts = Mesh.vertices;
        for (int x = 0; x <= dimension; x++) {
            for (int z = 0; z <= dimension; z++) {
                var y = 0f;
                for (int i = 0; i < Octaves.Length; i++) {
                    if (Octaves[i].alternate) {
                        var perl = Mathf.PerlinNoise((x + Octaves[i].scale.x) / dimension, (z * Octaves[i].scale.y) / dimension) * Mathf.PI * 2;
                        y += Mathf.Cos(perl + Octaves[i].speed.magnitude * Time.time) * Octaves[i].height;
                    } else {
                        var perl = Mathf.PerlinNoise((x + Octaves[i].scale.x + Time.time * Octaves[i].speed.x) / dimension, (z * Octaves[i].scale.y + Time.time * Octaves[i].speed.y) / dimension) * Mathf.PI * 2;
                        y += perl + Octaves[i].height;
                    }
                }
                verts[index(x, z)] = new Vector3(x, y, z);
            }
        }

        Mesh.vertices = verts;
        Mesh.RecalculateNormals();
    }

    [Serializable]
    public struct Octave {
        public Vector2 speed;
        public Vector2 scale;
        public float height;
        public bool alternate;
    }
}
