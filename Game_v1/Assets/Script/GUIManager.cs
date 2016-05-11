using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {

    public GUIText gameStartText, toStartText, instructionsText, pauseText;

    private Player player;

	// Use this for initialization
	void Start () {
        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;

        player = GameObject.FindObjectOfType<Player>();
        pauseText.enabled = false;
	}

	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1"))
        {
            GameEventManager.TriggerGameStart();
        }            
	}

    private void GameStart()
    {        
        gameStartText.enabled = false;
        toStartText.enabled = false;
        instructionsText.enabled = false;        
        enabled = false;
    }

    private void GameOver()
    {
        player.GameOver();
        enabled = true;
    }

    void OnDestroy()
    {
        GameEventManager.GameStart -= GameStart;
        GameEventManager.GameOver -= GameOver;
    }
}
