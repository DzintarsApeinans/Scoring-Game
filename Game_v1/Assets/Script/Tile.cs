using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    private float fallDelay = 1.5f;

    void OnTriggerExit(Collider collider)
    {
        //condition for tile falldown and spawning next amount of tiles(path) when player enter tile
        if (collider.tag == "Player")
        {
            TileManager.Instance.SpawnTile();
            //StartCoroutine(FallDown());
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            Debug.Log("change color");
            //transform.GetChild(0).transform.renderer.material.color = Color.yellow;
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
            case "LeftTile":
                TileManager.Instance.LeftTiles.Push(gameObject);
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                gameObject.SetActive(false);
                break;

            case "LTopTile":
                TileManager.Instance.LTopTiles.Push(gameObject);
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                gameObject.SetActive(false);
                break;

            case "RightTile":
                TileManager.Instance.RightTiles.Push(gameObject);
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                gameObject.SetActive(false);
                break;

            case "RTopTile":
                TileManager.Instance.RTopTiles.Push(gameObject);
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                gameObject.SetActive(false);
                break;

            default:
                break;
        }
    }
}
