using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

  public Transform cameraObject;
  public Transform cameraPivot;
  public Transform cameraSecondaryPivot;

  public bool allowZoom = true;

  public enum CameraMode { FirstPerson, ThirdPerson, ThirdPersonShoulder };

  [Header("Camera Modes")]
  public CameraMode cameraMode = CameraMode.ThirdPerson;
  public bool canUseFirstPerson = true;
  public bool canUseThirdPerson = true;
  public bool canUseThirdPersonShoulder = true;

  public GameObject thirdPersonViewModel;
  public GameObject firstPersonViewModel;

  [Header("Third-Person")]
  float cameraDistance;
  float targetCameraDistance;
  public float defaultCameraDistance = 3f;
  public Vector2 thirdPersonRange = new Vector2(2f, 5f);
  public float cameraSpeed = 20f;

  [Header("Third-Person Over-the-Shoulder")]
  public float overShoulderOffset = 1f;

  float pitch;
  float targetPitch;
  float yaw;
  float targetYaw;

  [Header("Pitch and Yaw")]
  public float defaultTargetPitch = 0f;
  public float defaultTargetYaw = 0f;

  [Header("Secondary Pivot Point")]
  public Vector3 secondaryTargetPosition = new Vector3(0, 0, 0);
  public Quaternion secondaryTargetRotation = Quaternion.identity;
  public float cameraSecondarySpeed = 5f;

  void Start() {
    cameraDistance = defaultCameraDistance;
    targetCameraDistance = defaultCameraDistance;
    pitch = defaultTargetPitch;
    targetPitch = defaultTargetPitch;
    yaw = defaultTargetYaw;
    targetYaw = defaultTargetYaw;

    SetCameraModeSettings(cameraMode);
  }

  void Update() {
    cameraMode = CameraModeInput(cameraMode);

    if (allowZoom) {
      targetCameraDistance += CameraDistanceChange();
      targetCameraDistance = LimitCameraRange(targetCameraDistance, thirdPersonRange.x, thirdPersonRange.y);
    }
    cameraDistance = Mathf.Lerp(cameraDistance, cameraMode == CameraMode.FirstPerson ? 0f : targetCameraDistance, 15f * Time.deltaTime);
    cameraDistance = PreventClipping(cameraPivot, cameraDistance);

    yaw = Mathf.Lerp(yaw, targetYaw, 15f * Time.deltaTime);
    pitch = Mathf.Lerp(pitch, targetPitch, 15f * Time.deltaTime);

    Vector3 newPosition = cameraObject.localPosition;
    newPosition.x = Mathf.Lerp(newPosition.x, cameraMode == CameraMode.ThirdPersonShoulder ? overShoulderOffset : 0f, 15f * Time.deltaTime);
    newPosition.z = -cameraDistance;
    cameraObject.localPosition = newPosition;

    Vector3 newRotation = cameraObject.localRotation.eulerAngles;
    newRotation.y = yaw;
    newRotation.x = pitch;
    cameraObject.localRotation = Quaternion.Euler(newRotation);

    ApproachSecondaryPivot(secondaryTargetPosition, secondaryTargetRotation);
  }

  float CameraDistanceChange() {
    float delta = Input.GetAxis("Scroll");
    return -delta * cameraSpeed * Time.deltaTime;
  }

  float LimitCameraRange(float distance, float min, float max) {
    return Mathf.Clamp(distance, min, max);
  }

  float PreventClipping(Transform origin, float distance) {
    RaycastHit hit;
    if (Physics.Raycast(origin.position, origin.TransformDirection(Vector3.back), out hit, distance)) {
      return hit.distance - 0.1f;
    }
    return distance;
  }

  CameraMode CameraModeInput(CameraMode mode) {
    if (Input.GetButtonDown("ThirdPerson") && canUseThirdPerson) { return SetCameraModeSettings(CameraMode.ThirdPerson); }
    if (Input.GetButtonDown("ThirdPersonShoulder") && canUseThirdPersonShoulder) { return SetCameraModeSettings(CameraMode.ThirdPersonShoulder); }
    if (Input.GetButtonDown("FirstPerson") && canUseFirstPerson) { return SetCameraModeSettings(CameraMode.FirstPerson); }
    return mode;
  }

  public void SetCameraMode(CameraMode mode) {
    if (mode == CameraMode.ThirdPerson && canUseThirdPerson) { cameraMode = mode; }
    if (mode == CameraMode.ThirdPersonShoulder && canUseThirdPersonShoulder) { cameraMode = mode; }
    if (mode == CameraMode.FirstPerson && canUseFirstPerson) { cameraMode = mode; }
  }

  CameraMode SetCameraModeSettings(CameraMode mode) {
    if (mode == CameraMode.ThirdPerson || mode == CameraMode.ThirdPersonShoulder) {
      if (firstPersonViewModel) { firstPersonViewModel.SetActive(false); }
      if (thirdPersonViewModel) { thirdPersonViewModel.SetActive(true); }
    } else {
      if (firstPersonViewModel) { firstPersonViewModel.SetActive(true); }
      if (thirdPersonViewModel) { thirdPersonViewModel.SetActive(false); }
    }
    return mode;
  }

  void ApproachSecondaryPivot(Vector3 position, Quaternion rotation) {
    cameraSecondaryPivot.localPosition = Vector3.Lerp(cameraSecondaryPivot.localPosition, position, cameraSecondarySpeed * Time.deltaTime);
    cameraSecondaryPivot.localRotation = Quaternion.Lerp(cameraSecondaryPivot.localRotation, rotation, cameraSecondarySpeed * Time.deltaTime);
  }

  public float GetCameraDistance() {
    return cameraDistance;
  }

  public void SetCameraDistance(float newDistance) {
    targetCameraDistance = newDistance;
  }

  public void SetSecondaryPivot(Vector3 position, Quaternion rotation) {
    secondaryTargetPosition = position;
    secondaryTargetRotation = rotation;
  }
}