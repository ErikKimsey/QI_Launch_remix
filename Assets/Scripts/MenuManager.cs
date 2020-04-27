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

    MenuBtn menuBtn;
    public GameObject btnPrefab;
    public GameObject subMenuBtn;
    private GameObject menuInstance;

    private float rad2Degs;

    public const float menuRadius = 1f; // hypotenuse for btn coordinates

    public const float menuAngle = 30f;

    private Vector3 menuStartPosition;
    private int menuStartQtyLimit = 1, menuStartQtyCount = 0;

    public float animationTime = 5f;
    private float lerpCompletion = 0.0f;
    private bool isLerping = false;

    private float timeHeld = 0f;
    private bool subMenuTouchEnd = true;
    private float timeTouched;

    private Vector3 screenCenter;
    private float centerX, centerY;
    private float screenWidth, screenHeight;

    public int numbOfMenuItems;
    private GameObject[] buttonArray;

    float pointAngle;
    float incrementAngleValue;

    private bool readyToLerp = false;
    float startTime; 
    public float speed = 1.0F;
    private float distanceLength;

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
          timeTouched = Time.time;
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
    private MenuItemVals CalcItemLocationOnArc( int index, int len ){
      incrementAngleValue = (Mathf.PI/2f) / len + 0.1f;
      float touchAngle = rad2Degs * Mathf.Deg2Rad;
      if (index == 0){
        pointAngle = touchAngle - (incrementAngleValue * (len/2));
      } else {
        pointAngle = pointAngle + incrementAngleValue;
      }
      float x = Mathf.Cos(pointAngle) * 2f;
      float y = Mathf.Sin(pointAngle) * 2f;
      Vector3 screen = new Vector3(x, y, menuInstance.transform.position.z);
      MenuItemVals vals = new MenuItemVals(pointAngle, screen);
      return vals;
    }

    /**
    * Instantiates submenu items.
    */
    IEnumerator CreateSubMenu(){
      buttonArray = new GameObject[numbOfMenuItems];
      for(int i=0; i<numbOfMenuItems; i++){
        MenuItemVals itemVals = CalcItemLocationOnArc(i, numbOfMenuItems);
        Vector3 itemPosition = itemVals.Position;

        itemPosition = itemPosition + menuInstance.transform.position;

        MenuBtn btn = new MenuBtn();

        btn.SetStartEndPos(menuInstance.transform.position, itemPosition);
        btn.SetAngleFromMenuStart(itemVals.Angle);

        GameObject clone = Instantiate(btnPrefab, itemPosition, Quaternion.identity, menuInstance.transform);
        clone.name = i.ToString();
        
        btn.SetName(clone.name);
        
        // GameObject clone = Instantiate(btnPrefab, menuInstance.transform.position, Quaternion.identity, menuInstance.transform);
        Vector3 endPos = itemPosition + menuInstance.transform.position;
        // LerpClones(clone, menuInstance.transform.position, endPos);
        buttonArray[i] = clone;
      }
      readyToLerp = true;
      Debug.Log(readyToLerp);
      startTime = Time.time;
      distanceLength = Vector3.Distance(menuInstance.transform.position, buttonArray[0].transform.position);
      Debug.Log("distanceLength");
      Debug.Log(distanceLength);
      yield return new WaitForSeconds(1f);
    }

    // private void LerpClones(GameObject clone, Vector3 startPos, Vector3 endPos){
    //   Vector3 nuPos = Vector3.Lerp(startPos, endPos, lerpCompletion);
    //   clone.transform.position = nuPos;
    // }

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
      isLerping = false;
      SetStartPosition(touch);
    }

    private void HandleRayCollision(){

    }

    private void HandleTouchMoved(Vector3 touch){
      DetermineHoverItem(touch);
    }

    private void DetermineHoverItem(Vector3 touch){
      Vector3 convertedTouch = Camera.main.ScreenToWorldPoint(touch);
      Debug.DrawLine(menuStartPosition, convertedTouch, Color.magenta);
      Ray ray = Camera.main.ScreenPointToRay(touch);
      RaycastHit hit;
      if (Physics.Raycast(ray, out hit)){
        // get this item
        // apply transform on this item
        if(hit.collider.gameObject.tag == "MenuBtn"){
          hit.collider.gameObject.transform.localScale = hit.collider.gameObject.transform.localScale * 1.005f;
        } 
      } else {
        for(int i=0; i < buttonArray.Length; i++){
          buttonArray[i].gameObject.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
        }
      }
    }

    /**
    * Toggles "subMenuTouchEnd" as true, to tell logic in Update to destroy menu
    */
    private void HandlesubMenuTouchEnded(Vector3 touch){
      subMenuTouchEnd = true;
      isLerping = true;
      timeHeld = 0f;
      menuStartQtyCount = 0;
      ClearMenuInstance();
    }

    private void HandleTouch(){
        if (Input.touchCount > 0)
        {
            Touch mobileTouch = Input.GetTouch (0); 
            switch (mobileTouch.phase)
            {
                case TouchPhase.Began:
                    Vector3 touchPos = new Vector3(mobileTouch.position.x, mobileTouch.position.y, 10f);
                    CalcAngleToCenter(touchPos);
                    screenCenter.z = 0f;
                    HandleTouchBegan (touchPos);
                    break;
                case TouchPhase.Moved:
                Vector3 mTouchPos = new Vector3(mobileTouch.position.x, mobileTouch.position.y, 10f);
                    HandleTouchMoved (mTouchPos);
                    break;
                case TouchPhase.Ended:
                    //  Vector3 touchPos = Camera.main.ScreenToWorldPoint(new Vector3(mobileTouch.position.x, mobileTouch.position.y, 10f));
                     Vector3 endTouchPos = Camera.main.ScreenToWorldPoint(new Vector3(mobileTouch.position.x, mobileTouch.position.y, 10f));
                    HandlesubMenuTouchEnded (endTouchPos);
                    break;
            }
        } 
        else {

          // Debug.Log("NO touchieS");
            // if (Input.GetMouseButtonDown (0))
            // {
            //     Vector3 touch = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f);
            //     CalcAngleToCenter(touch);
            //     screenCenter.z = 0f;
            //     HandleTouchBegan (touch);
            // }
            // if (Input.GetMouseButtonUp (0))
            // {
            //   Vector3 touch = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
            //     HandlesubMenuTouchEnded (touch);
            // }
        }
    }

    void Update()
    {
      HandleTouch();
      if(subMenuTouchEnd == false){
        TimeHeld();
        if(readyToLerp){
          float distCovered = (Time.time - startTime) * speed;
          float fractionOfJourney = distCovered / distanceLength;
          buttonArray[0].transform.position = Vector3.Lerp(menuInstance.transform.position, buttonArray[0].transform.position, fractionOfJourney);
        }
      } 
    }
}
