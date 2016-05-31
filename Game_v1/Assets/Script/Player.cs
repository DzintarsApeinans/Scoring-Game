using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour {

    public GameObject particalSystem;
    public GameObject teleportEffect;
    public Animator anim;
    public Text[] scoreTexts;
    public LayerMask whatIsGround;
    public Transform contactPoint;

    private float speed = 1.5f;    
    private int score = 0;
    private bool isDead = false;
    private bool isPlaying = false;
    private bool isPause;
    private Vector3 dir;
    private GUIManager guiManager;

	// Use this for initialization
	void Start () {
       
        dir = Vector3.zero;

        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;

        enabled = false;

        guiManager = GameObject.FindObjectOfType<GUIManager>();

        //TODO delete line below later, this line is used to delete all info in PlayerPrefs
        //PlayerPrefs.DeleteAll();
	}
	
	// Update is called once per frame
	void Update () {
        float amountToMove = speed * Time.deltaTime;
        //increasing object(player) speed each frame
        speed += Time.deltaTime;

        //player dead condition
        if (!IsGrounded() && isPlaying)
        {
            isDead = true;
            GameOver();
            if (transform.childCount > 0)
            {
                transform.GetChild(0).transform.parent = null;
            }
            gameObject.SetActive(false);
            GameEventManager.TriggerGameOver();
        }

        //pause condition
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            isPause = TogglePause();
            guiManager.pauseText.enabled = false;
            guiManager.gamePause.enabled = false;
        } 
        else
        //moving left condition
            if (Input.GetKeyDown(KeyCode.LeftArrow) && !isDead && !isPause || Input.GetMouseButtonDown(0) && !isDead && !isPause)
        {
            isPlaying = true;
            score++;
            scoreTexts[3].text = score.ToString();
            if(dir == Vector3.forward)
            {
                dir = Vector3.left;

            }
            else
            {
                dir = Vector3.forward;
            }
        }//moving right condition
        else if (Input.GetKeyDown(KeyCode.RightArrow) && !isDead && !isPause || Input.GetMouseButtonDown(1) && !isDead && !isPause)
        {
            isPlaying = true;
            score++;
            scoreTexts[3].text = score.ToString();
            if (dir == Vector3.forward)
            {
                dir = Vector3.right;
            }
            else
            {
                dir = Vector3.forward;
            }
        }
        transform.Translate(dir * amountToMove);
	}

    void OnTriggerEnter(Collider collider)
    {
        //scoring condition
        if (collider.tag == "Score")
        {
            collider.gameObject.SetActive(false);
            Instantiate(particalSystem, transform.position, Quaternion.identity);
            score+=3;
            scoreTexts[3].text = score.ToString();
        }//slow down condition
        else if (collider.tag == "Slow")
        {
            collider.gameObject.SetActive(false);
            speed = speed / 2f;
            Instantiate(teleportEffect, transform.position, Quaternion.identity);
        }//speed up condition
        else if (collider.tag == "SpeedUp")
        {
            collider.gameObject.SetActive(false);
            Instantiate(teleportEffect, transform.position, Quaternion.identity);
            speed = speed + 0.5f;
        }//direction validation after teleport
        else if (collider.tag == "RightTile")
        {
            if (dir == Vector3.left)
            {
                dir = Vector3.right;
            }   
        }
        else if (collider.tag == "LeftTile")
        {
            if (dir == Vector3.right)
            {
                dir = Vector3.left;
            }    
        }
    }

    /*void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Tile" || collider.tag == "LeftTile" || collider.tag == "RightTile" || collider.tag == "TopTile")
        {
            RaycastHit hit;
            Ray downRay = new Ray(transform.position, -Vector3.up);

            if (!Physics.Raycast(downRay, out hit))
            {
                //Debug.Log("Player is Dead");
                isDead = true;
                GameOver();
                if (transform.childCount > 0)
                {
                    transform.GetChild(0).transform.parent = null;
                }    
            }
        }
    }*/

    private void GameStart()
    {
        enabled = true;
    }

    public void GameOver()
    {     
        //game over condition + best score saving
        anim.SetTrigger("isGameOver");
        scoreTexts[0].text = score.ToString();
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        if (score > bestScore)
        {
            PlayerPrefs.SetInt("BestScore", score);
            scoreTexts[2].gameObject.SetActive(true);
        }
        scoreTexts[1].text = PlayerPrefs.GetInt("BestScore", 0).ToString();
        scoreTexts[3].gameObject.SetActive(false);

        enabled = false;
    }

    private bool IsGrounded()
    {
        //condition for notice if player is still on path
        Collider[] colliders = Physics.OverlapSphere(contactPoint.position, 0.01f, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                return true;
            }
        }
        return false;
    }

    void OnDestroy()
    {
        GameEventManager.GameStart -= GameStart;
        GameEventManager.GameOver -= GameOver;
    }

    void OnGUI()
    {
        if (isPause)
        {
            guiManager.pauseText.enabled = true;
            guiManager.gamePause.enabled = true;
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
