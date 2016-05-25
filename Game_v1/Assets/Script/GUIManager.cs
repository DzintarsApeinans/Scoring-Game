using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIManager : MonoBehaviour {

    public GUIText gameStartText, toStartText, instructionsText, pauseText;
    public GUITexture background;

    public Text gameName, gamePause, gameInstruction, toStart;
    public Image gameBackground;

    private Player player;
    private bool muteToggle = false;
    private float nativeWidth = 1280; //1920
    private float nativeHeight = 720; //1080

	// Use this for initialization
	void Start () {
        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;

        player = GameObject.FindObjectOfType<Player>();
        pauseText.enabled = false;
        gamePause.enabled = false;
	}

	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Jump"))
        {
            GameEventManager.TriggerGameStart();
        }            
	}

    void OnGUI()
    {
        float rx = Screen.width / nativeWidth;
        float ry = Screen.height / nativeHeight;
        GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(rx, ry, 1));

        muteToggle = GUI.Toggle(new Rect(1150, 25, 500, 500), muteToggle, "Mute");
        ToggleMute();
    }

    private void ToggleMute()
    {
        if (muteToggle == false)
        {
            AudioListener.volume = 1;
        }
        else if (muteToggle == true)
        {
            AudioListener.volume = 0;
        }
    }

    private void GameStart()
    {        
        //GUI
        gameStartText.enabled = false;
        toStartText.enabled = false;
        instructionsText.enabled = false;
        background.enabled = false;

        //UI Canvas
        gameName.enabled = false;
        gameInstruction.enabled = false;
        toStart.enabled = false;
        gameBackground.enabled = false;
        //enabled = false;
    }

    private void GameOver()
    {      
        player.GameOver();
        muteToggle = false;
        ToggleMute();
        enabled = true;
    }

    void OnDestroy()
    {
        GameEventManager.GameStart -= GameStart;
        GameEventManager.GameOver -= GameOver;
    }
}
