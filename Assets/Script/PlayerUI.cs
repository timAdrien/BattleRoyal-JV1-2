﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    [SerializeField]
    RectTransform healthBarFill;

    [SerializeField]
    GameObject pauseMenu;

    [SerializeField]
    GameObject scoreboard;

    [SerializeField]
    GameObject waitingScreen;

    [SerializeField]
    GameObject winScreen;

    [SerializeField]
    GameObject loseScreen;

    [SerializeField]
    Text ammoText;

    [SerializeField]
    Text killsText;

    private Player player;
    private WeaponManager weaponManager;

    public void SetPlayer(Player pPlayer, WeaponManager pWeaponManager)
    {
        player = pPlayer;
        weaponManager = pWeaponManager;
    }

    void Start()
    {
        PrincipalPauseMenu.isOn = false;
    }

    private void OnGUI()
    {
        GameObject gs = GameObject.Find("TestStats(Clone)");
        if(gs != null)
            GUI.Label(new Rect(10, 10, 400, 20), gs.GetComponent<testSc>().moppp);
    }

    void Update()
    {
        SetHealthBarAmount(player.GetHealthPct());

        var arme = weaponManager.GetCurrenntWeapon().GetComponent<PlayerWeapon>();
        Debug.Log(arme.ammos);
        SetAmmoAmount(arme.ammos, arme.maxAmmo);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            GameObject.Find("GameSettings").GetComponent<GameSettings>().ScoreLimit++;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            // player.RpcTakeDamage(10, "Player 1");
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            scoreboard.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            scoreboard.SetActive(false);
        }
    }

    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PrincipalPauseMenu.isOn = pauseMenu.activeSelf;
    }

    public void ToggleWaitingScreen()
    {
        waitingScreen.SetActive(false);
    }

    public void ToggleWinScreen()
    {
        winScreen.SetActive(true);
    }

    public void ToggleLoseScreen()
    {
        loseScreen.SetActive(true);
    }

    public void SetHealthBarAmount(float amount)
    {
        healthBarFill.localScale = new Vector3(1f, amount, 1f);
    }

    public void SetAmmoAmount(int amount, int maxAmmo)
    {
        if (amount > maxAmmo)
            amount = maxAmmo;
        ammoText.text = amount + " / " + maxAmmo;
    }

    public void SetKillsAmount(int ammout)
    {
        // killsText.text = ammout + " / " + GameSettings.instance.ScoreLimit;
        killsText.text = ammout.ToString();
    }
}
