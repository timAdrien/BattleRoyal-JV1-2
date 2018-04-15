using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class doorRight : NetworkBehaviour
{
    public int angleStart;
    public int angleEnd;

    private int angleToGo = -1;
    private float Resulting_Value_from_Input;
    private Quaternion Quaternion_Rotate_From;
    private Quaternion Quaternion_Rotate_To;
    private bool canOpenClose = true;
    public static bool doorLooked = false;

    GUIStyle style = new GUIStyle();

    void Start()
    {
        // Position the Text in the center of the Box
        style.alignment = TextAnchor.MiddleCenter;
        style.normal.textColor = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && doorLooked)
        {
            if (canOpenClose)
                StartCoroutine(OpenCloseDoor());
        }
    }

    public IEnumerator OpenCloseDoor()
    {
        canOpenClose = false;
        float percentRotation = 0f;

        if (angleToGo == -1)
            angleToGo = angleEnd;

        while (percentRotation < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, angleToGo, 0), percentRotation);
            percentRotation += 0.1f;
            yield return new WaitForSeconds(0.05f);
        }

        canOpenClose = true;
        angleToGo = angleToGo == angleStart ? angleEnd : angleStart;
    }

    void OnGUI()
    {
        if (doorLooked)
        {
            style.normal.background = Util.MakeTex(200, 50, new Color(0.1f, 0.1f, 0.1f, 0.3f));
            GUI.Box(new Rect((Screen.width / 2) - 125, (Screen.height / 2) - 25, 250, 50), "Appuyer sur 'T' pour ouvrir/fermer la porte", style);
        }
    }

    //[Command]
    //void CmdOpendDoor()
    //{
    //    OnOpendDoor();
    //}

    //[ClientRpc]
    //void COnOpendDoor()
    //{
    //    OnOpendDoor();
    //}


}
