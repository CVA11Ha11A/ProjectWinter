using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UiFollowPlayer : MonoBehaviour
{
    public Transform player;

    public Image LoadingBar;
    public float currentValue;
    

    private void Start()
    {
        LoadingBar = this.GetComponent<Image>();
    }
    private void Update()
    {
        if (player != null)
        {
            transform.position = player.position + new Vector3(0, 2.2f, 0); 
        }        
    }

    public void Gauge(float speed)
    {
        if (currentValue < 100)
        {
            currentValue += speed * Time.deltaTime;
        }
        LoadingBar.fillAmount = currentValue / 100;
    }
}
