using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pause_controller_button : MonoBehaviour
{   
    public pause_controller pc;

    void Start()
    {
        // pm = GetComponent<PlayerMovement>();
        pc = FindObjectOfType<pause_controller>();
    }
    public void Resume(){
        Debug.Log("resume");
        pc.pauseMenuUI.SetActive(false);
        pc.isPaused = false;
        Time.timeScale = 1;
    }

    public void Restart(){
        Debug.Log("restat");
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame(){
        Debug.Log("quit");
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(0);
    }
}
