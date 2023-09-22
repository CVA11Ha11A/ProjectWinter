using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG_WareHouseController : MonoBehaviour
{
    [SerializeField]
    private GameObject warehouseObjs;

    private bool isOpen = false;

    SG_PlayerActionControler playerActionClass;

    private void Start()
    {
        warehouseObjs.SetActive(false);
        playerActionClass = FindAnyObjectByType<SG_PlayerActionControler>();
        playerActionClass.WareHouseEvent += WareHouseInvenController;
    }

    public void WareHouseInvenController()
    {
        //Debug.Log("이벤트로 창고 여는 함수 조건이 잘들어와지나");
        if (isOpen == false)
        {
            OpenWareHouse();
        }
        else if (isOpen == true)
        {
            CloseWareHouse();
        }
    }

    private void OpenWareHouse()
    {
        isOpen = true;
        warehouseObjs.SetActive(true);
    }
    private void CloseWareHouse()
    {
        isOpen = false;
        warehouseObjs.SetActive(false);
    }


}
