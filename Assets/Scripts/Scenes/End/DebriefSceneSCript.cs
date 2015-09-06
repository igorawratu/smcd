using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DebriefSceneSCript : MonoBehaviour {
    private enum StatType
    {
        winner,
        bunnies,
        distance,
        pots,
        powerups,
        jumps
    }

    public GameObject PointElementPrefab;
    public GameObject PointElPanel;
    public Sprite gold;
    public Sprite silver;
    public Sprite bronze;
    public Sprite distance;
    public Sprite jumps;
    public Sprite powerups;
    public Sprite pots;
    public Sprite bunnies;
    public GameObject banner;

    private List<Sprite> header_icons;

    private List<StatType> stat_type_order;
    private int curr_stat_type = 0;
    private int curr_icon = 0;

	// Use this for initialization
	void Start () {
        stat_type_order = new List<StatType>(new StatType[] { StatType.winner, StatType.distance, StatType.jumps,
            StatType.powerups, StatType.pots, StatType.bunnies});

        header_icons = new List<Sprite>(new Sprite[] {gold, distance, jumps, powerups, pots, bunnies });

        populateCanvas();
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    private void displayFloatDat(string title, Dictionary<string, float> dat)
    {
        GameObject title_obj = banner.transform.Find("title").gameObject;
        Text text = title_obj.GetComponent<Text>();
        text.text = title;
        GameObject icon_backing_obj = title_obj.transform.Find("iconBacking").gameObject;
        GameObject icon_obj = icon_backing_obj.transform.Find("icon").gameObject;
        Image img = icon_obj.GetComponent<Image>();
        img.sprite = header_icons[curr_icon++];

        foreach (Transform child in PointElPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (KeyValuePair<string, float> player in dat)
        {
            GameObject player_score = (GameObject)Instantiate(PointElementPrefab);
            player_score.transform.parent = PointElPanel.transform;
            GameObject offset = player_score.transform.Find("offset").gameObject;
            GameObject points = offset.transform.Find("points").gameObject;
            Text pt_text = points.GetComponent<Text>();
            pt_text.text = player.Value.ToString();
            GameObject player_text = player_score.transform.Find("player_text").gameObject;
            Text plyr_text = player_text.GetComponent<Text>();
            plyr_text.text = player.Key;
        }
    }

    private void displayIntDat(string title, Dictionary<string, int> dat)
    {
        GameObject title_obj = banner.transform.Find("title").gameObject;
        Text text = title_obj.GetComponent<Text>();
        text.text = title;
        GameObject icon_backing_obj = title_obj.transform.Find("iconBacking").gameObject;
        GameObject icon_obj = icon_backing_obj.transform.Find("icon").gameObject;
        Image img = icon_obj.GetComponent<Image>();
        img.sprite = header_icons[curr_icon++];

        foreach (Transform child in PointElPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (KeyValuePair<string, int> player in dat)
        {
            GameObject player_score = (GameObject)Instantiate(PointElementPrefab);
            player_score.transform.parent = PointElPanel.transform;
            GameObject offset = player_score.transform.Find("offset").gameObject;
            GameObject points = offset.transform.Find("points").gameObject;
            Text pt_text = points.GetComponent<Text>();
            pt_text.text = player.Value.ToString();
            GameObject player_text = player_score.transform.Find("player_text").gameObject;
            Text plyr_text = player_text.GetComponent<Text>();
            plyr_text.text = player.Key;
        }
    }

    private void displayWinners() {

    }

    private void populateCanvas(){
        if (curr_stat_type >= stat_type_order.Count)
        {
            Application.LoadLevel("JoinScene");
        }
        else
        {
            StatType stat_type = stat_type_order[curr_stat_type++];
            switch (stat_type)
            {
                case StatType.winner:
                    displayIntDat("Wins", CurrentPlayerKeys.Instance.playerScores);
                    break;
                case StatType.distance:
                    displayFloatDat("Distance", CurrentPlayerKeys.Instance.distance_traveled);
                    break;
                case StatType.bunnies:
                    displayIntDat("Bunnies", CurrentPlayerKeys.Instance.bunnies_killed);
                    break;
                case StatType.jumps:
                    displayIntDat("Jumps", CurrentPlayerKeys.Instance.num_jumps);
                    break;
                case StatType.pots:
                    displayIntDat("Pots", CurrentPlayerKeys.Instance.pots_smashed);
                    break;
                case StatType.powerups:
                    displayIntDat("Powerups", CurrentPlayerKeys.Instance.powerups_acquired);
                    break;
                default:
                    Debug.Log("WEEEEE I SHOULD NOT BE HERE");
                    break;
            }
            Invoke("populateCanvas", 3);
        }
    }
}
