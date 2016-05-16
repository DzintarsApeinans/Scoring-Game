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
    //public Transform playerSpawnPoints;

    private float speed = 1.5f;    
    private int score = 0;
    private bool isDead = false;
    private bool isPlaying = false;
    private bool isPause;
    private Vector3 dir;
    private GUIManager guiManager;
    /*private Transform[] spawnPoints;
    private bool reSpawn = false;
    private bool lastToggle = false;*/

	// Use this for initialization
	void Start () {
       
        dir = Vector3.zero;

        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;

        enabled = false;

        guiManager = GameObject.FindObjectOfType<GUIManager>();

        //spawnPoints = playerSpawnPoints.GetComponentsInChildren<Transform>();

        //TODO delete line below later, this line is used to delete all info in PlayerPrefs
        //PlayerPrefs.DeleteAll();
        //Respawn();
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
            GameEventManager.TriggerGameOver();
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            isPause = TogglePause();
            guiManager.pauseText.enabled = false;
        } 
        else
        //moving left condition
	    if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetMouseButtonDown(0) && !isDead && !isPause)
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
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetMouseButtonDown(1) && !isDead && !isPause)
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

    /*private void Respawn()
    {
        int i = Random.Range(1, spawnPoints.Length);
        transform.position = spawnPoints[i].transform.position;
    }*/

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
        }
        else if (collider.tag == "RightTile")
        {
            dir = Vector3.right;
        }
        else if (collider.tag == "LeftTile")
        {
            dir = Vector3.left;
        }
        else if (collider.tag == "TopTile")
        {
            dir = Vector3.forward;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "TopTile" || collider.tag == "LeftTile" || collider.tag == "RightTile")
        {
            RaycastHit hit;
            Ray downRay = new Ray(transform.position, -Vector3.up);
            //Ray leftRay = new Ray(transform.position, -Vector3.left);

            if (!Physics.Raycast(downRay, out hit) /* || !Physics.Raycast(leftRay, out hit)*/)
            {
                Debug.Log("Player is Dead");
                isDead = true;
                GameOver();
                if (transform.childCount > 0)
                {
                    transform.GetChild(0).transform.parent = null;
                }    
            }
        }
    }

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
        //Vector3 halfExtents = new Vector3(0.2f, 0.25f);
        //Collider[] colliders = Physics.OverlapBox(contactPoint.position, halfExtents, Quaternion.identity, whatIsGround, QueryTriggerInteraction.UseGlobal);
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
