using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GaugeUi : MonoBehaviourPun
{
    private GameObject player;
    private PlayerHealth playerHealth;
    public Image health_full;
    public Image cold_full;
    public Image hunger_full;

    public float currentValue;
    private void Start()
    {
        player = transform.parent.gameObject;
        if(!player.GetComponent<PhotonView>().IsMine)
        {
            gameObject.SetActive(false);
        }
        playerHealth = player.GetComponent<PlayerHealth>();
    }
    private void Update()
    {
        health_full.fillAmount = playerHealth.health / 100;
        cold_full.fillAmount = playerHealth.cold / 100;
        hunger_full.fillAmount = playerHealth.hunger / 100;
    }
}
