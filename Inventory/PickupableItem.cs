using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableItem : MonoBehaviour {

    public Item item;
    public string playerTag = "Player";

    public void Interact () {
        GameObject.FindWithTag(playerTag).GetComponent<Inventory>().AddItem(item, gameObject);
    }
}