using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
  - MainMenu Prefab contains:
  -- subMenu item prefabs,
  -- menu functionality
*/

/**
  - On hold (( 2sec )): display main item
  - On hold (( +1sec )): main menu item scales 2x size && , and scaled-down submenu items,
  - On hold and move touch toward submenu items: indicated submenu item scale up
  - On mover over submenu item and hold (( 2sec )): acts as item selection

  - On touch end: destroy menu instance
*/

public class MenuManager : MonoBehaviour
{
    public GameObject btnPrefab;
    public GameObject subMenuBtn;
    private GameObject menuInstance;

    private float rad2Degs;

    public const float menuRadius = 1f; // hypotenuse for btn coordinates

    public const float menuAngle = 30f;

    private Vector3 menuStartPosition;
    private int menuStartQtyLimit = 1, menuStartQtyCount = 0;

    private float timeHeld = 0f;
    private bool subMenuTouchEnd = true;

    private Vector3 screenCenter;
    private float centerX, centerY;
    private float screenWidth, screenHeight;

    public int numbOfMenuItems;
    private GameObject[] buttonArray;

    float pointAngle;
    float incrementAngleValue;


    void Start()
    {
      SetScreenCenter();
    }

    private void TimeHeld(){
        timeHeld += Time.deltaTime;
        if(timeHeld > 0.5f && menuStartQtyCount < menuStartQtyLimit){
          InstantiateMenu();
          menuStartQtyCount+=1;
          return;
        }
    } 

    private float CalcAngleToCenter(Vector3 touch){
      Vector3 direction = touch - screenCenter;
      Debug.Log("direction");
      Debug.Log(direction);
      rad2Degs = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180f;
      Debug.Log("rad2Degs");
      Debug.Log(rad2Degs);
      return rad2Degs;
    }

    private Vector3 CalcItemLocationOnArc( int index, int len ){

      // in radians
      incrementAngleValue = (Mathf.PI/2f) / len;
      Debug.Log("incrementAngleValue");
      Debug.Log(incrementAngleValue);

      // arc = r * theta
      /**
        - 1. Touch position (touch.x, touch.y) is (0,0) for menu circle.
        - 2. Create arbitrary point: Use "touch" 
        - 3. Distance from touch to arbitrary point on circle: Vector3 direction = arbitraryPoint - touch;
          --- ex: arb2Touch = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180f;
        - 3. 
      */


      // April 9, 2020
      // 1. 

      float touchAngle = rad2Degs * Mathf.Deg2Rad;
      if (index == 0){
        // pointAngle = touchAngle - incrementAngleValue * (len/2);
        pointAngle = touchAngle - (incrementAngleValue * (len/2));
      } else {
        pointAngle = pointAngle + incrementAngleValue;
      }
      Debug.Log("pointAngle");
      Debug.Log(pointAngle);
      // float x = menuRadius * Mathf.Sin(pointAngle);
      float x = Mathf.Cos(pointAngle) * 2f;
      float y = Mathf.Sin(pointAngle) * 2f;
      Vector3 screen = new Vector3(x, y, menuInstance.transform.position.z);
      return screen;
    }

    IEnumerator CreateSubMenu(){
      buttonArray = new GameObject[numbOfMenuItems];
      for(int i=0; i<numbOfMenuItems; i++){
        Vector3 itemPosition = CalcItemLocationOnArc(i, numbOfMenuItems);
        Debug.Log("point location");
        Debug.Log(itemPosition);
        GameObject clone = Instantiate(btnPrefab, itemPosition + menuInstance.transform.position, Quaternion.identity, menuInstance.transform);
        buttonArray[i] = clone;
        Debug.DrawRay(menuStartPosition, itemPosition, Color.red);
      }
      yield return new WaitForSeconds(1f);
    }

    private void SetScreenCenter(){
      centerX = Screen.width / 2;
      centerY = Screen.height / 2;
      // screenCenter = Camera.main.ScreenToWorldPoint(new Vector3(centerX, centerY, 0f));
      screenCenter = new Vector3(centerX, centerY, 0f);
    }

    private void InstantiateMenu(){
      menuInstance = Instantiate(subMenuBtn, menuStartPosition, Quaternion.identity);
      StartCoroutine(CreateSubMenu());
    }

    private void ClearMenuInstance(){
      for (int i = 0; i < buttonArray.Length; i++)
      {
        Destroy(buttonArray[i]);
      }
      Destroy(menuInstance, 0.3f);
    }

    private void SetStartPosition(Vector3 touch){
      // Debug.Log("touch");
      // Debug.Log(touch);
      menuStartPosition = Camera.main.ScreenToWorldPoint(touch);
      Debug.Log("menuStartPosition");
      Debug.Log(menuStartPosition);
    }

    private void HandleTouchBegan(Vector3 touch){
      subMenuTouchEnd = false;
      SetStartPosition(touch);
    }

    private void HandleTouchMoved(Vector3 touch){
      // Debug.Log(touch);
    }

    private void HandlesubMenuTouchEnded(Vector3 touch){
      subMenuTouchEnd = true;
      timeHeld = 0f;
      menuStartQtyCount = 0;
      ClearMenuInstance();
    }

    private void HandleTouch(){
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch (0); 
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // HandleTouchBegan (touch.position);
                    // Vector3 touch = (new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
                    // float angle = CalcAngleToCenter(touch);
                    // screenCenter.z = 10f;
                    // HandleTouchBegan (touch);
                    break;
                case TouchPhase.Moved:
                    // HandleTouchMoved (touch.position);
                    break;
                case TouchPhase.Ended:
                //     HandlesubMenuTouchEnded (touch.position);
                //     Vector3 touch = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
                // HandlesubMenuTouchEnded (touch);
                    break;
            }
        } 
        else {
            if (Input.GetMouseButtonDown (0))
            {
                Vector3 touch = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f);
                CalcAngleToCenter(touch);
                screenCenter.z = 0f;
                HandleTouchBegan (touch);
            }
            if (Input.GetMouseButtonUp (0))
            {
              Vector3 touch = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
                HandlesubMenuTouchEnded (touch);
            }
        }
    }

    void Update()
    {
      HandleTouch();
      if(subMenuTouchEnd == false){
        TimeHeld();
      } 
    }
}
