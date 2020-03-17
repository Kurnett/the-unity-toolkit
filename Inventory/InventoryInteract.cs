using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Decouple inventory inheritance from interaction inheritance.

public class InventoryInteract : Interaction {

  Inventory player;

  public Item requiredItem;
  public bool shouldRemoveItem = false;

  public Interaction target;

  void Start() {
    player = GameObject.FindWithTag("Player").GetComponent<Inventory>();
  }

  override public void Interact() {
    if (requiredItem != null && target && player != null) {
      if (player.GetItem(requiredItem) != null) {
        target.Interact();
        if (shouldRemoveItem) {
          player.RemoveItem(requiredItem);
        }
      }
    }
  }
}
