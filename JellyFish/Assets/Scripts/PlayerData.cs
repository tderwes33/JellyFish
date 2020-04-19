using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int level;
    public int health;
    public int best;
    /* Constructor */
    public PlayerData(force forceObject)
    {
        level = force.level;
        health = force.health;
        best = force.previousBestLevel;
    }
}
