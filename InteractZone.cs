using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractZone : Interact {

  public List<GameObject> currentZones = new List<GameObject>();

  override public void InteractInput() {
    if (Input.GetButtonDown("Interact")) {
      TriggerInteract();
    }
  }

  override public void TriggerInteract() {
    foreach (GameObject zone in currentZones) {
      if (zone) {
        zone.SendMessage("Interact", null, SendMessageOptions.DontRequireReceiver);
      }
    }
  }

  public void EnterZone(GameObject zone) {
    currentZones.Add(zone);
  }

  public void LeaveZone(GameObject zone) {
    currentZones.Remove(zone);
  }
}