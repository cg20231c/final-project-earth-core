using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animatorStateController : MonoBehaviour
{
    Animator animator;
    int isWalkingForwardHash;
    int isWalkingBackHash;
    int isWalkingLeftHash;
    int isWalkingRightHash;
    int isRunningHash;
    int isJumpingHash;
    int isCrounchHash;
    int isCrounchHash_idle;
    int isSlideHash;
    int isClimbHash;
    int isGrabHash;
    int isSwingHash;
    
    // [SerializeField]
    public PlayerMovement pm;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingForwardHash = Animator.StringToHash("isWalkingForward");
        isWalkingBackHash = Animator.StringToHash("isWalkingBack");
        isWalkingLeftHash = Animator.StringToHash("isLeftWalking");
        isWalkingRightHash = Animator.StringToHash("isRightWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");
        isCrounchHash = Animator.StringToHash("isCrounching");
        isCrounchHash_idle = Animator.StringToHash("isCrounching_idle");
        isSlideHash = Animator.StringToHash("isSliding");
        isClimbHash = Animator.StringToHash("isClimbing");
        isGrabHash = Animator.StringToHash("isGrapping");
        isSwingHash = Animator.StringToHash("isSwinging");

        // pm = GetComponent<PlayerMovement>();
        pm = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isWalkingForward = animator.GetBool(isWalkingForwardHash);
        bool isWalkingBack = animator.GetBool(isWalkingBackHash);
        bool isWalkingLeft = animator.GetBool(isWalkingLeftHash);
        bool isWalkingRight = animator.GetBool(isWalkingRightHash);
        bool isRunning = animator.GetBool(isRunningHash);
        bool isJumping = animator.GetBool(isJumpingHash);
        bool isClimbing = animator.GetBool(isClimbHash);
        bool isCrounch = animator.GetBool(isCrounchHash);
        bool isCrounch_idle = animator.GetBool(isCrounchHash_idle);
        bool isSlide = animator.GetBool(isSlideHash);
        bool isGrab = animator.GetBool(isGrabHash);
        bool isSwing = animator.GetBool(isSwingHash);

        bool forwardPressed = Input.GetKey("w");
        bool backPressed = Input.GetKey("s");
        bool leftPressed = Input.GetKey("a");
        bool rightPressed = Input.GetKey("d");
        bool runPressed = Input.GetKey("left shift");
        bool jumpPressed = Input.GetKey("space");
        bool crounchPressed = Input.GetKey("c");
        bool slidePressed = Input.GetKey("left ctrl");
        bool swingPressed = Input.GetMouseButton(0);
        bool grabPressed = Input.GetMouseButton(1);

        // forward
        if(!isWalkingForward && forwardPressed){
            animator.SetBool(isWalkingForwardHash, true);
        }
        if(isWalkingForward && !forwardPressed){
            animator.SetBool(isWalkingForwardHash, false);
        }

        // back
        if(!isWalkingBack && backPressed){
            animator.SetBool(isWalkingBackHash, true);
        }
        if(isWalkingBack && !backPressed){
            animator.SetBool(isWalkingBackHash, false);
        }

        // left
        if(!isWalkingLeft && leftPressed){
            animator.SetBool(isWalkingLeftHash, true);
        }
        if(isWalkingLeft && !leftPressed){
            animator.SetBool(isWalkingLeftHash, false);
        }

        // right
        if(!isWalkingRight && rightPressed){
            animator.SetBool(isWalkingRightHash, true);
        }
        if(isWalkingRight && !rightPressed){
            animator.SetBool(isWalkingRightHash, false);
        }

        // run
        if(!isRunning && (forwardPressed && runPressed)){
            animator.SetBool(isRunningHash, true);
        }
        if(isRunning && (!forwardPressed || !runPressed)){
            animator.SetBool(isRunningHash, false);
        }

        // jump
        if((!isJumping && jumpPressed) || (!isRunning && (forwardPressed && runPressed && jumpPressed)) || (!isWalkingForward && (forwardPressed && jumpPressed))){
            animator.SetBool(isJumpingHash, true);
        }

        if((isJumping && !jumpPressed) || (isRunning && (!forwardPressed || !runPressed) && !jumpPressed) || (isWalkingForward && (!forwardPressed || !runPressed) && !jumpPressed)){
            animator.SetBool(isJumpingHash, false);
        }

        // climb
        // Debug.Log(pm.state);
        if(pm.climbing){
            animator.SetBool(isClimbHash, true);
        }
        if(!pm.climbing){
            animator.SetBool(isClimbHash, false);
        }

        // crounch
        if(!isCrounch_idle && crounchPressed && !isCrounch){
            animator.SetBool(isCrounchHash_idle, true);
            animator.SetBool(isCrounchHash, false);
        }
        if(isCrounch_idle && !crounchPressed && isCrounch){
            animator.SetBool(isCrounchHash_idle, false);
        }
        if((!isCrounch && (forwardPressed  && crounchPressed))){
            animator.SetBool(isCrounchHash, true);
            animator.SetBool(isCrounchHash_idle, false);
        }
        if((isCrounch && (!forwardPressed || !crounchPressed))){
            animator.SetBool(isCrounchHash, false);
        }

        // slide
        if(!isSlide && slidePressed || (!isRunning && (forwardPressed && runPressed) && !slidePressed) || (!isWalkingForward && (forwardPressed && slidePressed))){
            animator.SetBool(isSlideHash, true);
        }
        if(isSlide && !slidePressed  || (isRunning && (!forwardPressed || !runPressed) && !slidePressed) || (isWalkingForward && (!forwardPressed) && !slidePressed)){
            animator.SetBool(isSlideHash, false);
        }
        
        // grab
        if(!isGrab && grabPressed){
            animator.SetBool(isGrabHash, true);
        }
        if(isGrab && !grabPressed){
            animator.SetBool(isGrabHash, false);
        }

        // swing
        if(!isSwing && swingPressed){
            animator.SetBool(isSwingHash, true);
        }
        if(isSwing && !swingPressed){
            animator.SetBool(isSwingHash, false);
        }
    }
}
