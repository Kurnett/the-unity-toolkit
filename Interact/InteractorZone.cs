using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractorZone : Interactor {

  List<GameObject> currentZones = new List<GameObject>();

  override public void InteractInput() {
    if (Input.GetButtonDown("Interact")) {
      TriggerInteract();
    }
  }

  override public void TriggerInteract() {
    foreach (GameObject zone in currentZones) {
      if (zone) {
        Interaction intZone = zone.GetComponent<Interaction>();
        if (intZone) {
          intZone.Interact();
        }
      }
    }
  }

  public List<GameObject> GetZones() {
    return currentZones;
  }

  public void EnterZone(GameObject zone) {
    currentZones.Add(zone);
    UpdateInteractionUI();
  }

  public void LeaveZone(GameObject zone) {
    currentZones.Remove(zone);
    UpdateInteractionUI();
  }
}