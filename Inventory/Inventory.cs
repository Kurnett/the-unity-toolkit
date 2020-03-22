using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

  protected List<Item> inventory = new List<Item>();

  virtual public Item GetItem(string name) {
    return FindItemInInventory(name);
  }

  virtual public Item GetItem(Item item) {
    return FindItemInInventory(item);
  }

  virtual public void AddItem(Item item) {
    AddToInventory(item);
  }

  virtual public void AddItem(Item item, GameObject prop) {
    AddToInventory(item);
    Destroy(prop);
  }

  virtual protected void AddToInventory(Item item) {
    inventory.Add(item);
  }

  virtual public void RemoveItem(string name) {
    Item item = FindItemInInventory(name);
    RemoveItem(item);
  }

  virtual public void RemoveItem(Item item) {
    inventory.Remove(item);
  }

  virtual protected Item FindItemInInventory(string name) {
    foreach (Item item in inventory) {
      if (item.name == name) {
        return item;
      }
    }
    return null;
  }

  virtual protected Item FindItemInInventory(Item itemToFind) {
    foreach (Item item in inventory) {
      if (item == itemToFind) {
        return item;
      }
    }
    return null;
  }

}