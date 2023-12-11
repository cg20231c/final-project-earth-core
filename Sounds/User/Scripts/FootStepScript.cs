using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepScript : MonoBehaviour
{
    public GameObject footstep;
    public PlayerMovement playerMovement; // Reference to the PlayerMovement script
    public AudioSource slidingSound; // Reference to the AudioSource for sliding sound
    public AudioSource airborneSound; // Reference to the AudioSource for airborne sound

    private bool wasGrounded = true; // Track the grounded state in the previous frame
    private float airborneTimer = 0f; // Timer for tracking airborne time
    private bool isAirborneSoundPlaying = false; // Flag to check if airborne sound is currently playing

    // Start is called before the first frame update
    void Start()
    {
        footstep.SetActive(false);
        slidingSound.Stop(); // Stop sliding sound initially
        airborneSound.Stop(); // Stop airborne sound initially
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is grounded before playing footstep sounds
        if (playerMovement.grounded)
        {
            airborneTimer = 0f; // Reset the airborne timer when grounded
            if (isAirborneSoundPlaying)
            {
                airborneSound.Stop(); // Stop airborne sound if it's playing
                isAirborneSoundPlaying = false;
            }

            if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))
            {
                // Play footstep sounds
                footsteps();
            }

            if (Input.GetKeyUp("w") || Input.GetKeyUp("a") || Input.GetKeyUp("s") || Input.GetKeyUp("d"))
            {
                // Stop footstep sounds
                StopFootsteps();
            }

            if (wasGrounded && !playerMovement.grounded)
            {
                // Player was grounded in the previous frame but not in the current frame
                StopFootsteps();
            }

            // Check if the player is sliding
            if (playerMovement.sliding)
            {
                StopFootsteps(); // Stop footstep sounds
                PlaySlidingSound(); // Play sliding sound
            }
            else
            {
                StopSlidingSound(); // Stop sliding sound if not sliding
            }

            // Check if the player is crouching
            if (playerMovement.state == PlayerMovement.MovementState.crouching)
            {
                AdjustFootstepSpeed(0.2f); // Halve the footstep speed when crouching
            }
            // Check if the player is sprinting
            else if (playerMovement.state == PlayerMovement.MovementState.sprinting)
            {
                AdjustFootstepSpeed(1.0f); // Double the footstep speed when sprinting
            }
            else
            {
                AdjustFootstepSpeed(0.6f); // Reset footstep speed to normal
            }
        }
        else
        {
            // Player is not grounded, stop footstep sounds and sliding sound
            StopFootsteps();
            StopSlidingSound();

            // Update airborne timer
            airborneTimer += Time.deltaTime;

            // Check if the player has been airborne for more than 1 second
            if (airborneTimer > 1f && !isAirborneSoundPlaying)
            {
                airborneSound.Play(); // Play airborne sound
                isAirborneSoundPlaying = true;
            }
        }

        wasGrounded = playerMovement.grounded; // Update the grounded state for the next frame
    }

    void footsteps()
    {
        footstep.SetActive(true);
    }

    void StopFootsteps()
    {
        footstep.SetActive(false);
    }

    void PlaySlidingSound()
    {
        if (!slidingSound.isPlaying)
        {
            slidingSound.Play();
        }
    }

    void StopSlidingSound()
    {
        slidingSound.Stop();
    }

    void AdjustFootstepSpeed(float volumeMultiplier)
    {
        footstep.GetComponent<AudioSource>().volume = volumeMultiplier;
    }
}
