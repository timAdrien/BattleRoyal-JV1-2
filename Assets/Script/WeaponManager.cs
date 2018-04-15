
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
    void Start () {
        Equip(primaryWeapon);
    }

    public GameObject GetCurrenntWeapon()
    {
        return currentWeapon;
    }
    
    public void AddWeapon(GameObject weapon)
    {
        lstWeapon.Add(weapon);
        weapon.transform.SetParent(weaponHolder);
        Debug.Log("Weapon added: " + weapon.name);
    }

    void Equip(GameObject weapon)
    {
        currentWeapon = Instantiate(weapon, weaponHolder.position, weaponHolder.rotation);

        currentWeapon.transform.SetParent(weaponHolder);

        if (isLocalPlayer)
            Util.SetLayerRecursively(currentWeapon, LayerMask.NameToLayer(GetComponent<PlayerSetup>().GetWeaponLayerName()));
    }
}
