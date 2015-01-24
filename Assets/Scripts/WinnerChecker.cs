using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WinnerChecker : MonoBehaviour {
    public Text gameOver;

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

    public void removePlayer(string _player){
        if (mPlayersActive.Count == 1)
            return;

        mPlayersActive.Remove(_player);
        if (mPlayersActive.Count == 1){
            string winner = mPlayersActive[0];
            Dictionary<string, int> scoreBook = CurrentPlayerKeys.Instance.playerScores;
            if (CurrentPlayerKeys.Instance.playerScores.ContainsKey(winner)){
                CurrentPlayerKeys.Instance.playerScores[winner]++;
            }
            else CurrentPlayerKeys.Instance.playerScores[winner] = 1;

            CurrentPlayerKeys.Instance.lastWinner = winner;

            StartCoroutine(countDown());
        }
    }

    private IEnumerator countDown()
    {
        gameOver.text = "GAME OVER";
        yield return new WaitForSeconds(3);

        Application.LoadLevel("EndScene");
    }

    private List<string> mPlayersActive;
}
