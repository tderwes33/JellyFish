using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int level;
    public int health;
    public int best;
    public int easybest;
    public int mediumbest;
    public int hardbest;
    /* Constructor */
    public PlayerData(force forceObject)
    {
        level = force.level;
        health = force.health;
        best = force.previousBestLevel;
        easybest = force.previousEasyBestLevel;
        mediumbest = force.previousMediumBestLevel;
        hardbest = force.previousHardBestLevel;
    }
}
