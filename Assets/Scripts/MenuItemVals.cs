
using UnityEngine;

public struct MenuItemVals {
  public float Angle {get; set;}
  public Vector3 Position {get; set;}

  public MenuItemVals(float angle, Vector3 position){
    Angle = angle;
    Position = position;
  }
};