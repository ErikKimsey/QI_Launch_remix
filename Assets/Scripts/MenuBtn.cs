using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBtn : MonoBehaviour
{
    public GameObject btnPrefab;

    public float animationTime = 1f;
    private float lerpCompletion;
    private float lerpStartTime;
    private float angleFromMenuStart;

    private Vector3 startPos, endPos;

    void Start()
    {
      lerpStartTime = Time.time;
      lerpCompletion = 0f;
    }

    public void SetStartEndPos(Vector3 start, Vector3 end){
      startPos = start;
      endPos = end;
    }

    public void SetAngleFromMenuStart(float angle){
      angleFromMenuStart = angle;
    }

    public void Lerp(GameObject clone, Vector3 startPos, Vector3 endPos){
      Vector3 nuPos = Vector3.Lerp(startPos, endPos, lerpCompletion);
      transform.position = nuPos;
    }

    void Update()
    {
      
    }
}
