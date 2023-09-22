using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EscapeHeliUi : MonoBehaviourPun
{
    public HeliPad heliPad;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EscapeButton()
    {        
        heliPad.TakeHeli();
    }

    public void CancleButton()
    {
        gameObject.SetActive(false);
    }
}
