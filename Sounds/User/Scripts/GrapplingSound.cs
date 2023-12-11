using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingSound : MonoBehaviour
{
    public AudioSource grapplingSound; // Reference to the AudioSource for grappling sound
    public AudioSource hookSound; // Reference to the AudioSource for swinging sound
    public AudioSource swingingSpaceSound; // Reference to the AudioSource for swinging sound

    public Grappling grapplingScript; // Reference to the Grappling script
    public Swinging swingingScript; // Reference to the Swinging script

    private bool hasPlayedHookSound = false; // Flag to track if hook sound has been played

    // Start is called before the first frame update
    void Start()
    {
        grapplingSound.Stop(); // Stop grappling sound initially
        hookSound.Stop(); // Stop swinging sound initially
    }

    // Update is called once per frame
    void Update()
    {
        // Check if grappling is active
        if (grapplingScript.IsGrappling())
        {
            PlayGrapplingSound(); // Play grappling sound
            StopHookSound(); // Stop swinging sound if swinging
            hasPlayedHookSound = false; // Reset the flag when grappling
        }
        else if (swingingScript.pm.swinging)
        {
            if (!hasPlayedHookSound)
            {
                PlayHookSound(); // Play swinging sound only once
                hasPlayedHookSound = true; // Set the flag to true
            }
            StopGrapplingSound(); // Stop grappling sound if grappling
        }
        else
        {
            StopGrapplingSound(); // Stop grappling sound if not grappling
            StopHookSound(); // Stop swinging sound if not swinging
            hasPlayedHookSound = false; // Reset the flag when not grappling or swinging
        }

        // Check if swinging and the space key is pressed
        if (swingingScript.pm.swinging && Input.GetKeyDown(KeyCode.Space))
        {
            PlaySwingingSpaceSound(); // Play additional swinging sound when space is pressed
        }
    }

    void PlayGrapplingSound()
    {
        if (!grapplingSound.isPlaying)
        {
            grapplingSound.Play();
        }
    }

    void StopGrapplingSound()
    {
        grapplingSound.Stop();
    }

    void PlayHookSound()
    {
        if (!hookSound.isPlaying)
        {
            hookSound.Play();
        }
    }

    void StopHookSound()
    {
        hookSound.Stop();
    }

    void PlaySwingingSpaceSound()
    {
        swingingSpaceSound.Play();
    }
    void StopSwingingSpaceSound()
    {
        swingingSpaceSound.Stop();
    }
}
