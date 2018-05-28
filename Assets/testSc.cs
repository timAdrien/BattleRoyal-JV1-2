using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class testSc : NetworkBehaviour {

    [SyncVar]
    public string moppp = "dera";

    void Update()
    {
        if(isServer)
            if (Input.GetKey(KeyCode.P))
            {
                CmdBroadcastSettings();
            }
    }

    [Command]
    private void CmdBroadcastSettings()
    {
        moppp = "TADAAA: " + Random.value * 100;
    }
}
