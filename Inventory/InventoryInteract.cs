using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryInteract : MonoBehaviour
{

    Inventory player;

    public Item requiredItem;
    public bool shouldRemoveItem = false;

    public GameObject target;

    void Start () {
        player = GameObject.FindWithTag("Player").GetComponent<Inventory>();
    }

    public void Interact () {
        if (requiredItem != null && target && player != null) {
            if (player.GetItem(requiredItem) != null) {
                target.SendMessage("InteractInventory");
                if (shouldRemoveItem) {
                    player.RemoveItem(requiredItem);
                }
            }
        }
    }
}
