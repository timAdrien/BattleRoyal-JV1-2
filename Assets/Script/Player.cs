using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(PlayerSetup))]
public class Player : NetworkBehaviour {

	[SyncVar]
	private bool _isDead = false;
	public bool IsDead
	{
		get { return _isDead; }
		protected set { _isDead = value; }
	}

    [SerializeField]
    private int maxHealth = 100;

    [SyncVar]
    private int currentHealth;

    [SyncVar]
    public string username = "Chargement...";

    public int kills;
    public int deaths;

    [SerializeField]
	private Behaviour[] disableOnDeath;
	private bool[] wasEnabled;

    [SerializeField]
    private GameObject deathEffect;

    [SerializeField]
    private GameObject spawnEffect;

    [SerializeField]
    private GameObject[] disableGameObjectsOnDeath;

    private bool isFirstSetup = true;

    public float GetHealthPct()
    {
        // Need cast because int divisions result to 0
        return (float)currentHealth / (float)maxHealth;
    }

    public void PlayerSetup()
    {
        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraState(false);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
        }

        CmdBroadcastNewPlayerSetup();
    }

    [Command]
    private void CmdBroadcastNewPlayerSetup()
    {
        RpcSetupPlayerOnAllClients();
    }

    [ClientRpc]
    private void RpcSetupPlayerOnAllClients()
    {
        if (isFirstSetup)
        {
            wasEnabled = new bool[disableOnDeath.Length];
            for (int i = 0; i < wasEnabled.Length; i++)
            {
                wasEnabled[i] = disableOnDeath[i].enabled;
            }
            isFirstSetup = false;
        }

        SetDefaults();
    }

    [ClientRpc]
    public void RpcTakeDamage (string playerID, int amount, string sourcePlayerID)
    {
		if (IsDead)
			return;

        currentHealth -= amount;

		if (currentHealth <= 0)
        {
            Die(sourcePlayerID);
		}
    }

	private void Die(string sourcePlayerID)
	{
		IsDead = true;

        Player sourcePlayer = GameManager.GetPlayer(sourcePlayerID);
        if(sourcePlayer != null)
        {
            sourcePlayer.kills++;
            if (sourcePlayer.GetComponent<PlayerSetup>().playerUIInstance != null)
            {
                sourcePlayer.GetComponent<PlayerSetup>().playerUIInstance.GetComponent<PlayerUI>().SetKillsAmount(sourcePlayer.kills);
            }
            GameManager.instance.onPlayerKillCallback.Invoke(username, sourcePlayer.username);
        }

        deaths++;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(false);
        }

        Collider _col = GetComponent<Collider>();
		if (_col != null)
			_col.enabled = false;

        GameObject graphicsIns = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(graphicsIns, 3f);

        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraState(true);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);
        }

		Debug.Log(transform.name + " is DEAD!");

		StartCoroutine(Respawn());
	}

	private IEnumerator Respawn ()
	{
		yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);

		Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
		transform.position = _spawnPoint.position;
		transform.rotation = _spawnPoint.rotation;

        yield return new WaitForSeconds(0.1f);

        PlayerSetup();

        Debug.Log(transform.name + " respawned.");
	}

    public void SetDefaults ()
    {
		IsDead = false;

        currentHealth = maxHealth;

		for (int i = 0; i < disableOnDeath.Length; i++)
		{
			disableOnDeath[i].enabled = wasEnabled[i];
        }

        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(true);
        }

        Collider _col = GetComponent<Collider>();
		if (_col != null)
			_col.enabled = true;
        
        GameObject graphicsIns = (GameObject)Instantiate(spawnEffect, transform.position, Quaternion.identity);
        Destroy(graphicsIns, 3f);
    }

}
