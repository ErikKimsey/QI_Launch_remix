using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePointer : MonoBehaviour
{
    Vector3 menuPosition;
    float currAngle;
    Vector3 currTouchPosition;
    bool shouldSelect;
    bool shouldPlay = false;

    void Start()
    {
       ParticleSystem ps = GetComponent<ParticleSystem>();
       
        var main = ps.main;

        main.startDelay = 5.0f;
        main.startLifetime = 2.0f;
    }

    void SetPlayState(){

    }

    void UpdateCurrPosition(){

    }

    void UpdateCurrAngle(){

    }

    void OnCollision(){

    }

    void Update()
    {
        
    }
}
