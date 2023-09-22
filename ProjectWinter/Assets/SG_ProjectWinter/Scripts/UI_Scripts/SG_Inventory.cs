using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using static UnityEditor.Progress;

public class SG_Inventory : MonoBehaviour
{
    // 인벤토리가 켜져있을때에 다른 기능을 막기위해 사용되는 bool 변수
    // 플레이어와 많은 친구들이 사용해야하기에 static으로 선언
    // 하지만 인벤토리가 ProjcetWinter 는 인벤토리가 켜지고 다른 행동이 제약이 되야할때는
    // 크래프팅,창고열때,발전기같은곳에 재료를 넣을때에이기에 추후 바뀔수도 있음 23.09.07
    public static bool inventoryActicated = false;


    // 필요한 컴포넌트

    // 인벤토리 의 베이스 (슬롯의 부모객체)
    [SerializeField]
    private GameObject inventoryBase;
    // 슬롯들의 부모객체
    [SerializeField]
    private GameObject slotsParent;

    // Swap에서 slots 를 뒤져봐야하기 떄문에 public
    public SG_ItemSlot[] slots;

    // 인벤토리에서 주웠는지 확인후 PlayerAction Class에있는 Distroy실행시키는 방향으로만들기

    // 아이템 Distroy 이벤트를 위한 델리게이트 선언
    public delegate void ItemDestroyDelegate();

    // event를 델리게이트를 지정해서 델리게이트가 가지고 있는 void 의 리턴값과 매개변수가 없어야한다는 조건을넣어준샘이됨
    public event ItemDestroyDelegate ItemDestroyEvent;

    // 아래 변수의 태그로 플레이어인지 창고인지 구별할것임
    GameObject topParentObj;

    private int missionClearCount = 0;

    private void Awake()
    {

    }

    void Start()
    {
        slots = slotsParent.GetComponentsInChildren<SG_ItemSlot>();
        topParentObj = this.transform.parent.gameObject;
        // topParentObj변수에 최상위 게임오브젝트를 집어넣음 
        GetThisTopParentObj();
        // 슬롯의 고유번호 넣어주는 함수
        InItSlotCount();

    }
    void Update()
    {
        //TryOpenInventory();
    }

    /*LEGACY

    // { ProjectWiter 게임에는 필요하지 않는 기능
    // I버튼은 눌렀을때에 인벤토리 열리는 기능
    // ProjectWinter에 이 기능은 필요없을수도 있음
    private void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryActicated = !inventoryActicated;

            if (inventoryActicated)
            {
                OpenInventory();
            }
            else
            {
                CloseInventory();
            }
        }
    }



    private void OpenInventory()
    {
        inventoryBase.SetActive(true);
    }

    private void CloseInventory()
    {
        inventoryBase.SetActive(false);
    }
    // } ProjectWiter 게임에는 필요하지 않는 기능

    LEGACY*/

    // { AcquireItem()
    public void AcquireItem(SG_Item _item, int _count = 1)
    {
        // 아이템을 주우면 들어오는함수이지만 플레이어가 아니라면 바로 나가게 만듦
        if(!topParentObj.CompareTag("Player"))
        {
            return;
        }

        // 얻은 아이템의 ItemType이 Weapon 이 아닐경우에만 갯수 증가 로직 도는 조건
        if (SG_Item.ItemType.Weapon != _item.itemType)
        {
            // 아이템을 한번 슥 훑어보고 같은 아이템이 있다면 아이템의 갯수증가
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.itemName == _item.itemName)
                    {
                        if (slots[i].itemCount == 3)
                        {
                            //Debug.Log("3개 이상은 주울수 없음");
                            continue;
                        }
                        slots[i].SetSlotCount(_count);
                        // TODO : Distroy 먹은 아이템을 Distroy 하도록 추가해야함
                        ItemDestroyEventShot();
                        return;
                    }
                }
            }
        }
        else { /*PASS*/ }

        // 인벤토리에 같은 아이템이 없다면 아이템 추가
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(_item, _count);
                ItemDestroyEventShot();
                return;
            }
            else { /*Debug.Log("인벤토리에 빈공간이 없음");*/ }
        }
    }   // } AcquireItem()


    public void ItemDestroyEventShot()
    {
        // 이 함수를 부르면 ItemDistroyEvent를 구독하고 있는 모든 함수를 호출함
        ItemDestroyEvent?.Invoke();
    }


    private void GetThisTopParentObj()
    {
        //최상위 부모 오브젝트 태그를 가져오기 위해 찾는 로직
        while (topParentObj.transform.parent != null)
        {
            topParentObj = topParentObj.transform.parent.gameObject;
        }
    }

    private void InItSlotCount()
    {
        // 플레이어 인벤토리 일때에 플레이어의 고유번호 넣어줌
        if (topParentObj.CompareTag("Player"))
        {
            // 처음에 시작하면 슬롯에 고유번호 넣어줌    10 ~ 13
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].slotCount = i + 10;
            }
        }
        // 산장창고 인벤토리 일때에 산장창고의 고유번호 넣어줌 100 ~ 115 
        else if (topParentObj.CompareTag("Warehouse"))
        {
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].slotCount = i + 100;
            }
        }
        // 발전기 인벤토리 일때에 발전기의 고유번호 넣어줌   120 ~ 125   (1~2개 랜덤 생성이기에 121 없을수도 있음)
        else if (topParentObj.CompareTag("PowerStation"))
        {
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].slotCount = i + 120;
            }
        }
        // 헬리패드 인벤토리 일떄에 헬리패드의 고유번호 넣어줌 130 ~ 135 -> 헬리패드는 하나만 존재
        else if (topParentObj.CompareTag("HeliPad"))
        {
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].slotCount = i + 130;
            }
        }
        else { /*PASS*/ }
        // 발전기 고유번호 삽입 해줄 else if
        //else if(topParentObj.CompareTag(""))
        //{

        //}
    }


    // 아이템을 받아 왔을때에 슬롯을 확인해줄 함수
    // 23.09.14 지금 건들일 함수가 아닌거같음 나중에 틀을 잡고 수정할것
    // { AcquireItem()
    public void AcquireMoveItem(SG_Item _item, int _count)
    {
        // 얻은 아이템의 ItemType이 Weapon 이 아닐경우에만 갯수 증가 로직 도는 조건
        if (SG_Item.ItemType.Weapon != _item.itemType)
        {
            // 아이템을 한번 슥 훑어보고 같은 아이템이 있다면 아이템의 갯수증가
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.itemName == _item.itemName)
                    {
                        if (slots[i].itemCount == 3)
                        {
                            //Debug.Log("3개 이상은 주울수 없음");
                            continue;
                        }
                        slots[i].SetSlotCount(_count);
                        // TODO : Distroy 먹은 아이템을 Distroy 하도록 추가해야함
                        ItemDestroyEventShot();
                        return;
                    }
                }
            }
        }
        else { /*PASS*/ }

        // 인벤토리에 같은 아이템이 없다면 아이템 추가
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(_item, _count);
                ItemDestroyEventShot();
                return;
            }
            else { /*Debug.Log("인벤토리에 빈공간이 없음");*/ }
        }
    }   // } AcquireItem()


    public void CheckClearPowerStation()
    {
        if(topParentObj.CompareTag("PowerStation"))
        {
            missionClearCount++;
            Debug.Log("슬롯이 만족하고 함수를 불렀는지");
            Debug.LogFormat("클리어된 슬롯 카운터 -> {0} 총 슬롯 갯수 -> {1}", missionClearCount, slots.Length);
            if (slots.Length == missionClearCount)
            {
                GameManager.instance.RepairPowerStation();
            }
            else { /*PASS*/ }
        }
        else { /*PASS*/ }
    }

    public void CheckClearHeliPad()
    {
        if (topParentObj.CompareTag("HeliPad"))
        {
            missionClearCount++;
            if (slots.Length == missionClearCount)
            {
                GameManager.instance.RepairHelipad();
            }
            else { /*PASS*/ }
        }
        else { /*PASS*/ }
    }

}   //NAMESPACE
