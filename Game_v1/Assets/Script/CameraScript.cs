using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

    Transform playerTransform;

    Vector3 cameraOrientationVector = new Vector3(0, 15, -10f);

    private Transform player; 
    private Vector3 relCameraPos;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        relCameraPos = player.position - transform.position;
    }

	// Use this for initialization
	void Start () {
        //playerTransform = GameObject.Find("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = player.position - relCameraPos;
	}

    void LateUpdate()
    {
        //transform.position = playerTransform.position + cameraOrientationVector;
    } 
}
