using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : MonoBehaviour
{
    [SerializeField]
    private Transform healthBar;

    [SerializeField]
    private Text playerName;
    [SerializeField]
    private Text currentHealthAmount;

    [SerializeField]
    private Text currentThirsthAmount;
    [SerializeField]
    private Text currentHungerhAmount;
    [SerializeField]
    private Text attackIndex;
    [SerializeField]
    private Text defenseIndex;
    [SerializeField]
    private Text speedIndex;

    Image healthSlider;

    PlayerManager playerManager;

    PlayerStat player;

    //PlayerMotor motor;

    EquipmentManager equipment;

    ThirdPersonController thirdPlayer;

    private void Start()
    {
        playerManager = PlayerManager.instance;
        healthSlider = healthBar.transform.GetChild(0).GetComponent<Image>();
        player = playerManager.player.GetComponent<PlayerStat>();
        //motor = playerManager.player.GetComponent<PlayerMotor>();
        equipment = EquipmentManager.instance;

        thirdPlayer = playerManager.player.GetComponent<ThirdPersonController>();

        player.Load();
        player.OnHealthChanged += OnHealthChanged;
    }

    void OnHealthChanged(int maxHealth, int currentHealth)
    {
            playerName.text = player.gameObject.name;
            currentHealthAmount.text = "" + currentHealth;
            float healthPercent = (float)currentHealth / maxHealth;
            healthSlider.fillAmount = healthPercent;
    }

    private void Update()
    {
        UpdateStatus();
    }

    void UpdateStatus()
    {
        float temp = player.thirst;
        int curThirst = (int)temp;
        currentThirsthAmount.text = "" + curThirst;

        float tempHunger = player.hunger;
        int curHunger = (int)tempHunger;
        currentHungerhAmount.text = "" + curHunger;

        float tempDamge = player.damage.GetValue();
        int curDamge = (int)tempDamge;
        attackIndex.text = "" + curDamge;

        float tempArmor = player.armor.GetValue();
        int curArmor = (int)tempArmor;
        defenseIndex.text = "" + curArmor;

        //float tempSpeed = motor.agent.speed;
        //int curSpeed = (int)tempSpeed;
        float curSpeed = thirdPlayer.SprintSpeed;
        speedIndex.text = "" + curSpeed;
    }
}
