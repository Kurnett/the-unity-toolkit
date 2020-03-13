using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Modify InteractZone to inherit from Interact.

public class InteractionZone : Interaction {

    // TODO: Allow custom messages for interaction zones in UI.

    public GameObject target;

    override public void Interact () {
        target.SendMessage("Interact", null, SendMessageOptions.DontRequireReceiver);
    }

    void OnTriggerEnter (Collider other) {
        other.SendMessage("EnterZone", gameObject);
    }

    void OnTriggerExit (Collider other) {
        other.SendMessage("LeaveZone", gameObject);
    }
}