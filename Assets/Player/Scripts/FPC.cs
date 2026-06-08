using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FPC : MonoBehaviour
{
    [Header("Movement")]
    private Vector3 movement;
    private bool move = false;
    public float speed;
    public float walkSpeed;
    [Header("Sprint")]
    public float cooldownMax;
    private float cooldown;
    public float sprintSpeed;
    public float MaxStamina;
    private float Stamina;
    public Slider Sprintbar;
    [Header("Camera")]
    public float mouseSensitivity = 2f;
    public float minY = -90f;
    public float maxY = 90f;
    public Camera playerCamera;
    private float xRotation = 0f;
    private bool statusCurs;
    [Header("Jump")]
    public float forceJump;
    private bool isGrounded;
    [Header("Crouch")]
    public float crouchSpeed;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Stamina = MaxStamina;
    }
    void Update()
    {
        #region Movement
        movement = new Vector3(0, 0, 0);
        move = false;
        if (Input.GetKey(KeyCode.W))
        {
            movement.z = +1;
            move = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movement.x = -1;
            move = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movement.z = -1;
            move = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movement.x = +1;
            move = true;
        }
        transform.Translate(movement * speed * Time.deltaTime);
        #endregion
        #region Cursor
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (statusCurs)
                Cursor.lockState = CursorLockMode.Locked;
            else
                Cursor.lockState = CursorLockMode.Confined;
            statusCurs = !statusCurs;
        }
        #endregion
        #region Camera
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minY, maxY);
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
        #endregion
        #region Sprint & Crouch (исправлено)
        float targetSpeed = walkSpeed;
        bool isSprinting = move && Input.GetKey(KeyCode.LeftShift) && Stamina > 0f;
        bool isCrouching = Input.GetKey(KeyCode.LeftControl);
        if (isCrouching)
        {
            gameObject.GetComponent<CapsuleCollider>().height = 1;
            targetSpeed = crouchSpeed;
        }
        else
        {
            gameObject.GetComponent<CapsuleCollider>().height = 2;
        }
        if (isSprinting)
        {
            targetSpeed = sprintSpeed;
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, 90f, Time.deltaTime * 10f);
            Stamina -= Time.deltaTime;

            if (Stamina <= 0f)
            {
                Stamina = 0f;
                cooldown = cooldownMax;
            }
        }
        else
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, 70f, Time.deltaTime * 10f);
            if (cooldown > 0f)
            {
                cooldown -= Time.deltaTime;
                if (cooldown <= 0f)
                {
                    cooldown = 0f;
                }
            }
            else
            {
                Stamina += Time.deltaTime;
            }
        }

        speed = targetSpeed;
        Stamina = Mathf.Clamp(Stamina, 0f, MaxStamina);
        Sprintbar.value = Stamina / MaxStamina;
        #endregion
        #region Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                gameObject.GetComponent<Rigidbody>().AddForce(0, forceJump, 0, ForceMode.Impulse);
            }
        }
        #endregion
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}