﻿using UnityEngine;
using System.Collections.Generic;

public class CurrentPlayerKeys : Singleton<CurrentPlayerKeys> {
	protected CurrentPlayerKeys () {} // guarantee this will be always a singleton only - can't use the constructor!

    //public List<KeyCode> playerKeys = new List<KeyCode>();
    //public Dictionary<string, KeyCode> playerKeys = new Dictionary<string, KeyCode>();
    public List<Color> possibleColors = new List<Color>();
    public Stack<int> colourNumbers = new Stack<int>();
	//public List<Color> playerColors = new List<Color>();
    public Dictionary<string, int> playerScores = new Dictionary<string, int>();
    public string lastWinner = "";
    public List<KeyValuePair<KeyCode, Color>> players = new List<KeyValuePair<KeyCode, Color>>();

    public void removePlayer(KeyValuePair<KeyCode, Color> _playerInf) {
        List<KeyValuePair<KeyCode, Color>> newPlayers = new List<KeyValuePair<KeyCode, Color>>();

        foreach(KeyValuePair<KeyCode, Color> player in players) {
            if(player.Key != _playerInf.Key)
                newPlayers.Add(player);
        }

        players = newPlayers;
    }

    public bool playerExists(KeyCode _playerKey) {
        foreach(KeyValuePair<KeyCode, Color> player in players) {
            if(player.Key == _playerKey)
                return true;
        }

        return false;
    }
}