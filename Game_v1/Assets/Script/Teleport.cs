using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour {

    public int code;

    private float disableTimer = 0f;
    private Vector3 dir;

    void Update()
    {//maybe bug here when sometimes can't teleport 2 times in row or maybe not bug ... 
        if (disableTimer > 0)
        {
            disableTimer -= Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            foreach (Teleport tp in FindObjectsOfType<Teleport>())
            {
                if (tp.code == code && tp != this && disableTimer <= 0)
                {
                    tp.disableTimer = 1f;

                    Vector3 pos = tp.gameObject.transform.position;

                    collider.gameObject.transform.position = pos;
                }
            }
        }
    }
}
