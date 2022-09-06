using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{

    [SerializeField] int  sensHori;
    [SerializeField] int  sensVert;
    [SerializeField] int  lockVertMin;
    [SerializeField] int  lockVertMax;
    [SerializeField] bool invert;

    float xRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensHori;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensVert;
        if (invert)
            xRotation += mouseY;
        else
            xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, lockVertMin, lockVertMax);
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
