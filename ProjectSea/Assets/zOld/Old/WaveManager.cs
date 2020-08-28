using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour {

    public static WaveManager instance;
    [Header("WaveProperties")]
    public float amplitude = 1f;
    public float xlength = 2f;
    public float zlength = 2f;
    public float speed = 1f;
    public float offset = 0f;
    [Header("WeatherProperties")]
    public float timeToChange;
    public bool weatherChange;

    private float _curTimeToChange;
    private float _newX;
    private float _newAmplitude;
    private float _newZ;
    private bool _canChangeAmplitude;
    private bool _plusX, _subX, _plusZ, _subZ, _plusA, _subA;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this){
            Debug.LogWarning("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    private void Start() {
        _curTimeToChange = 0;
        weatherChange = false;
    }

    private void Update() {
        offset += Time.deltaTime * speed;
        _curTimeToChange += Time.deltaTime;
        
        if (xlength <= 2f && xlength >= -2f || zlength <= 2f && zlength >= -2f) {
            amplitude = 0.1f;
        }
         if (_curTimeToChange >= timeToChange) {
            weatherChange = true;
            _newX = GetRandomWeatherNum();
            Debug.Log(_newX);
            _newAmplitude = GetRandomAmplitude();
            Debug.Log(_newAmplitude);
            _newZ = GetRandomWeatherNum();
            Debug.Log(_newZ);
        }

        if (weatherChange) {
            _curTimeToChange = 0;
            if (_newX > xlength) {
                _plusX = true;
            } else if (_newX < xlength) {
                _subX = true;
            }

            if (_newZ > zlength) {
                _plusZ = true;
            } else if (_newZ < zlength) {
                _subZ = true;
            }

            if (_newAmplitude > amplitude) {
                _plusA = true;
            } else if (_newAmplitude < amplitude) {
                _subA = true;
            }

            weatherChange = false;
        }

        if (_plusX) {
            PlusValue(ref(xlength), _newX, ref(_plusX));
        }else if (_subX) {
            SubtractValue(ref (xlength), _newX, ref (_subX));
        }

        if (_plusZ) {
            PlusValue(ref (zlength), _newZ, ref (_plusZ));
        } else if (_subZ) {
            SubtractValue(ref (zlength), _newZ, ref (_subZ));
        }

        if (_plusA) {
            PlusValue(ref (amplitude), _newAmplitude, ref (_plusA));
        } else if (_subA) {
            SubtractValue(ref (amplitude), _newAmplitude, ref (_subA));
        }
    }

    public void PlusValue(ref float _Num, float _Max, ref bool _bool) {
        if (_Num < _Max) {
            _Num += Time.deltaTime / 10;
        } else {
            _bool = false;
        }
    }

    public void SubtractValue(ref float _Num, float _Max, ref bool _bool) {
        if (_Num > _Max) {
            _Num -= Time.deltaTime / 10;
        } else {
            _bool = false;
        }
    }

    public int GetRandomWeatherNum() {
        return Random.Range(2, 5);
    }

    public float GetRandomAmplitude() {
        return Random.Range(0.2f, 1f);
    }
    public float GetWaveHeight(float _x, float _z) {
        var perl = Mathf.PerlinNoise(_x, _z);
        return amplitude * Mathf.Sin(perl + _x/ xlength + offset) * Mathf.Sin(perl + _z/ zlength + offset)/* + Mathf.PI * 2*/;
    }

}
