﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class WinnerChecker : MonoBehaviour {
    public Text gameOver;
    public Text distanceText;
    public Text pauseText;

	// Use this for initialization
	void Start () {
        mPlayersActive = new List<string>();
        foreach (KeyValuePair<KeyCode, Color> player in CurrentPlayerKeys.Instance.players){
            mPlayersActive.Add(player.Key.ToString());
        }
        mEnd = false;
        mIsPaused = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.LoadLevel("JoinScene");
		}
        if(Input.GetKeyDown(KeyCode.Pause)){
            if(!mIsPaused){
                pauseText.text = "PAUSED";
                mIsPaused = true;
                Time.timeScale = 0;
            }
            else{
                pauseText.text = "";
                mIsPaused = false;
                Time.timeScale = 1;
            }

        }
        //distanceText.text = "Distance travelled: " + (int)Camera.main.transform.position.x + "m";
        distanceText.text = (int)Camera.main.transform.position.x + "m";
	}

    public void addPlayer(string _player){
        mPlayersActive.Add(_player);
    }

    public void removePlayer(string _player, PlayerMovement _pm){
        if(mEnd)
            return;

        Debug.Log("Alive players: " + mPlayersActive.Count);

        mPlayersActive.Remove(_player);

        Dictionary<string, int> bunnies_killed = CurrentPlayerKeys.Instance.bunnies_killed;
        Dictionary<string, float> distance_traveled = CurrentPlayerKeys.Instance.distance_traveled;
        Dictionary<string, int> pots_smashed = CurrentPlayerKeys.Instance.pots_smashed;
        Dictionary<string, int> powerups_acquired = CurrentPlayerKeys.Instance.powerups_acquired;

        if(bunnies_killed.ContainsKey(_player)) {
            bunnies_killed[_player] += _pm.bunniesMurdered();
        }
        else bunnies_killed[_player] = _pm.bunniesMurdered();

        if(distance_traveled.ContainsKey(_player)) {
            distance_traveled[_player] += _pm.transform.position.x;
        }
        else distance_traveled[_player] = _pm.transform.position.x;

        if(pots_smashed.ContainsKey(_player)) {
            pots_smashed[_player] += _pm.potsSmashed();
        }
        else pots_smashed[_player] = _pm.potsSmashed();

        if(powerups_acquired.ContainsKey(_player)) {
            powerups_acquired[_player] += _pm.powerupsAcquired();
        }
        else powerups_acquired[_player] = _pm.powerupsAcquired();

        if (mPlayersActive.Count <= 1){
            string winner;
            if (mPlayersActive.Count == 0)
                winner = _player;
            else winner = mPlayersActive[0];

            Dictionary<string, int> scoreBook = CurrentPlayerKeys.Instance.playerScores;
            if(scoreBook.ContainsKey(winner)) {
                scoreBook[winner]++;
            }
            else scoreBook[winner] = 1;

            CurrentPlayerKeys.Instance.lastWinner = winner;

            LevelSounds.inst.playWinSound(transform.position);
            //audio.PlayOneShot(SoundManager.instance.winSound, SoundManager.instance.winVolume);

            StartCoroutine(countDown());
            mEnd = true;
        }
    }

    public int getNumPlayersActive(){
        return mPlayersActive.Count;
    }

    private IEnumerator countDown()
    {
        GameObject player = GameObject.Find(CurrentPlayerKeys.Instance.lastWinner);
        string name = CurrentPlayerKeys.Instance.lastWinner.Contains("Arrow") ? CurrentPlayerKeys.Instance.lastWinner.Substring(0, CurrentPlayerKeys.Instance.lastWinner.Length - 5) : CurrentPlayerKeys.Instance.lastWinner;
        gameOver.text = "PLAYER " + name + " WINS";
        gameOver.color = player.GetComponent<PlayerMovement>().playerColour;
        yield return new WaitForSeconds(2);

        Application.LoadLevel("EndScene");
    }

    private List<string> mPlayersActive;
    private bool mEnd;
    private bool mIsPaused;
}
