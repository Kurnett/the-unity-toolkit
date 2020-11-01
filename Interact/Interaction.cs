using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

abstract public class Interaction : MonoBehaviour {

  public UnityEvent action;
  virtual public void Interact() {
    action.Invoke();
  }

}