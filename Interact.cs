using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Interact : MonoBehaviour {

  void Update() { InteractInput(); }

  abstract public void InteractInput();

  abstract public void TriggerInteract ();

  abstract public void UpdateInteractionUI ();
  
}