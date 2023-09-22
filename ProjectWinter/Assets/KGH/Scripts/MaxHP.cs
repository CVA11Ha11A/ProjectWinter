using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MaxHP : MonoBehaviour
{
    private GameObject player;
    private PlayerHealth playerHealth;

    private Image GaugeBar;
    // Start is called before the first frame update
    void Start()
    {
        GameObject currentObject = this.gameObject;

        GaugeBar = GetComponent<Image>();

        while (currentObject.transform.parent != null)
        {
            currentObject = currentObject.transform.parent.gameObject;
        } // 최상위 부모 오브젝트를 currentObject에 찾아서 저장.

        player = currentObject;
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        GaugeBar.fillAmount = playerHealth.maxHP / 100;
    }
}
