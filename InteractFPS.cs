// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// // TODO: Migrate interactions to use inheritance.
// // TODO: Allow zone interaction and FPS interaction simultaneously.
// // TODO: Ensure zone interactions only interact with the closest target.
// // TODO: Add UI handling to only show the closest zone target.

// public class InteractFPS : Interact {

//   public bool zoneInteract = false;

//   public List<GameObject> currentZones = new List<GameObject>();

//   // TODO: Migrate interaction UI to use inheritance.

//   public GameObject interactCanvas;
//   private GameObject currentCanvas;

//   // TODO: Move camera interactions to inherited class.
//   void Start() {
//     if (gameObject.GetComponent<CameraController>()) {
//       cameraController = gameObject.GetComponent<CameraController>();
//     }
//   }

//   void Update() {
//     InteractInput();
//   }

//   // TODO: Move interaction triggers (zones and FPS) to inherited classes. Interact class should primarily be boilerplate to support action-and-reaction.
//   virtual void InteractInput() {
//     if (Input.GetButtonDown("Interact")) {
      
//     }
//   }

//   virtual public void Interact () {
//     if (zoneInteract) {
//       InteractZone();
//     } else {
//       InteractFPS();
//     }
//   }

//   void InteractZone() {
//     foreach (GameObject zone in currentZones) {
//       if (zone) {
//         zone.SendMessage("Interact", null, SendMessageOptions.DontRequireReceiver);
//       }
//     }
//   }

//   void InteractFPS() {
//     RaycastHit hit;
//     if (Physics.Raycast(cameraTransform.position, cameraTransform.TransformDirection(Vector3.forward), out hit, GetCameraDistance() + range, mask)) {
//       hit.collider.gameObject.SendMessage("Interact", null, SendMessageOptions.DontRequireReceiver);
//     }
//   }

//   // TODO: Move camera interactions to inherited class.
//   float GetCameraDistance() {
//     if (cameraController) {
//       return cameraController.GetCameraDistance();
//     }
//     return cameraDistance;
//   }

//   // TODO: Move UI interactions to inherited class.
//   void CheckUI() {
//     if (currentCanvas) {

//     }
//   }

//   // TODO: Move zone interactions to inherited class.
//   public void EnterZone(GameObject zone) {
//     currentZones.Add(zone);
//   }

//   public void LeaveZone(GameObject zone) {
//     currentZones.Remove(zone);
//   }
// }