using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    private float fallDelay = 1.5f;

    public Renderer renderer;

    void Start()
    {
        renderer = GetComponentInChildren<Renderer>();
    }

    void OnTriggerExit(Collider collider)
    {
        //condition for tile falldown and spawning next amount of tiles(path) when player enter tile
        if (collider.tag == "Player")
        {
            TileManager.Instance.SpawnTile();            
            StartCoroutine(ChangeTileColor());
            StartCoroutine(FallDown());
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            renderer.material.color = Color.yellow;
            TileManager.Instance.SpawnTile();
        }
    }

    IEnumerator FallDown()
    {
        //wait few seconds for doing operation in line below
        yield return new WaitForSeconds(fallDelay);
        GetComponent<Rigidbody>().isKinematic = false;

        yield return new WaitForSeconds(fallDelay + 0.5f);
        //"case of" condition for deciding which tiles push in correct stack, disable it and set rigidbody isKinematic to true
        switch (gameObject.name)
        {
            case "LeftTile(Clone)":
                TileManager.Instance.LeftTiles.Push(gameObject);
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                gameObject.SetActive(false);
                Destroy(gameObject);
                break;

            case "LTopTile(Clone)":
                TileManager.Instance.LTopTiles.Push(gameObject);
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                gameObject.SetActive(false);
                Destroy(gameObject);
                break;

            case "RightTile(Clone)":
                TileManager.Instance.RightTiles.Push(gameObject);
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                gameObject.SetActive(false);
                Destroy(gameObject);
                break;

            case "RTopTile(Clone)":
                TileManager.Instance.RTopTiles.Push(gameObject);
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                gameObject.SetActive(false);
                Destroy(gameObject);
                break;

            default:
                break;
        }
    }

    IEnumerator ChangeTileColor()
    {
        yield return new WaitForSeconds(2.0f);
        renderer.material.color = Color.gray;
    }
}
