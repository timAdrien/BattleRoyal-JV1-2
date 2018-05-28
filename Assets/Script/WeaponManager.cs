
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour {

    [SerializeField]
    private GameObject primaryWeapon;

    private GameObject currentWeapon;

    [SerializeField]
    private List<GameObject> lstWeapon = new List<GameObject>();

    [SerializeField]
    private Transform weaponHolder;

    // Use this for initialization
    void Start ()
    {
        Equip(Instantiate(primaryWeapon));
    }

    public GameObject GetCurrenntWeapon()
    {
        return currentWeapon;
    }

    public void SetCurrentWeapon(GameObject weapon)
    {
        currentWeapon = weapon;
    }

    public void AddWeapon(GameObject weapon)
    {
        Equip(weapon);
        weaponHolder.GetComponent<WeaponSwitching>().selectedWeapon++;
        weaponHolder.GetComponent<WeaponSwitching>().SelectWeapon();
    }

    void Equip(GameObject weapon)
    {
        weapon.GetComponent<Animator>().enabled = true;
        weapon.transform.position = weaponHolder.position;
        weapon.transform.rotation = weaponHolder.rotation;
        currentWeapon = weapon;

        lstWeapon.Add(currentWeapon);
        currentWeapon.transform.SetParent(weaponHolder);

        if (isLocalPlayer)
            Util.SetLayerRecursively(currentWeapon, LayerMask.NameToLayer(GetComponent<PlayerSetup>().GetWeaponLayerName()));
    }
}
