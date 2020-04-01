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

    public const int menuRadius = 200;
    public const int menuAngle = 90;

    private Vector3 menuStartPosition;
    private int menuStartQytyLimit = 1, menuStartQytyCount = 0;

    private float timeHeld = 0f;
    private bool subMenuTouchEnd = true;

    private Vector3 screenCenter;
    private float centerX, centerY;
    private float screenWidth, screenHeight;

    public int numbOfMenuItems;
    private GameObject[] buttonArray;


    void Start()
    {
      SetScreenCenter();
    }

    private void TimeHeld(){
        timeHeld += Time.deltaTime;
        if(timeHeld > 0.5f && menuStartQytyCount < menuStartQytyLimit){
          InstantiateMenu();
          menuStartQytyCount+=1;
          return;
        }
    } 

    IEnumerator CreateSubMenu(){
      Debug.Log("DELAYED");
      for(int i=0; i<numbOfMenuItems; i++){
         Instantiate(btnPrefab,new Vector3(2 * i,2 * i,menuStartPosition.z),Quaternion.identity);
      }
      yield return new WaitForSeconds(1f);
    }

    private void CalcItemLocationOnArc(){

    }

    private void SetScreenCenter(){
      centerX = Screen.width / 2;
      centerY = Screen.height / 2;
      screenCenter = new Vector3(centerX, centerY, 0f);
    }

    private void InstantiateMenu(){
      menuInstance = Instantiate(subMenuBtn, menuStartPosition, Quaternion.identity);
      StartCoroutine(CreateSubMenu());
    }

    private void ClearMenuInstance(){
      Destroy(menuInstance, 0.3f);
      Destroy(btnPrefab);
    }

    private void SetStartPosition(Vector3 touch){
      menuStartPosition = Camera.main.ScreenToWorldPoint(touch);
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
      menuStartQytyCount = 0;
      ClearMenuInstance();
    }

    private float CalcAngleToCenter(Vector3 touch){
      Vector3 direction = touch - screenCenter;
      float rad2Deg = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180f;
      return rad2Deg;
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
                Vector3 touch = (new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
                float angle = CalcAngleToCenter(touch);
                screenCenter.z = 10f;
                // Debug.DrawRay(touch, screenCenter, Color.cyan);
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
