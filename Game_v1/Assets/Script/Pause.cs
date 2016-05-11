using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {

    private bool isPause;
    private GUIManager guiManager;

    void Start()
    {
        guiManager = GameObject.FindObjectOfType<GUIManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            isPause = TogglePause();
            guiManager.pauseText.enabled = false;
        }            
    }

    void OnGUI()
    {
        if (isPause)
        {
            guiManager.pauseText.enabled = true;
        }
    }

    private bool TogglePause()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            return (false);
        }
        else
        {
            Time.timeScale = 0f;
            return (true);
        }
    }
}
