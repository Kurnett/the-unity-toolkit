using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManagerMovement : MonoBehaviour {

    Transform player;
    Movement movement;
    CameraController cameraController;

    Transform npc;

    Vector3 originalCameraPosition;
    Vector3 originalCameraRotation;
    public Vector3 dialogueCameraPosition;
    public Vector3 dialogueCameraRotation;

    void Start () {
        GameObject playerObject = GameObject.FindWithTag("Player");
        player = playerObject.transform;
        movement = playerObject.GetComponent<Movement>();
        cameraController = playerObject.GetComponent<CameraController>();
    }

    public void StartDialogue (GameObject npcObject) {
        movement.allowMovement = false;
        movement.allowRotation = false;
        cameraController.allowZoom = false;

        npc = npcObject.transform;
        Quaternion direction = Quaternion.LookRotation(npc.position - player.position, Vector3.up);
        Vector3 targetDirection = new Vector2(direction.eulerAngles.y,0);
        movement.SetTargetRotation(targetDirection);

        originalCameraPosition = cameraController.cameraSecondaryPivot.localPosition;
        originalCameraRotation = cameraController.cameraSecondaryPivot.localRotation.eulerAngles;
        cameraController.SetSecondaryPivot(dialogueCameraPosition, Quaternion.Euler(dialogueCameraRotation));
    }

    public void EndDialogue () {
        movement.allowMovement = true;
        movement.allowRotation = true;
        cameraController.allowZoom = true;

        cameraController.SetSecondaryPivot(originalCameraPosition, Quaternion.Euler(originalCameraRotation));
    }

}