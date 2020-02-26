using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    static int allCoins = 0;

    public static void setScore(int value)
    {
        allCoins += value;
    }
    public static int getScore()
    {
        return allCoins;
    }

    void OnGUI()
    {
        GUILayout.Label("Coins: " + allCoins.ToString());
    }
}
