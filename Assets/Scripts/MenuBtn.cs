using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBtn : MonoBehaviour
{
    public GameObject btnPrefab;

    public float animationTime = 5f;
    private float lerpCompletion = 0.0f;
    private float lerpStartTime;

    void Start()
    {
      lerpStartTime = Time.time;
    }

    public void Lerp(GameObject clone, Vector3 startPos, Vector3 endPos){
      Vector3 nuPos = Vector3.Lerp(startPos, endPos, lerpCompletion);
      transform.position = nuPos;
    }

    void Update()
    {
      
    }
}
