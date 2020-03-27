using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
  - On hold (( 2sec )): display main item, and scaled-down submenu items,
  - On hold and move toward submenu items: submenu items scale up
  - On over submenu item and hold (( 2sec )): acts as item selection
*/

public class MenuManager : MonoBehaviour
{
    public GameObject btnPrefab;
    private GameObject menuInstance;
    private float timeDown = 0f;
    private Vector3 menuStartPosition;
    private int menuStartQytyLimit = 1, menuStartQytyCount = 0;

    private float timeHeld = 0f;
    private bool touchEnd = true;



    void Start()
    {
      Debug.Log(menuStartPosition);
    }

    private void TimeHeld(){
        timeHeld += Time.deltaTime;
        if(timeHeld > 0.5f && menuStartQytyCount < menuStartQytyLimit){
          InstantiateMenu();
          menuStartQytyCount+=1;
          return;
        }
    } 

    private void InstantiateMenu(){
      menuInstance = Instantiate(btnPrefab,menuStartPosition, Quaternion.identity);
    }

    private void ClearMenuInstance(){
      Destroy(menuInstance, 0.3f);
    }

    private void SetStartPosition(Vector3 touch){
      menuStartPosition = touch;
    }

    private void HandleTouchBegan(Vector3 touch){
      touchEnd = false;
      SetStartPosition(touch);
    }
    private void HandleTouchMoved(Vector3 touch){
      Debug.Log(touch);
    }
    private void HandleTouchEnded(Vector3 touch){
      touchEnd = true;
      timeHeld = 0f;
      menuStartQytyCount = 0;
      ClearMenuInstance();
    }



    private void HandleTouch(){
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch (0); 
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    HandleTouchBegan (touch.position);
                    break;
                case TouchPhase.Moved:
                    HandleTouchMoved (touch.position);
                    break;
                case TouchPhase.Ended:
                    HandleTouchEnded (touch.position);
                    break;
            }
        } 
        else {
            if (Input.GetMouseButtonDown (0))
            {
                Vector3 touch = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
                HandleTouchBegan (touch);
            }
            if (Input.GetMouseButtonUp (0))
            {
              Vector3 touch = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
                HandleTouchEnded (touch);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
      HandleTouch();
      if(touchEnd == false){
        TimeHeld();
      } 
    }
}
