using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Stealth : MonoBehaviour {

    public Image threatIndic;
    public Text scoreText;

    static int killCount = 0;
    static float threatMeter = 0.0f;
    static float threatIncr = 0.1f;
    static float threatCap = 25f;

    public static void IncreaseThreat() {
        Stealth.threatMeter += 2*Stealth.threatIncr;
        if (Stealth.threatMeter >= Stealth.threatCap)
            LoseGame();
    }

    public static void AddKill() {
        Stealth.killCount++;    
        if (Stealth.killCount >= 7)
            Stealth.WinGame();
    }

    private void Update() {
        UpdateIndicColor();
        UpdateScore();
        if (Stealth.threatMeter>0)
            Stealth.threatMeter -= (Stealth.threatIncr / 5);
    }

    void UpdateIndicColor() {
        Color c = threatIndic.color;
        c.a = (Stealth.threatMeter / Stealth.threatCap);
        threatIndic.color = c;
    }

    static void WinGame() {
        //killCount = 0;
        //threatMeter = 0.0f;
        //SceneManager.LoadScene(3);
    }

    static void LoseGame() {
        //killCount = 0;
        //threatMeter = 0.0f;
        //SceneManager.LoadScene(2);
    }

    void UpdateScore() {
        scoreText.text = string.Format("{0}/7", Stealth.killCount);
    }
}
