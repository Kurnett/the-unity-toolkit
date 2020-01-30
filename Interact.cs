using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour {

    public bool zoneInteract = false;

    CameraController cameraController;
    public Transform cameraTransform;

    public LayerMask mask;

    public float cameraDistance = 0f;
    public float range = 2f;

    public List<GameObject> currentZones = new List<GameObject> ();

    public GameObject interactCanvas;
    private GameObject currentCanvas;

    void Start () {
        if (gameObject.GetComponent<CameraController> ()) {
            cameraController = gameObject.GetComponent<CameraController> ();
        }
    }

    void Update () {
        InteractInput ();
    }

    void InteractInput () {
        if (Input.GetButtonDown ("Interact")) {
            if (zoneInteract) {
                InteractZone ();
            } else {
                InteractFPS ();
            }
        }
    }

    void InteractZone () {
        foreach (GameObject zone in currentZones) {
            if (zone) {
                zone.SendMessage ("Interact", null, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    void InteractFPS () {
        RaycastHit hit;
        if (Physics.Raycast (cameraTransform.position, cameraTransform.TransformDirection (Vector3.forward), out hit, GetCameraDistance () + range, mask)) {
            hit.collider.gameObject.SendMessage ("Interact", null, SendMessageOptions.DontRequireReceiver);
        }
    }

    float GetCameraDistance () {
        if (cameraController) {
            return cameraController.GetCameraDistance ();
        }
        return cameraDistance;
    }

    void CheckUI () {
        if (currentCanvas) {
            
        } 
    }

    public void EnterZone (GameObject zone) {
        currentZones.Add(zone);
    }

    public void LeaveZone (GameObject zone) {
        currentZones.Remove(zone);
    }
}