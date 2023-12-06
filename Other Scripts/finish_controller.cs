using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finish_controller : MonoBehaviour
{
    public CollectingCoin cc;
    public GameObject finishMenuUI;
    // Start is called before the first frame update
    void Start()
    {
        cc = FindObjectOfType<CollectingCoin>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(cc.getEnableQuit());
        if (cc.getEnableQuit() == true)
        {
            finishMenuUI.SetActive(true);
            if(Input.GetKeyDown(KeyCode.Return)){
                Debug.Log(cc.getEnableQuit());
                finishMenuUI.SetActive(false);
                cc.Music.Stop();
                cc.setEnableQuit();
            }
        }
    }
}
