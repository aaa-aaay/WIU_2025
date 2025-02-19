using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTarget;
    public Camera playerCamera;
    private Animator _animator;
    private CharacterController _cc;
    private PlayerStats _playerStats; // Reference to PlayerStats

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

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _cc = GetComponent<CharacterController>();
        _playerStats = GetComponent<PlayerStats>(); // Initialize PlayerStats reference
        _animator.applyRootMotion = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        defaultYPos = cameraTarget.localPosition.y;
    }

    private void Update()
    {
        HandleCameraRotation();
        HandleMovement();
        HandleDodge();
        HandleRoll();
        HandleHealthAndAbilities(); // Handle health and abilities (e.g., healing, barrier)
    }

    private void HandleCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        yRotation += mouseX;
        cameraTarget.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        Quaternion targetRotation = Quaternion.Euler(0, yRotation, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * playerTurnSpeed);
    }

    private void HandleMovement()
    {
        if (isDodging) return;

        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isMoving = input.sqrMagnitude > 0.01f;
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // Adjust speed based on PlayerStats
        currentSpeed *= _playerStats.Speed / 100f; // 

        if (isMoving)
        {
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;
            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            Vector3 direction = (forward * input.y + right * input.x).normalized;
            _cc.Move(direction * currentSpeed * Time.deltaTime);
            float dynamicBobbingSpeed = isRunning ? bobbingSpeed * 1.5f : bobbingSpeed;
            float dynamicBobbingAmount = isRunning ? bobbingAmount * 1.2f : bobbingAmount * 0.8f;
            timer += Time.deltaTime * dynamicBobbingSpeed;
            cameraTarget.localPosition = new Vector3(cameraTarget.localPosition.x, defaultYPos + Mathf.Sin(timer) * dynamicBobbingAmount, cameraTarget.localPosition.z);
        }
        else
        {
            timer = 0;
            cameraTarget.localPosition = new Vector3(cameraTarget.localPosition.x, Mathf.Lerp(cameraTarget.localPosition.y, defaultYPos, Time.deltaTime * playerTurnSpeed), cameraTarget.localPosition.z);
        }

        _animator.SetFloat("Speed", isMoving ? (isRunning ? 1f : 0.5f) : 0);
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
        float dodgeDuration = 0.3f;
        float elapsedTime = 0f;
        float speed = dodgeDistance / dodgeDuration;
        Vector3 originalCameraPos = playerCamera.transform.localPosition;
        Quaternion originalCameraRot = playerCamera.transform.localRotation;
        Vector3 dodgeCameraPos = new Vector3(originalCameraPos.x, originalCameraPos.y - 0.5f, originalCameraPos.z);
        Quaternion dodgeCameraRot = Quaternion.Euler(20, originalCameraRot.eulerAngles.y, originalCameraRot.eulerAngles.z);
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
        Vector3 originalCameraPos = playerCamera.transform.localPosition;
        Quaternion originalCameraRot = playerCamera.transform.localRotation;
        Vector3 rollCameraPos = new Vector3(originalCameraPos.x, originalCameraPos.y - 1.2f, originalCameraPos.z);
        Quaternion rollCameraRot = Quaternion.Euler(25, originalCameraRot.eulerAngles.y, originalCameraRot.eulerAngles.z);
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

    private void HandleHealthAndAbilities()
    {
        // Example: Heal when pressing H
        if (Input.GetKeyDown(KeyCode.H) && _playerStats.UnlockedHeal)
        {
            _playerStats.Heal();
        }

        // Example: Activate barrier when pressing B
        if (Input.GetKeyDown(KeyCode.B) && _playerStats.UnlockedBarrier)
        {
            _playerStats.Barrier();
        }

        // Example: Take damage when pressing T (for testing)
        if (Input.GetKeyDown(KeyCode.T))
        {
            _playerStats.TakeDamage(10);
        }
    }
}