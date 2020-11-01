using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Interactor : MonoBehaviour {

  void Update() { InteractInput(); }

  abstract public void InteractInput();
  abstract public void TriggerInteract();
  virtual public void UpdateInteractionUI() { }

}