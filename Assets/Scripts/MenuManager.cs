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

    public const float menuRadius = 20f; // hypotenuse for btn coordinates

    public const float menuAngle = 5f;

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
      rad2Degs = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180f;
      Debug.Log("rad2Degs");
      Debug.Log(rad2Degs);
      return rad2Degs;
    }

    private Vector3 CalcItemLocationOnArc( int index, int len ){

      incrementAngleValue = menuAngle / len;

      if(index == 0){
        pointAngle = rad2Degs;
      } else if (index > 0) {
        // pointAngle = pointAngle + incrementAngleValue;
      }
      float x = menuRadius * Mathf.Sin(rad2Degs);
      Debug.Log("x");
      Debug.Log(x);
      float y = menuRadius * Mathf.Cos(rad2Degs);
      Debug.Log("y");
      Debug.Log(y);

      float z = 10f;
      return new Vector3(x, y, z);
    }

    IEnumerator CreateSubMenu(){
      buttonArray = new GameObject[numbOfMenuItems];
      for(int i=0; i<numbOfMenuItems; i++){
        Vector3 itemPosition = CalcItemLocationOnArc(i, numbOfMenuItems);
        // Debug.Log("itemPosition");
        // Debug.Log(itemPosition);
         GameObject clone = Instantiate(btnPrefab, new Vector3(itemPosition.x, itemPosition.y, menuStartPosition.z),Quaternion.identity);
        buttonArray[i] = clone;
        Debug.DrawRay(menuStartPosition, itemPosition, Color.red);
      }
      yield return new WaitForSeconds(1f);
    }

    private void SetScreenCenter(){
      centerX = Screen.width / 2;
      centerY = Screen.height / 2;
      screenCenter = new Vector3(centerX, centerY, 0f);
      Debug.Log(screenCenter);
      Vector3 world
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
      menuStartPosition = touch;
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
                Vector3 touch = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
                CalcAngleToCenter(touch);
                screenCenter.z = 10f;
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
