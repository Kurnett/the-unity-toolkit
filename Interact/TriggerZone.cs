using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerZone : MonoBehaviour {
  [SerializeField]
  bool single = false;
  bool fired = false;
  [SerializeField]
  UnityEvent entry;

  void OnTriggerEnter(Collider other) {
    if (!single || !fired) {
      entry.Invoke();
      fired = true;
    }
  }
}
