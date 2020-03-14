using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Modify InteractZone to inherit from Interact.

public class InteractionZone : Interaction {

  // TODO: Allow custom messages for interaction zones in UI.

  public Interaction target;

  override public void Interact() {
    target.Interact();
  }

  void OnTriggerEnter(Collider other) {
    InteractZone target = other.GetComponent<InteractZone>();
    if (target != null) {
      target.EnterZone(gameObject);
    }
  }

  void OnTriggerExit(Collider other) {
    InteractZone target = other.GetComponent<InteractZone>();
    if (target != null) {
      target.LeaveZone(gameObject);
    }
  }
}