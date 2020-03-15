using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionZone : Interaction {

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