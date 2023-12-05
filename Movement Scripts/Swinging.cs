using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swinging : MonoBehaviour
{
    [Header("References")]
    public LineRenderer lr;
    public Transform gunTip, cam, player;
    public LayerMask whatIsGrappleable;
    public PlayerMovement pm;

    [Header("Swinging")]
    public float maxSwingDistance = 100f;
    private Vector3 swingPoint;
    private SpringJoint joint;

    [Header("OdmGear")]
    public Transform orientation;
    public Rigidbody rb;
    public float horizontalThrustForce;
    public float forwardThrustForce;
    public float extendCableSpeed;

    [Header("Prediction")]
    public RaycastHit predictionHit;
    public float predictionSphereCastRadius;
    public Transform predictionPoint;

    [Header("Input")]
    public KeyCode swingKey = KeyCode.Mouse0;
    private string joystickSwingButton = "joystick button 4";

    private bool swingKeyDown;
    private bool swingKeyUp;

    // Movement variables
    bool forwardKeyPressed;
    bool backwardKeyPressed;
    bool leftKeyPressed;
    bool rightKeyPressed;

    bool joystickForwardPressed;
    bool joystickBackwardPressed;
    bool joystickLeftPressed;
    bool joystickRightPressed;

    float joystickForwardInput;
    float joystickBackwardInput;
    float joystickLeftInput;
    float joystickRightInput;

    // Combined movement variables
    bool moveForward;
    bool moveBackward;
    bool moveLeft;
    bool moveRight;

    // Define keyboard keys
    KeyCode forwardKey = KeyCode.W;
    KeyCode backwardKey = KeyCode.S;
    KeyCode leftKey = KeyCode.A;
    KeyCode rightKey = KeyCode.D;
    public KeyCode jumpKey = KeyCode.Space;

    // Define joystick axes
    string joystickForwardAxis = "Vertical";
    string joystickBackwardAxis = "Vertical";
    string joystickLeftAxis = "Horizontal";
    string joystickRightAxis = "Horizontal";
    private string joystickJumpButton = "joystick button 0";
    private bool jumpKeyPressed;

    private void Update()
    {
        // Check for keyboard input
        forwardKeyPressed = Input.GetKey(forwardKey);
        backwardKeyPressed = Input.GetKey(backwardKey);
        leftKeyPressed = Input.GetKey(leftKey);
        rightKeyPressed = Input.GetKey(rightKey);

        // Check for joystick input
        joystickForwardInput = Input.GetAxis(joystickForwardAxis);
        joystickBackwardInput = Input.GetAxis(joystickBackwardAxis);
        joystickLeftInput = Input.GetAxis(joystickLeftAxis);
        joystickRightInput = Input.GetAxis(joystickRightAxis);

        // Combine both inputs
        moveForward = forwardKeyPressed || joystickForwardInput > 0.0f;
        moveBackward = backwardKeyPressed || joystickBackwardInput > 0.0f;
        moveLeft = leftKeyPressed || joystickLeftInput > 0.0f;
        moveRight = rightKeyPressed || joystickRightInput > 0.0f;

        jumpKeyPressed = Input.GetKey(jumpKey) || Input.GetButton(joystickJumpButton);







        swingKeyDown = Input.GetKeyDown(swingKey) || Input.GetButtonDown(joystickSwingButton);
        swingKeyUp = Input.GetKeyUp(swingKey) || Input.GetButtonUp(joystickSwingButton);

        if (swingKeyDown) StartSwing();
        if (swingKeyUp) StopSwing();

        CheckForSwingPoints();

        if (joint != null) OdmGearMovement();
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    private void CheckForSwingPoints()
    {
        if (joint != null) return;

        RaycastHit sphereCastHit;
        Physics.SphereCast(cam.position, predictionSphereCastRadius, cam.forward,
                            out sphereCastHit, maxSwingDistance, whatIsGrappleable);

        RaycastHit raycastHit;
        Physics.Raycast(cam.position, cam.forward,
                            out raycastHit, maxSwingDistance, whatIsGrappleable);

        Vector3 realHitPoint;

        if (raycastHit.point != Vector3.zero)
            realHitPoint = raycastHit.point;

        else if (sphereCastHit.point != Vector3.zero)
            realHitPoint = sphereCastHit.point;

        else
            realHitPoint = Vector3.zero;

        if (realHitPoint != Vector3.zero)
        {
            predictionPoint.gameObject.SetActive(true);
            predictionPoint.position = realHitPoint;
        }
        else
        {
            predictionPoint.gameObject.SetActive(false);
        }

        predictionHit = raycastHit.point == Vector3.zero ? sphereCastHit : raycastHit;
    }


    private void StartSwing()
    {

        if (predictionHit.point == Vector3.zero) return;

        if (GetComponent<Grappling>() != null)
            GetComponent<Grappling>().StopGrapple();
        pm.ResetRestrictions();

        pm.swinging = true;

        swingPoint = predictionHit.point;
        joint = player.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = swingPoint;

        float distanceFromPoint = Vector3.Distance(player.position, swingPoint);

        joint.maxDistance = distanceFromPoint * 0.8f;
        joint.minDistance = distanceFromPoint * 0.25f;

        joint.spring = 4.5f;
        joint.damper = 7f;
        joint.massScale = 4.5f;

        lr.positionCount = 2;
        currentGrapplePosition = gunTip.position;
    }

    public void StopSwing()
    {
        pm.swinging = false;

        lr.positionCount = 0;

        Destroy(joint);
    }

    private void OdmGearMovement()
    {

        if (moveRight) rb.AddForce(orientation.right * horizontalThrustForce * Time.deltaTime);

        if (moveLeft) rb.AddForce(-orientation.right * horizontalThrustForce * Time.deltaTime);

        if (moveForward) rb.AddForce(orientation.forward * horizontalThrustForce * Time.deltaTime);

        if (jumpKeyPressed)
        {
            Vector3 directionToPoint = swingPoint - transform.position;
            rb.AddForce(directionToPoint.normalized * forwardThrustForce * Time.deltaTime);

            float distanceFromPoint = Vector3.Distance(transform.position, swingPoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;
        }

        if (moveBackward)
        {
            float extendedDistanceFromPoint = Vector3.Distance(transform.position, swingPoint) + extendCableSpeed;

            joint.maxDistance = extendedDistanceFromPoint * 0.8f;
            joint.minDistance = extendedDistanceFromPoint * 0.25f;
        }
    }

    private Vector3 currentGrapplePosition;

    private void DrawRope()
    {
        if (!joint) return;

        currentGrapplePosition =
            Vector3.Lerp(currentGrapplePosition, swingPoint, Time.deltaTime * 8f);

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }
}
