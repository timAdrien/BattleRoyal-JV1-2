using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        RaycastHit hit;

        Camera cam = GetComponent<PlayerShoot>().GetCamPrincipale();
        doorRight.doorLooked = false;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 3))
        {
            if (hit.collider.name == "doorCorners_right" || hit.collider.name == "doorCorners_left")
            {
                doorRight.doorLooked = true;
            }
        }
    }
}
