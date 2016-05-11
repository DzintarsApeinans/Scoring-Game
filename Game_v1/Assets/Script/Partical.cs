using UnityEngine;
using System.Collections;

public class Partical : MonoBehaviour {

    private ParticleSystem particalSystem, teleportEffect;

	// Use this for initialization
	void Start () {
        particalSystem = GetComponent<ParticleSystem>();
        teleportEffect = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!particalSystem.isPlaying)
        {
            Destroy(gameObject);
        }else

        if (!teleportEffect.isPlaying)
        {
            teleportEffect.loop = false;
            Destroy(gameObject);
        }
	}
}
