using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class TileManager : MonoBehaviour {

    public GameObject currentTile;
    public GameObject[] tilePrefabs;

    private static TileManager instance;
    private Stack<GameObject> leftTiles = new Stack<GameObject>();

    public Stack<GameObject> LeftTiles
    {
        get { return leftTiles; }
        set { leftTiles = value; }
    }
    private Stack<GameObject> lTopTiles = new Stack<GameObject>();

    public Stack<GameObject> LTopTiles
    {
        get { return lTopTiles; }
        set { lTopTiles = value; }
    }
    private Stack<GameObject> rightTiles = new Stack<GameObject>();

    public Stack<GameObject> RightTiles
    {
        get { return rightTiles; }
        set { rightTiles = value; }
    }

    private Stack<GameObject> rTopTiles = new Stack<GameObject>();

    public Stack<GameObject> RTopTiles
    {
        get { return rTopTiles; }
        set { rTopTiles = value; }
    }

    public static TileManager Instance
    {
        get 
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<TileManager>();
            }
            return instance; 
        }
    }

    private GameObject tile;
    private GameObject next;
    private GameObject nextRight;

    void Awake()
    {
        currentTile = Instantiate(tilePrefabs[1], currentTile.transform.GetChild(0).transform.GetChild(1).position, Quaternion.identity) as GameObject;
        tile = currentTile;
    }

	// Use this for initialization
	void Start () {
        //CreateTiles(200);

        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;

        for (int i = 0; i < 150; i++)
        {
            SpawnTile();
        }
	}

    public void CreateTiles(int amount)
    {
        //creating new tiles for random generated path (using stacks)
        for (int i = 0; i < amount; i++)
        {
            leftTiles.Push(Instantiate(tilePrefabs[0]));
            lTopTiles.Push(Instantiate(tilePrefabs[1]));
            rightTiles.Push(Instantiate(tilePrefabs[2]));
            rTopTiles.Push(Instantiate(tilePrefabs[3]));

            leftTiles.Peek().SetActive(false);
            lTopTiles.Peek().SetActive(false);
            rightTiles.Peek().SetActive(false);
            rTopTiles.Peek().SetActive(false);

            leftTiles.Peek().name = "LeftTile";
            lTopTiles.Peek().name = "LTopTile";
            rightTiles.Peek().name = "RightTile";
            rTopTiles.Peek().name = "RTopTile";
        }
    }

    public void SpawnTile()
    {
        //tile spawn random variables
        int randomLeft = Random.Range(0, 2);
        int randomRight = Random.Range(0, 2);
        int randomTop = Random.Range(0, 2);

        //collectable object spawn random variable
        int teleportIndex = Random.Range(0, 3);

        //bonuses, teleport spawn random variables
        int speedUp = Random.Range(0, 10);
        int slowDown = Random.Range(0, 20);
        int bonusScore = Random.Range(0, 7);

        //condition for checking if there is tiles for path if not then creating new tiless
        /*if (leftTiles.Count == 0 || lTopTiles.Count == 0 || rightTiles.Count == 0 || rTopTiles.Count == 0)
        {
            CreateTiles(100);
        }*/

        //spawning tiles with teleports and bonus score object 
        if (tile == currentTile)
        {
            //spawn tiles to left or top
            if (randomLeft == 0)
            {
                currentTile = Instantiate(tilePrefabs[0], currentTile.transform.GetChild(0).transform.GetChild(0).position, Quaternion.identity) as GameObject;

                if (teleportIndex == 0)
                {
                    BonusScore(bonusScore);
                }
                else if (teleportIndex == 1)
                {
                    currentTile = Instantiate(tilePrefabs[0], currentTile.transform.GetChild(0).transform.GetChild(0).position, Quaternion.identity) as GameObject;
                    SlowDown(slowDown);

                    if (currentTile.transform.GetChild(2).gameObject.activeInHierarchy)
                    {
                        currentTile = Instantiate(tilePrefabs[0], currentTile.transform.GetChild(0).transform.GetChild(0).position, Quaternion.identity) as GameObject;
                        currentTile = Instantiate(tilePrefabs[0], currentTile.transform.GetChild(0).transform.GetChild(0).position, Quaternion.identity) as GameObject;
                    }
                }
                else if (teleportIndex == 2)
                {
                    currentTile = Instantiate(tilePrefabs[0], currentTile.transform.GetChild(0).transform.GetChild(0).position, Quaternion.identity) as GameObject;
                    SpeedUp(speedUp);

                    if (currentTile.transform.GetChild(3).gameObject.activeInHierarchy)
                    {
                        currentTile = Instantiate(tilePrefabs[0], currentTile.transform.GetChild(0).transform.GetChild(0).position, Quaternion.identity) as GameObject;
                        currentTile = Instantiate(tilePrefabs[0], currentTile.transform.GetChild(0).transform.GetChild(0).position, Quaternion.identity) as GameObject;
                    }
                }

                tile = currentTile;
            }
            else if (randomLeft == 1)
            {
                currentTile = Instantiate(tilePrefabs[1], currentTile.transform.GetChild(0).transform.GetChild(1).position, Quaternion.identity) as GameObject;

                if (teleportIndex == 0)
                {
                    BonusScore(bonusScore);
                }
                else if (teleportIndex == 1)
                {
                    currentTile = Instantiate(tilePrefabs[1], currentTile.transform.GetChild(0).transform.GetChild(1).position, Quaternion.identity) as GameObject;
                    SlowDown(slowDown);

                    if (currentTile.transform.GetChild(2).gameObject.activeInHierarchy)
                    {
                        currentTile = Instantiate(tilePrefabs[1], currentTile.transform.GetChild(0).transform.GetChild(1).position, Quaternion.identity) as GameObject;
                    }
                }
                else if (teleportIndex == 2)
                {
                    currentTile = Instantiate(tilePrefabs[1], currentTile.transform.GetChild(0).transform.GetChild(1).position, Quaternion.identity) as GameObject;
                    SpeedUp(speedUp);

                    if (currentTile.transform.GetChild(3).gameObject.activeInHierarchy)
                    {
                        currentTile = Instantiate(tilePrefabs[1], currentTile.transform.GetChild(0).transform.GetChild(1).position, Quaternion.identity) as GameObject;
                    }
                }

                next = currentTile;
            }
        }
        else if (next == currentTile)
        {
            //spawn tiles to top with condition parameter which decide to turn right/left
            if (randomTop == 0)
            {
                currentTile = Instantiate(tilePrefabs[1], currentTile.transform.GetChild(0).transform.GetChild(1).position, Quaternion.identity) as GameObject;

                tile = currentTile;
            }
            else if (randomTop == 1)
            {
                currentTile = Instantiate(tilePrefabs[3], currentTile.transform.GetChild(0).transform.GetChild(1).position, Quaternion.identity) as GameObject;

                nextRight = currentTile;
            }
        }
        else if (nextRight == currentTile)
        {
            //spawn tiles to right or top
            if (randomRight == 0)
            {
                currentTile = Instantiate(tilePrefabs[2], currentTile.transform.GetChild(0).transform.GetChild(0).position, Quaternion.identity) as GameObject;

                if (teleportIndex == 0)
                {
                    BonusScore(bonusScore);
                }
                else if (teleportIndex == 1)
                {
                    currentTile = Instantiate(tilePrefabs[2], currentTile.transform.GetChild(0).transform.GetChild(0).position, Quaternion.identity) as GameObject;
                    SlowDown(slowDown);

                    if (currentTile.transform.GetChild(2).gameObject.activeInHierarchy)
                    {
                        currentTile = Instantiate(tilePrefabs[2], currentTile.transform.GetChild(0).transform.GetChild(0).position, Quaternion.identity) as GameObject;
                        currentTile = Instantiate(tilePrefabs[2], currentTile.transform.GetChild(0).transform.GetChild(0).position, Quaternion.identity) as GameObject;
                    }
                }
                else if (teleportIndex == 2)
                {
                    currentTile = Instantiate(tilePrefabs[2], currentTile.transform.GetChild(0).transform.GetChild(0).position, Quaternion.identity) as GameObject;
                    SpeedUp(speedUp);

                    if (currentTile.transform.GetChild(3).gameObject.activeInHierarchy)
                    {
                        currentTile = Instantiate(tilePrefabs[2], currentTile.transform.GetChild(0).transform.GetChild(0).position, Quaternion.identity) as GameObject;
                        currentTile = Instantiate(tilePrefabs[2], currentTile.transform.GetChild(0).transform.GetChild(0).position, Quaternion.identity) as GameObject;
                    }
                }

                nextRight = currentTile;
            }
            else if (randomRight == 1)
            {
                currentTile = Instantiate(tilePrefabs[1], currentTile.transform.GetChild(0).transform.GetChild(1).position, Quaternion.identity) as GameObject;

                if (teleportIndex == 0)
                {
                    currentTile = Instantiate(tilePrefabs[1], currentTile.transform.GetChild(0).transform.GetChild(1).position, Quaternion.identity) as GameObject;
                    BonusScore(bonusScore);
                }
                else if (teleportIndex == 1)
                {
                    currentTile = Instantiate(tilePrefabs[1], currentTile.transform.GetChild(0).transform.GetChild(1).position, Quaternion.identity) as GameObject;
                    SlowDown(slowDown);

                    if (currentTile.transform.GetChild(2).gameObject.activeInHierarchy)
                    {
                        currentTile = Instantiate(tilePrefabs[1], currentTile.transform.GetChild(0).transform.GetChild(1).position, Quaternion.identity) as GameObject;
                        currentTile = Instantiate(tilePrefabs[1], currentTile.transform.GetChild(0).transform.GetChild(1).position, Quaternion.identity) as GameObject;
                    }
                }
                else if (teleportIndex == 2)
                {
                    currentTile = Instantiate(tilePrefabs[1], currentTile.transform.GetChild(0).transform.GetChild(1).position, Quaternion.identity) as GameObject;
                    SpeedUp(speedUp);

                    if (currentTile.transform.GetChild(3).gameObject.activeInHierarchy)
                    {
                        currentTile = Instantiate(tilePrefabs[1], currentTile.transform.GetChild(0).transform.GetChild(1).position, Quaternion.identity) as GameObject;
                        currentTile = Instantiate(tilePrefabs[1], currentTile.transform.GetChild(0).transform.GetChild(1).position, Quaternion.identity) as GameObject;
                    }
                }

                tile = currentTile;
            }
        }
    }
    
    private void BonusScore(int bonuses)
    {
        //add bonuses object to play field
        if (bonuses == 0)
        {
            currentTile.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    private void SpeedUp(int speedUp)
    {        
        //add speed up object to play field
        if (speedUp == 0)
        {
            currentTile.transform.GetChild(3).gameObject.SetActive(true);
        }
    }

    private void SlowDown(int slowDown)
    {
        //add slow down object to play field
        if (slowDown == 0)
        {
            currentTile.transform.GetChild(2).gameObject.SetActive(true);
        }
    }

    private void LeftTileSpawn()
    {
        int randomIndex = Random.Range(0, 2);

        if (randomIndex == 0)
        {
            GameObject temp = leftTiles.Pop();
            temp.SetActive(true);
            temp.transform.position = currentTile.transform.GetChild(0).transform.GetChild(randomIndex).position;
            currentTile = temp;
        }
        else if (randomIndex == 1)
        {
            GameObject temp = lTopTiles.Pop();
            temp.SetActive(true);
            temp.transform.position = currentTile.transform.GetChild(0).transform.GetChild(randomIndex).position;
            currentTile = temp;
        }
    }

    private void RightTileSpawn()
    {
        int spawnIndex = Random.Range(0, 2);

        if (spawnIndex == 0)
        {
            GameObject temp = rightTiles.Pop();
            temp.SetActive(true);
            temp.transform.position = tile.transform.GetChild(0).transform.GetChild(spawnIndex).position;
            tile = temp;
        }            
        else if (spawnIndex == 1)
        {
            GameObject temp = rTopTiles.Pop();
            temp.SetActive(true);
            temp.transform.position = tile.transform.GetChild(0).transform.GetChild(spawnIndex).position;
            tile = temp;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void GameStart()
    {
        enabled = true;
    }

    private void GameOver()
    {
        enabled = false;
    }

    void OnDestroy()
    {
        GameEventManager.GameStart -= GameStart;
        GameEventManager.GameOver -= GameOver;
    }
}
