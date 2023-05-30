using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TPSCameraController : MonoBehaviour
{
    [SerializeField] float CameraSensitivity;
    [SerializeField] Transform cameraRoot;
    [SerializeField] Transform cameraRoot2;
    [SerializeField] float lookDistance;
    [SerializeField] CinemachineVirtualCamera camera;

    private Vector2 lookDelta;
    private float xRotation;
    private float yRotation;
    private bool change=true;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {

        Vector3 lookPoint = Camera.main.transform.position + Camera.main.transform.forward * lookDistance;
        lookPoint.y = transform.position.y;
        transform.LookAt(lookPoint);
    }

    private void LateUpdate()
    {
        Look();
    }
    private void Look()
    {
        yRotation += lookDelta.x * CameraSensitivity * Time.deltaTime;
        xRotation -= lookDelta.y * CameraSensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        
        cameraRoot.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        
        cameraRoot2.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        
    }
    private void OnLook(InputValue value)
    {
        lookDelta = value.Get<Vector2>();
    }

    private void OnCamera(InputValue value)
    {
        if (value.isPressed)
        {
            camera.Priority = 11;
        }
        else
        {
            Debug.Log("sad");
            camera.Priority = 5;
        }
        
    }
}
