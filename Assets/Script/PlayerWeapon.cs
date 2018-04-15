
using UnityEngine;

public class PlayerWeapon : MonoBehaviour {

	public string nameWeapon = "Glock";
	public int damage = 10;

    [HideInInspector]
    public int ammos;

    public int maxAmmo = 10;
    public float range = 100f;
    public float fireRate = 0f;
    public float reloadTime = 1f;
    public float impactForce = 100f;

    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    public PlayerWeapon()
    {
        ammos = maxAmmo;
    }
}
