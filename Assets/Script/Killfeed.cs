using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killfeed : MonoBehaviour {

    [SerializeField]
    GameObject killFeedItemPrefab;

	// Use this for initialization
	void Start () {
        GameManager.instance.onPlayerKillCallback += OnKill;
	}
	
	public void OnKill(string playerUsername, string source)
    {
        GameObject go = Instantiate(killFeedItemPrefab, transform);
        go.GetComponent<KillfeedItem>().Setup(playerUsername, source);
        Destroy(go, 5f);
    }
}
