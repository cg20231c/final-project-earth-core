using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;

    public bool invertVerticalJoystick = true;

    // Flip camera variables
    bool isFlipped = false;
    public string flipButton = "joystick button 0"; // Adjust the button index based on your joystick configuration

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Flip camera horizontally if the flipButton is pressed


        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        float joystickX = Input.GetAxis("RightStickX") * Time.deltaTime * sensX;
        float joystickY = invertVerticalJoystick ? -Input.GetAxis("RightStickY") : Input.GetAxis("RightStickY");
        joystickY *= Time.deltaTime * sensY;

        yRotation += mouseX + joystickX;
        xRotation -= mouseY + joystickY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotate cam and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, isFlipped ? yRotation + 180f : yRotation, 0);
    }
}
