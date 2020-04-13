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


    /**
    * Watches initial time that touch is held.  (Touch must be held mininum number of seconds before menu is instantiated.)
    */
    private void TimeHeld(){
        timeHeld += Time.deltaTime;
        if(timeHeld > 0.5f && menuStartQtyCount < menuStartQtyLimit){
          InstantiateMenu();
          menuStartQtyCount+=1;
          return;
        }
    } 

    /**
    * Calculates angle from touch to center of screen
    */
    private float CalcAngleToCenter(Vector3 touch){
      Vector3 direction = touch - screenCenter;
      rad2Degs = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180f;
      return rad2Degs;
    }

    /**
    * Calculates submenu item location along arc
    */
    private Vector3 CalcItemLocationOnArc( int index, int len ){

      incrementAngleValue = (Mathf.PI/2f) / len;

      float touchAngle = rad2Degs * Mathf.Deg2Rad;
      if (index == 0){
        pointAngle = touchAngle - (incrementAngleValue * (len/2));
      } else {
        pointAngle = pointAngle + incrementAngleValue;
      }
      float x = Mathf.Cos(pointAngle) * 2f;
      float y = Mathf.Sin(pointAngle) * 2f;
      Vector3 screen = new Vector3(x, y, menuInstance.transform.position.z);
      return screen;
    }

    /**
    * Instantiates submenu items.
    */
    IEnumerator CreateSubMenu(){
      buttonArray = new GameObject[numbOfMenuItems];
      for(int i=0; i<numbOfMenuItems; i++){
        Vector3 itemPosition = CalcItemLocationOnArc(i, numbOfMenuItems);
        GameObject clone = Instantiate(btnPrefab, itemPosition + menuInstance.transform.position, Quaternion.identity, menuInstance.transform);
        buttonArray[i] = clone;
      }
      yield return new WaitForSeconds(1f);
    }


    /**
    * Finds center of screen (in screen points)
    */
    private void SetScreenCenter(){
      centerX = Screen.width / 2;
      centerY = Screen.height / 2;
      screenCenter = new Vector3(centerX, centerY, 0f);
    }

    /**
    * menu instance instantiated at touch position ("menuStartPosition")
    */
    private void InstantiateMenu(){
      menuInstance = Instantiate(subMenuBtn, menuStartPosition, Quaternion.identity);
      StartCoroutine(CreateSubMenu());
    }

    /**
    * Upon touchEnd, destroys instances of initial menu item and submenu items 
    */
    private void ClearMenuInstance(){
      for (int i = 0; i < buttonArray.Length; i++)
      {
        Destroy(buttonArray[i]);
      }
      Destroy(menuInstance, 0.3f);
    }

    /**
    * Sets menu position in world points
    */
    private void SetStartPosition(Vector3 touch){
      menuStartPosition = Camera.main.ScreenToWorldPoint(touch);
    }

    /**
    * Toggles "subMenuTouchEnd" as false, to tell logic in Update to instantiate menu
    */
    private void HandleTouchBegan(Vector3 touch){
      subMenuTouchEnd = false;
      SetStartPosition(touch);
    }

    private void HandleTouchMoved(Vector3 touch){
      // Debug.Log(touch);
    }


    /**
    * Toggles "subMenuTouchEnd" as true, to tell logic in Update to destroy menu
    */
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
                    // Vector3 touch = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f);
                    // CalcAngleToCenter(touch);
                    // screenCenter.z = 0f;
                    // HandleTouchBegan (touch);
                    break;
                case TouchPhase.Moved:
                    // HandleTouchMoved (touch.position);
                    break;
                case TouchPhase.Ended:
                    //  Vector3 touch = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
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
