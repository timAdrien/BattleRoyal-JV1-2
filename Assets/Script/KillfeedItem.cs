using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillfeedItem : MonoBehaviour
{

    [SerializeField]
    Text text;

    // Use this for initialization
    public void Setup(string player, string source)
    {
        text.text = "<b>" + source + "</b>" + " a tué " + "<i>" + player + "</i>";
    }
}
