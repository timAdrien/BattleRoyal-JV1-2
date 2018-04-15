using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

    private const string PLAYER_TAG = "Player";


    private GameObject currentWeapon;
    private PlayerWeapon currentWeaponStats;

    private WeaponManager weaponManager;

    private bool isReloading;
    private float nextTimeFire = 0f;

    [SerializeField]
    private Camera cam;
    public Camera GetCamPrincipale()
    {
        return cam;
    }

    [SerializeField]
	private LayerMask mask;

	void Start ()
	{
		if (cam == null)
		{
			Debug.LogError("PlayerShoot: No camera referenced !");
			this.enabled = false;
		}
	}
    
	void Update ()
	{
        currentWeapon = GetComponent<WeaponManager>().GetCurrenntWeapon();
        currentWeaponStats = currentWeapon.GetComponent<PlayerWeapon>();

        if (PrincipalPauseMenu.isOn)
            return;

        if ((currentWeaponStats.ammos <= 0 || (Input.GetKeyDown(KeyCode.R) && currentWeaponStats.ammos < currentWeaponStats.maxAmmo)) && !isReloading)
        {
            Reload();
            return;
        }
        if( currentWeaponStats.fireRate <= 0f)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !isReloading)
            {
                Shoot();
            }
        } else
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !isReloading)
            {
               InvokeRepeating("Shoot", 0f, 1f/currentWeaponStats.fireRate);
            } else if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                CancelInvoke("Shoot");
            }
        }
    }

    [Client]
	void Shoot ()
	{
        if (!isLocalPlayer)
        {
            return;
        }

        CmdOnShoot();

        RaycastHit hit;
        currentWeaponStats.muzzleFlash.Play();
        currentWeaponStats.ammos--;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, currentWeaponStats.range))
        {
            Target target = hit.transform.GetComponent<Target>();

            if (target != null)
            {
                target.TakeDamage(currentWeaponStats.damage);
            }

            if (hit.rigidbody != null && hit.collider.tag != PLAYER_TAG)
            {
                hit.rigidbody.AddForce(-hit.normal * currentWeaponStats.impactForce);
            }

            if (hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(hit.collider.name, currentWeaponStats.damage, transform.name);
            }

            CmdOnHit(hit.point, hit.normal);
        }
	}

    [Command]
    void CmdOnShoot()
    {
        RpcDoShootEffect();
    }

    [Command]
    void CmdOnHit(Vector3 pos, Vector3 normal)
    {
        RpcDoHitEffect(pos, normal);
    }

    [ClientRpc]
    void RpcDoShootEffect()
    {
        currentWeaponStats.muzzleFlash.Play();
    }

    [ClientRpc]
    void RpcDoHitEffect(Vector3 pos, Vector3 normal)
    {
        GameObject impactGO = Instantiate(currentWeaponStats.impactEffect, pos, Quaternion.LookRotation(normal));
        Destroy(impactGO, 0.6f);
    }

    [Command]
	void CmdPlayerShot (string playerID, int damage, string sourcePlayerID)
	{
		Debug.Log(playerID + " has been shot.");

        Player player = GameManager.GetPlayer(playerID);
        player.RpcTakeDamage(playerID, damage, sourcePlayerID);
	}

    public void Reload()
    {
        if (!isLocalPlayer)
            return;
        StartCoroutine(Reload_Coroutine());
    }


    private IEnumerator Reload_Coroutine()
    {
        isReloading = true;
        CancelInvoke("Shoot");

        if (currentWeapon.GetComponent<Animator>().gameObject.activeSelf)
            currentWeapon.GetComponent<Animator>().SetTrigger("Reload");

        yield return new WaitForSeconds(currentWeapon.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        
        isReloading = false;
        currentWeaponStats.ammos = currentWeaponStats.maxAmmo;
    }
}
