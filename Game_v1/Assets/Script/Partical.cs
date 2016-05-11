using UnityEngine;
using System.Collections;

public class Partical : MonoBehaviour {

    private ParticleSystem particalSystem;

	// Use this for initialization
	void Start () {
        particalSystem = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!particalSystem.isPlaying)
        {
            Destroy(gameObject);
        }
	}
}
