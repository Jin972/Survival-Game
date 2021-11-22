using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Database", menuName = "Save/Player Data")]
public class GameData : ScriptableObject
{
    public int health;
    public float thirst;
    public float hunger;
    public float[] position;

    

    //public GameData(PlayerStat player)
    //{
    //    if (player != null)
    //    {
    //        health = player.currentHealth;
    //        thirst = player.thirst;
    //        hunger = player.hunger;

    //        position = new float[3];
    //        position[0] = player.transform.position.x;
    //        position[1] = player.transform.position.y;
    //        position[2] = player.transform.position.z;
    //    }      
    //}

}

