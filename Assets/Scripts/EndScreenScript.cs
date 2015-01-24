using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EndScreenScript : MonoBehaviour {
    public Text title;
    public Text winner;
    public Text resetTimer;
    public Text record;


	// Use this for initialization
	void Start () {
	    Dictionary<string, int> scoreBoard = new Dictionary<string, int>();
        title.text = "GAME OVER:";
        string win = "x";
        winner.text = "PLAYER " + win + " WINS";
        resetTimer.text = "PLAY AGAIN? Y/N";
        record.text = "WINS:\n";

        foreach (KeyValuePair<string, int> player in scoreBoard){
            record.text += "PLAYER " + player.Key + ": " + player.Value;
        }
        mBusy = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!mBusy){
            if (Input.GetKey("n"))
                Application.Quit();
            else if (Input.GetKey("y"))
            {
                StartCoroutine(countDown());
            }
        }
	}

    private IEnumerator countDown()
    {
        for (int i = 10; i >= 0; i--){
            resetTimer.text = "RESTARTING IN " + i;
            yield return new WaitForSeconds(1);
        }

        Application.LoadLevel("TestScene");
    }

    bool mBusy;
}
