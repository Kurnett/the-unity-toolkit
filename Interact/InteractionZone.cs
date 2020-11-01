using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionZone : Interaction {
  void OnTriggerEnter(Collider other) {
    InteractorZone target = other.GetComponent<InteractorZone>();
    if (target != null) {
      target.EnterZone(gameObject);
    }
  }

  void OnTriggerExit(Collider other) {
    InteractorZone target = other.GetComponent<InteractorZone>();
    if (target != null) {
      target.LeaveZone(gameObject);
    }
  }
}