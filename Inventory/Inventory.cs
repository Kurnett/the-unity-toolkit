using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public bool canPickUpItems = true;
    public bool autoHoldItems = false;
    public string autoHoldPositionName = "";
    ItemPosition autoHoldPosition;

    public List<Item> inventory = new List<Item> ();
    public List<ItemPosition> itemPositions = new List<ItemPosition> ();

    void Start () {
        autoHoldPosition = FindPositionByName (autoHoldPositionName);
    }

    public Item GetItem (string name) {
        return FindItemInInventory (name);
    }

    public Item GetItem (Item item) {
        return FindItemInInventory (item);
    }

    public void AddItem (Item item) {
        if (canPickUpItems) {
            AddToInventory (item);
        }
    }

    public void AddItem (Item item, GameObject prop) {
        if (canPickUpItems) {
            AddToInventory (item);
            Destroy (prop);
        }
    }

    void AddToInventory (Item item) {
        inventory.Add (item);
        if (autoHoldItems && autoHoldPosition != null) {
            autoHoldPosition.SetItem (item);
        }
    }

    public void DropItem (Item item) {
        if (item != null) {
            GameObject.Instantiate (item.prop, transform.position, transform.rotation);
            RemoveItem(item);
        }
    }

    public void DropItem (string name) {
        Item item = FindItemInInventory (name);
        DropItem (item);
    }

    public void DropItemByPosition (string name) {
        ItemPosition position = FindPositionByName (name);
        Item item = position.currentItem ? position.currentItem : null;
        DropItem (item);
    }

    public void RemoveItem (string name) {
        Item item = FindItemInInventory (name);
        RemoveItem(item);
    }

    public void RemoveItem (Item item) {
        inventory.Remove (item);
        RemoveItemFromPosition (item);
    }

    Item FindItemInInventory (string name) {
        foreach (Item item in inventory) {
            if (item.name == name) {
                return item;
            }
        }
        return null;
    }

    Item FindItemInInventory (Item itemToFind) {
        foreach (Item item in inventory) {
            if (item == itemToFind) {
                return item;
            }
        }
        return null;
    }

    ItemPosition FindPositionByName (string name) {
        foreach (ItemPosition position in itemPositions) {
            if (position.name == name) {
                return position;
            }
        }
        return null;
    }

    bool RemoveItemFromPosition (Item item) {
        foreach (ItemPosition position in itemPositions) {
            if (position.currentItem == item) {
                GameObject.Destroy (position.currentItemProp);
                position.currentItem = null;
                return true;
            }
        }
        return false;
    }

}