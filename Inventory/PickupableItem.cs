using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Potentially move to local scripts to avoid dependence on Inventory class.

public class PickupableItem : Interaction {

  public Item item;
  public string playerTag = "Player";

  override public void Interact() {
    GameObject.FindWithTag(playerTag).GetComponent<Inventory>().AddItem(item, gameObject);
  }
}