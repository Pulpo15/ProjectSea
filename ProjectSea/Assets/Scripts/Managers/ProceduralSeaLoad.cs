using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralSeaLoad : MonoBehaviour {
    
    public int ChunkDistance = 100;

    protected Waves Waves;
    protected int _XChunk;
    protected int _ZChunk;
    private void Awake() {
        Waves = FindObjectOfType<Waves>();
    }

    protected void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Waves.ReloadWaves();
        }
    }
}
