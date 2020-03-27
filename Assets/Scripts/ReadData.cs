using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadData : MonoBehaviour {


  [System.Serializable]
  public class MenuItem {
    public string menuItem;
  }

  [System.Serializable]
  public class MenuItems {
    public MenuItem[] menuItems;
  }


  public TextAsset menuJson;

  private void Start()
  {

      MenuItems menuItemsArr = JsonUtility.FromJson<MenuItems>(menuJson.text);
      foreach (MenuItem menuItem in menuItemsArr.menuItems)
        {
            Debug.Log("states: " + menuItem.menuItem);
        }
  }
}

