using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class climbing : MonoBehaviour
{

    [Header("References")]
    public Transform orientation;
    public Rigidbody rb;
    public PlayerMovement pm;
    public LayerMask whatIsWall;

    [Header("Climbing")]
    public float climbSpeed;
    public float maxClimbTime;
    private float climbTimer;

    private bool climb;

    [Header("ClimbJumping")]
    public float climbJumpUpForce;
    public float climbJumpBackForce;

    public KeyCode jumpKey = KeyCode.Space;
    private string joystickJumpButton = "joystick button 0";
    private bool jumpKeyPressed;
    public int climbJumps;
    private int climbJumpsLeft;

    [Header("Detection")]
    public float detectionLength;
    public float sphereCastRadius;
    public float maxWallLookAngle;
    private float wallLookAngle;

    private RaycastHit frontWallHit;
    private bool wallFront;

    private Transform lastWall;
    private Vector3 lastWallNormal;
    public float minWallNormalAngleChange;

    [Header("Exiting")]
    public bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;

    private KeyCode forwardKey = KeyCode.W;
    private string joystickForwardAxis = "Vertical";
    private bool forwardKeyPressed;
    private float joystickForwardInput;
    private bool joystickForwardPressed;
    private bool moveForward;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        jumpKeyPressed = Input.GetKeyDown(jumpKey) || Input.GetButtonDown(joystickJumpButton);
        // Check for keyboard input
        forwardKeyPressed = Input.GetKey(forwardKey);

        // Check for joystick input
        joystickForwardInput = Input.GetAxis(joystickForwardAxis);
        joystickForwardPressed = joystickForwardInput > 0.0f;

        // Combine both inputs
        moveForward = forwardKeyPressed || joystickForwardPressed;

        WallCheck();
        StateMachine();

        if(climb && !exitingWall) ClimbingMovement();
    }

    private void StateMachine()
    {
        if(wallFront && moveForward && wallLookAngle < maxWallLookAngle && !exitingWall)
        {
            if(!climb && climbTimer > 0)
            {
                StartClimbing();
            }

            if(climbTimer > 0) climbTimer -= Time.deltaTime;
            if(climbTimer < 0) StopClimbing();
        }

        else if(exitingWall)
        {
            if (climb) StopClimbing();
            if(exitWallTimer > 0) exitWallTimer -= Time.deltaTime;
            if (exitWallTimer < 0) exitingWall = false;
        }

        else
        {
            if(climb) StopClimbing() ;
        }

        if(wallFront && jumpKeyPressed && climbJumpsLeft > 0) ClimbJump();
    }

    private void WallCheck()
    {
        wallFront = Physics.SphereCast(transform.position, sphereCastRadius, orientation.forward, out frontWallHit, detectionLength, whatIsWall);
        wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);

        bool newWall = frontWallHit.transform != lastWall || Mathf.Abs(Vector3.Angle(lastWallNormal, frontWallHit.normal)) > minWallNormalAngleChange;

        if ( ( wallFront && newWall ) || pm.grounded )
        {
            climbTimer = maxClimbTime;
            climbJumpsLeft = climbJumps;
        }
    }

    private void StartClimbing()
    {
        climb = true;
        pm.climbing = true;

        lastWall = frontWallHit.transform;
        lastWallNormal = frontWallHit.normal;

    }

    private void ClimbingMovement()
    {
        rb.velocity = new Vector3(rb.velocity.x, climbSpeed, rb.velocity.z);
    }

    private void StopClimbing()
    {
        climb = false;
        pm.climbing = false;
    }

    private void ClimbJump()
    {
        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 forceToApply = transform.up * climbJumpUpForce + frontWallHit.normal * climbJumpBackForce;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);

        climbJumpsLeft--;
    }
}
