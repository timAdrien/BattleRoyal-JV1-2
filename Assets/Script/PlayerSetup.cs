//-------------------------------------
// Responsible for setting up the player.
// This includes adding/removing him correctly on the network.
//-------------------------------------

using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {

	[SerializeField]
	Behaviour[] componentsToDisable;

    [SerializeField]
    string remoteLayerName = "RemotePlayerLayer";

    [SerializeField]
    string dontDrawLayerName = "DontDrawLayer";

    [SerializeField]
    private GameObject weaponHolderGFX;

    [SerializeField]
    private string weaponLayerName = "WeaponLayer";
    public string GetWeaponLayerName()
    {
        return weaponLayerName;
    }

    [SerializeField]
    GameObject playerGraphics;

    [SerializeField]
    GameObject playerUIPrefab;

    [HideInInspector]
    public GameObject playerUIInstance;
    

	void Start ()
	{
		// Disable components that should only be
		// active on the player that we control
		if (!isLocalPlayer)
		{
			DisableComponents();
			AssignRemoteLayer();
		}
		else
		{

            Util.SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));
            Util.SetLayerRecursively(weaponHolderGFX, LayerMask.NameToLayer(weaponLayerName));

            playerUIInstance = Instantiate(playerUIPrefab);
            playerUIInstance.name = playerUIPrefab.name;

            PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
            if (ui == null)
                Debug.LogError("No PlayerUI defined...");

            ui.SetPlayer(GetComponent<Player>(), GetComponent<WeaponManager>());

            GetComponent<Player>().PlayerSetup();

            string username = "Chargement...";

            if (UserAccountManager.IsLoggedIn)
                username = UserAccountManager.PlayerUsername;
            else
                username = transform.name;

            CmdSetUsername(transform.name, username);

        }
	}

    [Command]
    void CmdSetUsername(string playerID, string username)
    {
        Player player = GameManager.GetPlayer(playerID);
        if(player != null)
        {
            player.username = username;
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();

        GameManager.RegisterPlayer(_netID, _player);
    }

    void AssignRemoteLayer ()
	{
		gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
	}

	void DisableComponents ()
	{
		for (int i = 0; i < componentsToDisable.Length; i++)
		{
			componentsToDisable[i].enabled = false;
		}
	}

	// When we are destroyed
	void OnDisable ()
	{
		Destroy(playerUIInstance);

        if(isLocalPlayer)
            GameManager.instance.SetSceneCameraState(true);

        GameManager.UnRegisterPlayer(transform.name);
	}

}
