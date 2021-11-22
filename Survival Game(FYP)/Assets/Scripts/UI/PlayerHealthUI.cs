using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField]
    private Text playerName;
    [SerializeField]
    private Text currentHealthAmount;

    public GameObject ui;

    Image healthSlider;

    PlayerManager playerManager;

    PlayerStat player;

    private void Start()
    {
        playerManager = PlayerManager.instance;
        healthSlider = ui.transform.GetChild(0).GetComponent<Image>();
        player = playerManager.player.GetComponent<PlayerStat>();
        player.OnHealthChanged += OnHealthChanged;
    }

    void OnHealthChanged(int maxHealth, int currentHealth)
    {
        if (ui != null)
        {
            playerName.text = player.gameObject.name;
            currentHealthAmount.text = "" + currentHealth;
            float healthPercent = (float)currentHealth / maxHealth;
            healthSlider.fillAmount = healthPercent;
        }
    }
}
