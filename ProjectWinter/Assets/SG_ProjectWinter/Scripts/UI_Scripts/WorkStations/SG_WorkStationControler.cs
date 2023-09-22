using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG_WorkStationControler : MonoBehaviour
{
    [SerializeField]
    private GameObject WorkStationObjs;

    private bool isOpen = false;

    private SG_PlayerActionControler playerActionClass;

    private Transform topParentTrans;

    public SG_Inventory playerInventory;   // 아이템 제작시 아이템계산,제작여부 확인할때 사용할 플레이어의 인벤토리 -> 이벤트의 매개변수로 제작대를 오픈한 플레이어의 인벤토리를 받아옴                   

    private void Start()
    {
        FirstInIt();
    }

    private void FirstInIt()
    {
        WorkStationObjs.SetActive(false);
        playerActionClass = FindAnyObjectByType<SG_PlayerActionControler>();
        SerchTopParentTrans();  //최상위 부모를 찾아주는 함수
        EventSubscriber();      // 이벤트 구독하는 함수

    }
    private void SerchTopParentTrans()  //최상위 부모를 찾아주는 함수
    {
        topParentTrans = transform;

        while (topParentTrans.parent != null)
        {
            topParentTrans = topParentTrans.parent;
        }
    }

    private void EventSubscriber()  // PlayerAction Script속에 Open Event 구독
    {
        playerActionClass.WorkStationOpenEvent += WorkStationInvenController;
    }

    public void WorkStationInvenController()    // 이벤트 발생시 불려질 함수
    {        
        if (isOpen == false)
        {
            OpenWorkStation();
        }
        else if (isOpen == true)
        {
            CloseWorkStation();
        }
    }

    private void OpenWorkStation()
    {
        isOpen = true;
        WorkStationObjs.SetActive(true);
        playerActionClass.tossInventoryEvent += GetPlayerInventory;
    }
    private void CloseWorkStation()
    {
        isOpen = false;
        playerActionClass.tossInventoryEvent -= GetPlayerInventory;
        WorkStationObjs.SetActive(false);
    }

    private void GetPlayerInventory(SG_Inventory _playerInventory)
    {
        // 23.09.22 Inventory 매개변수를 잘받는것을 확인
        //Debug.Log("함수 잘 call 되는가?");
        //Debug.LogFormat("받아온 매개변수 이름 -> {0}", _playerInventory.name);

        playerInventory = _playerInventory;

    }


}
