using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Items")]
public class Item : ScriptableObject {
  public bool unique;
  new public string name;
  public string description;
  public GameObject prop;
  public GameObject heldProp;
}

[System.Serializable]
public class ItemPosition {
  public string name;
  public Transform position;
  public Item currentItem;
  public GameObject currentItemProp;

  public ItemPosition(string nameInit, Transform positionInit) {
    name = nameInit;
    position = positionInit;
  }

  public void SetItem(Item item) {
    currentItem = item;
    currentItemProp = GameObject.Instantiate(item.heldProp, position);
    currentItemProp.transform.localPosition = Vector3.zero;
  }
}