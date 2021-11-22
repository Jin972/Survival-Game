using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialManager : MonoBehaviour
{
    #region Singleton

    public static EssentialManager instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion
    public delegate void OnEssentialChanged();
    public OnEssentialChanged onEssentialChangedCallback;

    private PlayerStat player;
    PlayerManager playerManager;



    // Start is called before the first frame update
    void Start()
    {
        playerManager = PlayerManager.instance;
        player = playerManager.player.GetComponent<PlayerStat>();
        //player = GameObject.FindWithTag("Player");
    }

    public void UserEssential(Essential value)
    {
        if (value.essentialType == EssentialType.Water)
        {
            player.Drink(value.decreaseRate);
        }
        else if (value.essentialType == EssentialType.Food)
        {
            player.Eat(value.decreaseRate);
        }
        else if (value.essentialType == EssentialType.Heath)
        {
            player.Healing(value.decreaseRate);
        }

        if (onEssentialChangedCallback != null)
            onEssentialChangedCallback.Invoke();
    }
}
