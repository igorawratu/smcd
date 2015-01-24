using UnityEngine;
using System.Collections;

public class WinnerChecker : MonoBehaviour {

	// Use this for initialization
	void Start () {
        mPlayersActive = new List<string>();
        foreach (KeyCode player in CurrentPlayerKeys.Instance.playerKeys){
            mPlayersActive.Add(player.ToString());
        }
	}
	
	// Update is called once per frame
	void Update () {
	}

    void removePlayer(string _player){
        mPlayersActive.remove(_player);
        if (mPlayersActive.Count == 1){
            Application.LoadLevel("EndScene");
        }
    }

    private List<string> mPlayersActive;
}
