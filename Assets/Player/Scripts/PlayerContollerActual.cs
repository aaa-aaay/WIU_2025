using UnityEngine;
using System.Collections;
using Cinemachine;

public class PlayerControllerActual : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTarget;
    public Camera playerCamera;
    private Animator _animator;
    private CharacterController _cc;
    public Camera firstPersonCamera; // Your existing first-person camera
    public CinemachineFreeLook thirdPersonCamera; // Your Cinemachine Virtual Camera

    [Header("Camera Toggle")]
    public KeyCode toggleCameraKey = KeyCode.C; // Key to toggle between cameras

    [Header("Movement Settings")]
    public float walkSpeed = 3.0f;
    public float runSpeed = 6.0f;

    [Header("Dodge Settings")]
    public float dodgeDistance = 3.0f;
    public float dodgeSpeed = 10.0f;
    public float dodgeCooldown = 1.0f;
    private bool isDodging = false;
    private float lastDodgeTime = -1f;

    [Header("Roll Settings")]
    public float rollDistance = 3.0f;
    public float rollSpeed = 10.0f;
    public float rollCooldown = 1.0f;
    public float rollCameraHeight = -0.8f;
    public float playerTurnSpeed = 5f;

    [Header("Camera Settings")]
    public float cameraSensitivity = 100f;
    public float cameraRollEffectIntensity = 0.5f;
    

    private float xRotation = 0f;
    private float yRotation = 0f;

    [Header("Bobbing Settings")]
    public float bobbingSpeed = 0.18f;
    public float bobbingAmount = 0.2f;
    private float defaultYPos = 0;
    private float timer = 0;

    [Header("Gravity Settings")]
    public float gravity = -9.81f;
    private float verticalVelocity = 0f; 


    private void Start()
    {
        _animator = GetComponent<Animator>();
        _cc = GetComponent<CharacterController>();
        _animator.applyRootMotion = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        defaultYPos = cameraTarget.localPosition.y;
        // Start with first-person camera active
        firstPersonCamera.enabled = true;
        thirdPersonCamera.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleCameraKey))
        {
            ToggleCamera();
        }

        HandleMovement();
        HandleDodge();
        HandleRoll();

        if (firstPersonCamera.enabled)
        {
            HandleCameraRotation();
        }
    }

    private void HandleCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;

        // Rotate camera only; do not affect player rotation in third person
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        yRotation += mouseX;

        if (firstPersonCamera.enabled)
        {
            // First-person camera active: Rotate both the camera and the player model
            cameraTarget.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
            Quaternion targetRotation = Quaternion.Euler(0, yRotation, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * playerTurnSpeed);
        }
        else
        {
            // Third-person mode: Camera rotates independently
            // Ensure Cinemachine's camera is correctly setup to rotate around the player
        }
    }

    private void ToggleCamera()
    {
        bool isThirdPerson = thirdPersonCamera.gameObject.activeSelf;

        // Toggle the state of the cameras
        firstPersonCamera.enabled = isThirdPerson;
        thirdPersonCamera.gameObject.SetActive(!isThirdPerson);
    }

    private void HandleMovement()
    {
        if (isDodging) return;

        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isMoving = input.sqrMagnitude > 0.01f;
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // Apply gravity
        if (_cc.isGrounded)
        {
            verticalVelocity = -0.5f; 
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime; 
        }

        if (isMoving)
        {
            // Get the camera's forward and right vectors
            Vector3 forward = Camera.main.transform.forward;
            Vector3 right = Camera.main.transform.right;
            forward.y = 0; // Ignore the y component for flat movement
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            // Calculate direction based on input relative to the camera
            Vector3 direction = (forward * input.y + right * input.x).normalized;

            // Rotate the character to face the direction of movement
            if (!firstPersonCamera.enabled) 
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * playerTurnSpeed);
            }

            // Move the character (including vertical velocity for gravity)
            _cc.Move((direction * currentSpeed + Vector3.up * verticalVelocity) * Time.deltaTime);

            // Handle bobbing effect
            float dynamicBobbingSpeed = isRunning ? bobbingSpeed * 1.5f : bobbingSpeed;
            float dynamicBobbingAmount = isRunning ? bobbingAmount * 1.2f : bobbingAmount * 0.8f;
            timer += Time.deltaTime * dynamicBobbingSpeed;
            cameraTarget.localPosition = new Vector3(cameraTarget.localPosition.x, defaultYPos + Mathf.Sin(timer) * dynamicBobbingAmount, cameraTarget.localPosition.z);
        }
        else
        {
            // Apply gravity even when not moving
            _cc.Move(Vector3.up * verticalVelocity * Time.deltaTime);

            timer = 0;
            cameraTarget.localPosition = new Vector3(cameraTarget.localPosition.x, Mathf.Lerp(cameraTarget.localPosition.y, defaultYPos, Time.deltaTime * playerTurnSpeed), cameraTarget.localPosition.z);
        }
        _animator.SetFloat("Speed", isMoving ? (isRunning ? 1f : 0.5f) : 0);
    }



    private void HandleBobbing(bool isRunning, Vector3 direction)
    {
        if (direction.magnitude > 0)  // Only bob if actually moving
        {
            float dynamicBobbingSpeed = isRunning ? bobbingSpeed * 1.5f : bobbingSpeed;
            float dynamicBobbingAmount = isRunning ? bobbingAmount * 1.2f : bobbingAmount * 0.8f;
            timer += Time.deltaTime * dynamicBobbingSpeed;
            cameraTarget.localPosition = new Vector3(cameraTarget.localPosition.x, defaultYPos + Mathf.Sin(timer) * dynamicBobbingAmount, cameraTarget.localPosition.z);
        }
        else
        {
            timer = 0;  // Reset bobbing timer if not moving
            cameraTarget.localPosition = new Vector3(cameraTarget.localPosition.x, Mathf.Lerp(cameraTarget.localPosition.y, defaultYPos, Time.deltaTime * playerTurnSpeed), cameraTarget.localPosition.z);
        }
    }


    private void HandleDodge()
    {
        if (Time.time < lastDodgeTime + dodgeCooldown || isDodging) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 dodgeDir = Vector3.zero;
            int dodgeDirectionValue = -1;

            if (Input.GetKey(KeyCode.W)) { dodgeDir = transform.forward; dodgeDirectionValue = 0; }
            if (Input.GetKey(KeyCode.S)) { dodgeDir = -transform.forward; dodgeDirectionValue = 1; }
            if (Input.GetKey(KeyCode.A)) { dodgeDir = -transform.right; dodgeDirectionValue = 2; }
            if (Input.GetKey(KeyCode.D)) { dodgeDir = transform.right; dodgeDirectionValue = 3; }

            if (dodgeDirectionValue != -1)
            {
                isDodging = true;
                _animator.SetInteger("dodgeDirection", dodgeDirectionValue);
                _animator.SetBool("dodge", true);
                StartCoroutine(DodgeMovement(dodgeDir));
                lastDodgeTime = Time.time;
            }
        }
    }

    private IEnumerator DodgeMovement(Vector3 dodgeDir)
    {
        //dodge
        float dodgeDuration = 0.3f;
        float elapsedTime = 0f;
        float speed = dodgeDistance / dodgeDuration;

        //camera
        Vector3 originalCameraPos = playerCamera.transform.localPosition;
        Quaternion originalCameraRot = playerCamera.transform.localRotation;
        Vector3 dodgeCameraPos = new Vector3(originalCameraPos.x, originalCameraPos.y - 0.5f, originalCameraPos.z);
        Quaternion dodgeCameraRot = Quaternion.Euler(20, originalCameraRot.eulerAngles.y, originalCameraRot.eulerAngles.z);

        //hiehgt center collider
        float originalHeight = _cc.height;
        Vector3 originalCenter = _cc.center;
        float reducedHeight = originalHeight * 0.5f;

        Vector3 reducedCenter = new Vector3(originalCenter.x, originalCenter.y - originalHeight * 0.25f, originalCenter.z);
        _cc.height = reducedHeight;
        _cc.center = reducedCenter;


        while (elapsedTime < dodgeDuration)
        {
            _cc.Move(dodgeDir * speed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            playerCamera.transform.localPosition = Vector3.Lerp(originalCameraPos, dodgeCameraPos, elapsedTime / dodgeDuration);
            playerCamera.transform.localRotation = Quaternion.Lerp(originalCameraRot, dodgeCameraRot, elapsedTime / dodgeDuration);
            yield return null;
        }

        _cc.height = originalHeight;
        _cc.center = originalCenter;

        float resetTime = 0.4f;
        elapsedTime = 0f;

        while (elapsedTime < resetTime)
        {
            playerCamera.transform.localPosition = Vector3.Lerp(dodgeCameraPos, originalCameraPos, elapsedTime / resetTime);
            playerCamera.transform.localRotation = Quaternion.Lerp(dodgeCameraRot, originalCameraRot, elapsedTime / resetTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isDodging = false;
        _animator.SetBool("dodge", false);
    }

    private void HandleRoll()
    {
        if (Time.time < lastDodgeTime + rollCooldown || isDodging) return;

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Vector3 rollDir = Vector3.zero;
            bool isRolling = false;

            if (Input.GetKey(KeyCode.W)) { rollDir += transform.forward; isRolling = true; }
            if (Input.GetKey(KeyCode.S)) { rollDir += -transform.forward; isRolling = true; }
            if (Input.GetKey(KeyCode.A)) { rollDir += -transform.right; isRolling = true; }
            if (Input.GetKey(KeyCode.D)) { rollDir += transform.right; isRolling = true; }

            if (isRolling)
            {
                isDodging = true;
                _animator.SetBool("roll", true);
                StartCoroutine(RollMovement(rollDir.normalized));
                lastDodgeTime = Time.time;
            }
        }
    }

    private IEnumerator RollMovement(Vector3 rollDir)
    {
        float rollDuration = 0.5f;
        float elapsedTime = 0f;
        float speed = rollDistance / rollDuration;

        //camera 
        Vector3 originalCameraPos = playerCamera.transform.localPosition;
        Quaternion originalCameraRot = playerCamera.transform.localRotation;
        Vector3 rollCameraPos = new Vector3(originalCameraPos.x, originalCameraPos.y - 1.2f, originalCameraPos.z);
        Quaternion rollCameraRot = Quaternion.Euler(25, originalCameraRot.eulerAngles.y, originalCameraRot.eulerAngles.z);

        //collider
        float originalHeight = _cc.height;
        Vector3 originalCenter = _cc.center;
        float reducedHeight = originalHeight * 0.5f;
        Vector3 reducedCenter = new Vector3(originalCenter.x, originalCenter.y - originalHeight * 0.25f, originalCenter.z);

        _cc.height = reducedHeight; 
        _cc.center = reducedCenter;

        while (elapsedTime < rollDuration)
        {
            _cc.Move(rollDir * speed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            playerCamera.transform.localPosition = Vector3.Lerp(originalCameraPos, rollCameraPos, elapsedTime / rollDuration);
            playerCamera.transform.localRotation = Quaternion.Lerp(originalCameraRot, rollCameraRot, elapsedTime / rollDuration);
            yield return null;
        }

        _cc.height = originalHeight;
        _cc.center = originalCenter;

        elapsedTime = 0f;
        while (elapsedTime < 0.3f)
        {
            playerCamera.transform.localPosition = Vector3.Lerp(rollCameraPos, originalCameraPos, elapsedTime / 0.2f);
            playerCamera.transform.localRotation = Quaternion.Lerp(rollCameraRot, originalCameraRot, elapsedTime / 0.2f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isDodging = false;
        _animator.SetBool("roll", false);
    }
}