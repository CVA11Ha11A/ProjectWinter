using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG_PowerStationControler : MonoBehaviour
{

    [SerializeField]
    private GameObject poweStationObjs;

    private bool isOpen = false;
        
    SG_PlayerActionControler playerActionClass;

    Transform topParentTrans;

    private void Start()
    {
        FirstInIt();
    }

    private void FirstInIt()
    {
        poweStationObjs.SetActive(false);
        playerActionClass = FindAnyObjectByType<SG_PlayerActionControler>();
        SerchTopParentTrans();  //최상위 부모를 찾아주는 함수
        EventSubscriber();  // 최상위 부모에 따라 구독하는 이벤트가 정해지는 함수

    }
    private void SerchTopParentTrans()  //최상위 부모를 찾아주는 함수
    {
        topParentTrans = transform;

        while(topParentTrans.parent != null)
        {
            topParentTrans = topParentTrans.parent;
        }
    }

    private void EventSubscriber()
    {
        if(topParentTrans.CompareTag("PowerStation"))
        {
            playerActionClass.PowerStationInventoryEvent += PoweStationInvenController;
        }
        else if(topParentTrans.CompareTag("HeliPad"))
        {
            playerActionClass.HeliPadInventoryEvent += PoweStationInvenController;
        }
    }

    public void PoweStationInvenController()
    {
        //Debug.Log("이벤트로 발전기 여는 함수 조건이 잘들어와지나");
        if (isOpen == false)
        {
            OpenPoweStation();
        }
        else if (isOpen == true)
        {
            ClosePoweStation();
        }
    }

    private void OpenPoweStation()
    {
        isOpen = true;
        poweStationObjs.SetActive(true);
     
    }
    private void ClosePoweStation()
    {
        isOpen = false;
        poweStationObjs.SetActive(false);
    }



}
