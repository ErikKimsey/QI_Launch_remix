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

    private string itemText;
    private string itemLink;
    private int itemArrayIndex;

    private Vector3 startPos, endPos;

    void Start(){
      lerpStartTime = Time.time;
      lerpCompletion = 0f;
    }

    public void SetStartEndPos(Vector3 start, Vector3 end){
      startPos = start;
      endPos = end;
    }

    public void SetLink(string link){
      itemLink = link;
    }

    public void SetText(string text){
      itemText = text;
    }

    public void SetItemIndex(int i){
      itemArrayIndex = i;
    }

    public void SetAngleFromMenuStart(float angle){
      angleFromMenuStart = angle;
    }

    public void Lerp(GameObject clone, Vector3 startPos, Vector3 endPos){
      Vector3 nuPos = Vector3.Lerp(startPos, endPos, lerpCompletion);
      transform.position = nuPos;
    }

    void Update(){
      
    }
}
