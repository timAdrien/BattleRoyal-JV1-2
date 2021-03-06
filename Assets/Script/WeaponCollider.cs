﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponCollider : NetworkBehaviour {

    GUIStyle style = new GUIStyle();
    bool canTakeWeapon = false;
    
    WeaponCollider wc;
    WeaponManager wm;

    void Start()
    {
        // Position the Text in the center of the Box
        style.alignment = TextAnchor.MiddleCenter;
        style.normal.textColor = Color.white;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canTakeWeapon)
        {
            wc.enabled = false;
            wm.AddWeapon(transform.parent.gameObject);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        bool isLocalP = other.gameObject.GetComponent<Player>().isLocalPlayer;
        if (!canTakeWeapon && isLocalP)
        {
            canTakeWeapon = true;
            wc = gameObject.GetComponent<WeaponCollider>();
            wm = other.GetComponent<WeaponManager>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canTakeWeapon = false;
    }


    void OnGUI()
    {
        if (canTakeWeapon)
        {
            style.normal.background = Util.MakeTex(200, 50, new Color(0.1f, 0.1f, 0.1f, 0.3f));
            GUI.Box(new Rect((Screen.width / 2) - 125, (Screen.height / 2) - 25, 250, 50), "Appuyer sur 'E' pour récuperer l'arme", style);
        }
    }
}
