using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBtn : MonoBehaviour
{
    public GameObject btn;
    private Vector3 btnWorldToScreenPosition;
    private Vector3 btnRadiusWorldToScreenPosition;
    private float screenWidth, screenHeight;
    private float leftDist, rightDist, topDist, bottomDist;
    private float greatestXDist, greatestYDist;
    private float[] btnToScreenDistances;
    private float btnRadius;
    private Mesh btnMesh;

    Camera cam;

    void Start()
    {
      cam = Camera.main;

      btnMesh = GetComponent<MeshFilter>().mesh;
      Renderer renderer = GetComponent<Renderer>();
     
      btnWorldToScreenPosition = cam.WorldToScreenPoint(renderer.bounds.center);
      Debug.Log("btnWorldToScreenPosition");
      Debug.Log(btnWorldToScreenPosition);
      screenWidth = Screen.width;
      screenHeight = Screen.height;
      
      CalcRadius(btnMesh);
      CalcGreatestXAxisDistance();
    }

    private void CalcRadius(Mesh mesh){
      Vector3 extents = cam.WorldToScreenPoint(mesh.bounds.extents);
      Vector3 center = cam.WorldToScreenPoint(mesh.bounds.center);
      btnRadius = extents.x - center.x;
    }

    private void CalcGreatestXAxisDistance(){
      rightDist = screenWidth - (btnWorldToScreenPosition.x);
      leftDist = screenWidth - rightDist;
      Debug.Log("leftDist");
      Debug.Log(leftDist);
      Debug.Log("rightDist");
      Debug.Log(rightDist);
      greatestXDist = (leftDist > rightDist) ? leftDist : rightDist;
      Debug.Log(greatestXDist);
    }

    /**
      - Find length of visible arc, around touch position,
      ---- use static radius and 90deg to get arc
      -- AND --
      -# To find direction menu extends towards: #-
      - ? Use screen's center ?- 
      ---- if touch is above or below center on either x-/y-axis 
    */

    private void CalcGreatestYAxisDistance(){

    }

    void Update()
    {
      
    }
}
