using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBtn : MonoBehaviour
{
    public GameObject btn;
    private Vector3 btnWorldToScreenPosition;
    private Vector3 btnRadiusWorldToScreenPosition;
    private float[] btnToScreenDistances;
    private float btnRadius;
    private Mesh btnMesh;

    Camera cam;

    void Start()
    {
      cam = Camera.main;
      btnMesh = GetComponent<MeshFilter>().mesh;
      Renderer renderer = GetComponent<Renderer>();
      btnWorldToScreenPosition = renderer.bounds.center;  
      CalcRadius(btnMesh);
    }

    private void CalcRadius(Mesh mesh){
      Vector3 extents = mesh.bounds.extents;
      Vector3 center = mesh.bounds.center;
      btnRadius = extents.x - center.x;
    }


    void Update()
    {
      Debug.Log(transform.position);
    }
}
