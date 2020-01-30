using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    CharacterController playerController;
    public Transform cameraPivot;
    public Transform playerCamera;

    Vector3 velocity;
    Vector3 involuntaryMovement;

    [Header("Rotation")]
    public float mouseSensitivity = 30f;
    public float maximumUpwardAngle = 90f;
    public float maximumDownwardAngle = 90f;
    
    [Header("Acceleration")]
    public float acceleration = 10f;
    public float airAcceleration = 3f;

    [Header("Maximum Speed")]
    public float maxSpeedForward = 5f;
    public float maxSpeedBackward = 3f;
    public float maxSpeedSide = 4f;
    
    [Header("Jumping")]
    public float jumpStrength = 5f;
    float terminalVelocity = 52f;
    float gravity = 9.8f;

    Vector2 rotation;
    Vector2 targetRotation;

    bool grounded;
    Vector3 lastPosition;
    CollisionFlags lastCollisionFlags;

    bool sprinting;

    [Header("Sprinting")]
    public float sprintAcceleration = 30f;

    public float maxSprintSpeedForward = 8f;
    public float maxSprintSpeedBackward = 3f;
    public float maxSprintSpeedSide = 5f;

    [Header("Stamina")]
    public float stamina;
    float maxStamina = 100f;
    public float staminaDrainRate = 10f;
    public float staminaRegenRate = 10f;
    public float sprintThreshold = 30f;

    bool crouching;
    bool togglingCrouch;
    float height = 1.8f;
    float crouchHeight = .8f;

    [Header("Crouching")]
    public float maxCrouchSpeedForward = 3f;
    public float maxCrouchSpeedBackward = 1f;
    public float maxCrouchSpeedSide = 2f;

    public bool allowMovement = true;
    public bool allowRotation = true;

    void Start () {
        playerController = GetComponent<CharacterController> ();
        targetRotation.x = transform.rotation.eulerAngles.y;
        targetRotation.y = playerCamera.rotation.eulerAngles.x;
        rotation = targetRotation;
        stamina = maxStamina;
    }

    void Update () {
        if (allowRotation) {
            targetRotation = TurnInput (targetRotation);
        }
        if (allowMovement) {
            Move ();
            Crouch ();
            Sprint ();
        }
        velocity = Turn (velocity);
    }

    void Move () {
        velocity = Acceleration (velocity);
        velocity = Jump (velocity);

        Vector3 frameMovement = velocity * Time.deltaTime;
        frameMovement = StickToGround (frameMovement);
        lastPosition = transform.position;

        lastCollisionFlags = playerController.Move (frameMovement);

        Vector3 newVelocity = (transform.position - lastPosition) / Time.deltaTime;
        velocity.x = newVelocity.x;
        velocity.y = (newVelocity.y <= 0f && newVelocity.y > -10f) ? newVelocity.y : velocity.y;
        velocity.z = newVelocity.z;

        grounded = IsGrounded ();
    }

    Vector3 Acceleration (Vector3 worldVelocity) {
        Vector3 localVelocity = transform.InverseTransformDirection (worldVelocity);
        float xInput = Input.GetAxis ("Horizontal");
        float zInput = Input.GetAxis ("Vertical");
        float xAcceleration = (grounded ? (sprinting ? sprintAcceleration : acceleration) : airAcceleration) * xInput * Time.deltaTime;
        float zAcceleration = (grounded ? (sprinting ? sprintAcceleration : acceleration) : airAcceleration) * zInput * Time.deltaTime;

        float currentMaxSpeedForward = (crouching ? maxCrouchSpeedForward : (sprinting ? maxSprintSpeedForward : maxSpeedForward));
        float currentMaxSpeedBackward = (crouching ? maxCrouchSpeedBackward : (sprinting ? maxSprintSpeedBackward : maxSpeedBackward));
        float currentMaxSpeedSide = (crouching ? maxCrouchSpeedSide : (sprinting ? maxSprintSpeedSide : maxSpeedSide));

        if (xInput > 0f && localVelocity.x < currentMaxSpeedSide) {
            localVelocity.x = Mathf.MoveTowards (localVelocity.x, currentMaxSpeedSide, xAcceleration * (localVelocity.x < 0 && grounded ? 2 : 1));
        } else if (xInput < 0f && localVelocity.x > -currentMaxSpeedSide) {
            localVelocity.x = Mathf.MoveTowards (localVelocity.x, -currentMaxSpeedSide, -xAcceleration * (localVelocity.x > 0 && grounded ? 2 : 1));
        } else if (grounded) {
            localVelocity.x = Mathf.MoveTowards (localVelocity.x, 0f, acceleration * Time.deltaTime);
        }
        if (zInput > 0f && localVelocity.z < currentMaxSpeedForward) {
            localVelocity.z = Mathf.MoveTowards (localVelocity.z, currentMaxSpeedForward, zAcceleration * (localVelocity.z < 0 && grounded ? 2 : 1));
        } else if (zInput < 0f && localVelocity.z > -currentMaxSpeedBackward) {
            localVelocity.z = Mathf.MoveTowards (localVelocity.z, -currentMaxSpeedBackward, -zAcceleration * (localVelocity.z > 0 && grounded ? 2 : 1));
        } else if (grounded) {
            localVelocity.z = Mathf.MoveTowards (localVelocity.z, 0f, acceleration * Time.deltaTime);
        }

        if (!grounded) {
            localVelocity.y = Mathf.MoveTowards (localVelocity.y, -terminalVelocity, gravity * Time.deltaTime);
        }

        return transform.TransformDirection (localVelocity);
    }

    Vector3 StickToGround (Vector3 frameMovement) {
        if (grounded) {
            RaycastHit pushDownRay;
            if (Physics.Raycast (transform.position, -Vector3.up, out pushDownRay, .5f)) {
                frameMovement.y -= pushDownRay.distance;
            }
        }

        return frameMovement;
    }

    Vector3 Jump (Vector3 worldVelocity) {
        if (Input.GetButtonDown ("Jump") && grounded) {
            worldVelocity.y = jumpStrength;
            grounded = false;
        }
        return worldVelocity;
    }

    bool IsGrounded () {
        if ((lastCollisionFlags & CollisionFlags.Below) != 0) {
            return true;
        }
        return false;
    }

    Vector3 Turn (Vector3 velocity) {
        Vector3 localVelocity = transform.InverseTransformDirection (velocity);
        rotation = Vector2.Lerp (rotation, targetRotation, 25f * Time.deltaTime);
        Vector3 playerRotation = new Vector3 (0f, rotation.x, 0f);
        Vector3 cameraRotation = new Vector3 (rotation.y, 0f, 0f);
        transform.rotation = Quaternion.Euler (playerRotation);
        cameraPivot.localRotation = Quaternion.Euler (cameraRotation);

        if (grounded) {
            return transform.TransformDirection (localVelocity);
        }
        return velocity;
    }

    Vector2 TurnInput (Vector2 currentRotation) {
        float deltaX = Input.GetAxis ("MouseX");
        float deltaY = Input.GetAxis ("MouseY");
        currentRotation.x += deltaX * mouseSensitivity * Time.deltaTime;
        currentRotation.y -= deltaY * mouseSensitivity * Time.deltaTime;
        currentRotation.y = Mathf.Clamp (currentRotation.y, -maximumDownwardAngle, maximumUpwardAngle);
        return currentRotation;
    }

    void Sprint () {
        if (Input.GetButtonDown ("Sprint") && stamina > sprintThreshold) {
            sprinting = true;
        } else if (Input.GetButtonUp ("Sprint") || stamina == 0) {
            sprinting = false;
        }
        if (sprinting && velocity.sqrMagnitude >.5f) {
            stamina = Mathf.MoveTowards (stamina, 0, staminaDrainRate * Time.deltaTime);
            if (crouching) {
                togglingCrouch = true;
            }
        } else if (grounded) {
            stamina = Mathf.MoveTowards (stamina, maxStamina, staminaRegenRate * Time.deltaTime);
        }
    }

    void Crouch () {
        if (Input.GetButtonDown ("Crouch")) {
            togglingCrouch = true;
        }
        if (togglingCrouch) {
            ToggleCrouch ();
        }
        AdjustCrouchHeight ();
    }

    void ToggleCrouch () {
        if (!crouching) {
            crouching = true;
            togglingCrouch = false;
        } else {
            Vector3 spherePosition = transform.position;
            spherePosition.y += playerController.radius;
            float sphereDistance = height - (playerController.radius * 2f);
            Ray sphereRay = new Ray (spherePosition, Vector3.up);
            if (!Physics.SphereCast (sphereRay, playerController.radius, sphereDistance)) {
                crouching = false;
                togglingCrouch = false;
            }
        }
    }

    void AdjustCrouchHeight () {
        if (crouching) {
            playerController.height = Mathf.MoveTowards (playerController.height, crouchHeight, 5f * Time.deltaTime);
        } else {
            playerController.height = Mathf.MoveTowards (playerController.height, height, 5f * Time.deltaTime);
        }
        Vector3 newPosition = cameraPivot.localPosition;
        playerController.center = new Vector3 (0, playerController.height / 2f, 0);
        newPosition.y = playerController.height - .15f;
        cameraPivot.localPosition = newPosition;
    }

    public bool GetSprinting () {
        return sprinting;
    }

    public bool GetCrouching () {
        return crouching;
    }

    public float GetCrouchSpeed () {
        return maxCrouchSpeedBackward;
    }

    public float GetWalkSpeed () {
        return maxSpeedBackward;
    }

    public float GetSprintSpeed () {
        return maxSprintSpeedForward * .8f;
    }

    public Vector3 GetVelocity () {
        return velocity;
    }

    public float GetStamina () {
        return stamina;
    }

    public void SetTargetRotation (Vector3 newTarget) {
        targetRotation = newTarget;
    }
}