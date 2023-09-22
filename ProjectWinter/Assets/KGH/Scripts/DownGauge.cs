using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ##################################
// 모든 유저에게 보여야됨
// ##################################

public class DownGauge : MonoBehaviour
{
    private GameObject player;
    private PlayerHealth playerHealth;

    //private Renderer objectRenderer;

    public Image GaugeBar;
    //public float currentValue;
    // Start is called before the first frame update
    void Start()
    {
        GameObject currentObject = this.gameObject;

        while (currentObject.transform.parent != null)
        {
            currentObject = currentObject.transform.parent.gameObject;
        }

        // 최상위 부모 오브젝트를 currentObject에 찾아서 저장합니다.
        player = currentObject;
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            transform.position = player.transform.position + new Vector3(0, 2.2f, 0);
        }
        GaugeBar.fillAmount = playerHealth.playerDown / 100;
    }
}
