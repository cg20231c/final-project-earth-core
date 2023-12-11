using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Import Unity's UI namespace
using UnityEngine.SceneManagement;


public class CollectingCoin : MonoBehaviour
{
    public int coins;
    public int TargetCoins;
    private bool enableQuit = false;
    public Text coinCountText; // Reference to the Text UI element
    public AudioSource Music;

    void Start()
    {
        // Initialize the UI text
        UpdateCoinCountText();
    }

    void Update()
    {
        if (enableQuit == true)
        {
            if (Input.GetKey("escape"))
            {
                ChangeScene();
            }
        }
    }

    void OnTriggerEnter(Collider Col)
    {
        if (Col.gameObject.tag == "Coin")
        {
            Debug.Log("Coin Collected!");
            coins = coins + 1;
            UpdateCoinCountText(); // Update the UI text

            if (coins == TargetCoins)
            {
                PlayMusic();
                enableQuit = true;
            }
            
            

        }

    }

    void UpdateCoinCountText()
    {
        // Update the UI text with the current coin count
        coinCountText.text = "Coins: " + coins.ToString() + " / " + TargetCoins.ToString();
    }

    void ChangeScene()
    {
        SceneManager.LoadScene("EndingScene");
    }

    void PlayMusic()
    {
        Music.Play();
    }

    
}
