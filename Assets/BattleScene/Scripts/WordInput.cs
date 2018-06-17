using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordInput : MonoBehaviour
{
    public BattleManager BattleManager;
    public DateTime timeStart = DateTime.Now;
    public double typedKeys = 0;
    public double mean = 0;
    public double milisec = 0;

    // Update is called once per frame
    void Update () {
	    foreach (var letter in Input.inputString)
	    {
	        BattleManager.TypeLetter(letter);
	    }
        GameManager.instance.milisec += DateTime.Now.Subtract(timeStart).Milliseconds;
        milisec += DateTime.Now.Subtract(timeStart).Milliseconds;
        timeStart = DateTime.Now;
        double kps = GameManager.instance.typedKeys / (GameManager.instance.milisec / 60000); // 60 sekun * 1000 milisekund = 1 min
        GameManager.instance.meanKPM = kps;
       EmotionMenager.GetInstance().SatisfactionFromKPS(kps);
        //Debug.Log("mili " + GameManager.instance.milisec);
        //Debug.Log("KPS " + kps);
    }
}
