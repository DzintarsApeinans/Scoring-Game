using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

    Transform playerTransform;

    private Transform player; 
    private Vector3 relCameraPos;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        relCameraPos = player.position - transform.position;
    }

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        transform.position = player.position - relCameraPos;
	}
}
