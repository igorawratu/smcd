using UnityEngine;
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
        Dictionary<string, int> num_jumps = CurrentPlayerKeys.Instance.num_jumps;

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

        if(num_jumps.ContainsKey(_player)) {
            num_jumps[_player] += _pm.numJumps();
        }
        else num_jumps[_player] = _pm.numJumps();

        if (mPlayersActive.Count <= 1){
            string winner;
            if(mPlayersActive.Count == 0) {
                winner = _player;
            }
            else {
                winner = mPlayersActive[0];
                GameObject winner_obj = GameObject.Find(winner);
                PlayerMovement pm = winner_obj.GetComponent<PlayerMovement>();

                if (bunnies_killed.ContainsKey(winner))
                {
                    bunnies_killed[winner] += pm.bunniesMurdered();
                }
                else bunnies_killed[winner] = pm.bunniesMurdered();

                if (distance_traveled.ContainsKey(winner))
                {
                    distance_traveled[winner] += pm.transform.position.x;
                }
                else distance_traveled[winner] = pm.transform.position.x;

                if (pots_smashed.ContainsKey(winner))
                {
                    pots_smashed[winner] += pm.potsSmashed();
                }
                else pots_smashed[winner] = pm.potsSmashed();

                if (powerups_acquired.ContainsKey(winner))
                {
                    powerups_acquired[winner] += pm.powerupsAcquired();
                }
                else powerups_acquired[winner] = pm.powerupsAcquired();

                if (num_jumps.ContainsKey(winner))
                {
                    num_jumps[winner] += pm.numJumps();
                }
                else num_jumps[winner] = pm.numJumps();
            }

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

        Debug.Log(bunnies_killed[_player]);
        Debug.Log(distance_traveled[_player]);
        Debug.Log(pots_smashed[_player]);
        Debug.Log(powerups_acquired[_player]);
        Debug.Log(num_jumps[_player]);
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

        LevelTypeManager.loadLevel();
    }

    private List<string> mPlayersActive;
    private bool mEnd;
    private bool mIsPaused;
}
