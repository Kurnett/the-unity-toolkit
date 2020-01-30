using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractZone : MonoBehaviour {

    public GameObject target;

    public void Interact () {
        target.SendMessage("Interact", null, SendMessageOptions.DontRequireReceiver);
    }

    void OnTriggerEnter (Collider other) {
        other.SendMessage("EnterZone", gameObject);
    }

    void OnTriggerExit (Collider other) {
        other.SendMessage("LeaveZone", gameObject);
    }
}