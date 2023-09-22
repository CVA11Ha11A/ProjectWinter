using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SG_WareHouseInventory : MonoBehaviour
{
    // 23.09.10 아래 스크립트는 플레이어의 인벤토리에 들어있는 스크립트를 Clone 딴것
    // 산장 창고에 맞게 수정해서 사용해야함

    // 23.09.10 한번 확인 해보고 주석을 달아놓았음
    // (1) : 창고를 판별할게 플레이어가 Ray를 쏘아서 확인할지 선택해야함
    // (2) : 산장창고에서 Destroy함수 수정이 필요함 플레이어의 인벤토리에서 삭제를 해주어야함
    // (3) : 창고 Open 과 Close 는 아래의 TryOpenInventory 함수를 이용해서 사용하면 될거같음

    // 인벤토리가 켜져있을때에 다른 기능을 막기위해 사용되는 bool 변수
    // 플레이어와 많은 친구들이 사용해야하기에 static으로 선언
    // 하지만 인벤토리가 ProjcetWinter 는 인벤토리가 켜지고 다른 행동이 제약이 되야할때는
    // 크래프팅,창고열때,발전기같은곳에 재료를 넣을때에이기에 추후 바뀔수도 있음 23.09.07

    public static bool inventoryActicated = false;


    // 필요한 컴포넌트

    // 인벤토리 의 베이스 (그리드의 부모객체)
    [SerializeField]
    private GameObject inventoryBase;
    // 슬롯들의 부모객체 -> 그리드
    [SerializeField]
    private GameObject slotsParent;
    [SerializeField]
    private GameObject warehouseTextObj;

    // 창고의 슬롯을 배열로 선언
    public SG_WareHouseItemSlot[] slots;

    // 인벤토리에서 주웠는지 확인후 PlayerAction Class에있는 Distroy실행시키는 방향으로만들기

    // 아이템 Distroy 이벤트를 위한 델리게이트 선언
    public delegate void ItemDestroyDelegate();

    // event를 델리게이트를 지정해서 델리게이트가 가지고 있는 void 의 리턴값과 매개변수가 없어야한다는 조건을넣어준샘이됨
    public event ItemDestroyDelegate ItemDestroyEvent;

    private Transform topParentObj;




    private void Awake()
    {

    }

    void Start()
    {

        slots = slotsParent.GetComponentsInChildren<SG_WareHouseItemSlot>();

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].wareHouseSlotCount = i + 100;
        }
    }

    void Update()
    {
        //TryOpenInventory();
    }

    // { ProjectWiter 게임에는 필요하지 않는 기능
    // I버튼은 눌렀을때에 인벤토리 열리는 기능
    // ProjectWinter에 이 기능은 필요없을수도 있음

    // 23.09.10 산장의 창고는 E키를 눌러서 열어야하기에 아래의 함수를 조건을 추가해서 잘 사용하면 용의할것으로 예상
    private void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.E))
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
        warehouseTextObj.SetActive(true);
    }

    private void CloseInventory()
    {
        inventoryBase.SetActive(false);
        warehouseTextObj.SetActive(false);
    }

    // } ProjectWiter 게임에는 필요하지 않는 기능

    // { AcquireItem()
    public void AcquireItem(SG_Item _item, int _count = 1)
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
                            Debug.Log("3개 이상은 주울수 없음");
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
                // TODO : Distroy 먹은 아이템을 Distroy 하도록 추가해야함
                ItemDestroyEventShot();
                return;
            }
            else { Debug.Log("인벤토리에 빈공간이 없음"); }
        }
    }   // } AcquireItem()

    // 창고에서의 Destroy는 떨어져있는것이아닌 플레이어의 창고에서 옮기는것이기에 아랫 부분 수정이 필요할것으로 예상
    public void ItemDestroyEventShot()
    {
        // 이 함수를 부르면 ItemDistroyEvent를 구독하고 있는 모든 함수를 호출함
        ItemDestroyEvent?.Invoke();
    }



}   //NAMESPACE
