﻿using UnityEngine;
using System.Collections.Generic;

public class CurrentPlayerKeys : Singleton<CurrentPlayerKeys> {
	protected CurrentPlayerKeys () {} // guarantee this will be always a singleton only - can't use the constructor!

    public List<KeyCode> playerKeys = new List<KeyCode>();
    //public Dictionary<string, KeyCode> playerKeys = new Dictionary<string, KeyCode>();
    public List<Color> possibleColors = new List<Color>();
    public Stack<int> colourNumbers = new Stack<int>();
	public List<Color> playerColors = new List<Color>();
    public Dictionary<string, int> playerScores = new Dictionary<string, int>();
    public string lastWinner = "";
}