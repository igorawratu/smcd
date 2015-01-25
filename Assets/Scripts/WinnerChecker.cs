using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class WinnerChecker : MonoBehaviour {
    public Text gameOver;

	// Use this for initialization
	void Start () {
        mPlayersActive = new List<string>();
        foreach (KeyCode player in CurrentPlayerKeys.Instance.playerKeys){
            mPlayersActive.Add(player.ToString());
        }
        end = false;
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void removePlayer(string _player){
        if (end)
            return;

        mPlayersActive.Remove(_player);
        if (mPlayersActive.Count < 1){
            string winner;
            if (mPlayersActive.Count == 0)
                winner = _player;
            else winner = mPlayersActive[0];
            Dictionary<string, int> scoreBook = CurrentPlayerKeys.Instance.playerScores;
            if (CurrentPlayerKeys.Instance.playerScores.ContainsKey(winner)){
                CurrentPlayerKeys.Instance.playerScores[winner]++;
            }
            else CurrentPlayerKeys.Instance.playerScores[winner] = 1;

            CurrentPlayerKeys.Instance.lastWinner = winner;
            
            audio.PlayOneShot(SoundManager.soundManager.winSound, SoundManager.soundManager.winSoundLevel);

            StartCoroutine(countDown());
            end = true;
        }
    }

    private IEnumerator countDown()
    {
        GameObject player = GameObject.Find(CurrentPlayerKeys.Instance.lastWinner);
        gameOver.text = "PLAYER " + CurrentPlayerKeys.Instance.lastWinner + " WINS";
        gameOver.color = player.GetComponent<PlayerMovement>().playerColour;
        yield return new WaitForSeconds(2);

        Application.LoadLevel("EndScene");
    }

    private List<string> mPlayersActive;
    private bool end;
}
