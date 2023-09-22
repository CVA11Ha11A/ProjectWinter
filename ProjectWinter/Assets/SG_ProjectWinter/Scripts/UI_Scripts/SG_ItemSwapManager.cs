using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class SG_ItemSwapManager : MonoBehaviour
{
    // 플레이어 슬롯 번호는 10을 더해서 곂치는 부분이 없도록 하였음 10 11 12 13 이런번호로 나올거임

    // 아래 함수는 매개변수를 플레이어와 창고로 받지않고 주는인벤토리 , 받는 인벤토리 나누어서 받아야할거같음

    // 플레이어 인벤토리 10 ~ 13
    // 산장창고 인벤토리 100 ~ 115

    // 받는 매개변수 accept , 주는 매개면수 give

    // 아이템을 줄떄에 갯수 검사하고 임시 변수에 담아두던지 넘겨주지 말던지 해야할거같음


    // 매개변수 정보 : 주는얘 주소, 받는얘주소 , 주는 아이템슬롯, 받는 아이템슬롯

    //SG_ItemSlot giveSlotClass;  // 아이템을 줄 슬롯의 클래스
    //SG_ItemSlot accepSlotClass; // 아이템을 받을 슬롯의 클래스


    private SG_Inventory giveInvenClass;    // 아이템을 줄 인벤토리 클래스
    private SG_Inventory acceptInvenClass;   // 아이템을 받을 인벤토리 클래스

    private SG_PowerStationItemInIt tempMissionInitClass; // Mission Slot 들은 원하는 count 가 slot속에 있는 Component가 가지고 있기에 변동적으로 사용

    //트랜스폼으로 받아야함 GameObject로 받으니 Grid에서 참조불가
    private Transform tempTrans001;
    private Transform tempTrans002;
    private SG_ItemDragScript giveItemDragScript; // 3개 이상일때 원래 아이템은 있어야하고 슬롯에 맞게 가야하기 떄문에 필요

    private SG_Item moveItem;   // 옮길 아이템 정보

    private int tempItemCount;  // 임시 아이템 갯수 저장할 변수

    private byte giveSlotCount; // 불필요한 for문을 줄이기 위해 처음 슬롯 찾을때 그 슬롯의 좌표를 저장할 byte 변수
    private byte accepSlotCount;

    private bool isPassExamine = true;  // 아이템 검사하고 검사 조건에 들어가면 false 로바꿔줄거임
    private bool isSwap = true;         // 받는 아이템에서 조건에 안맞으면 스왑 못하게 false 로 만들고 함수들을 return시켜줄거임
    private bool sameSlot = false;      // 같은 슬롯에 두었을떄는 스왑을 하지 않도록
    private bool isMissionInven = false;// Swap 받는 쪽이 미션과 연관되어있는 인벤토리 인지 확인 (발전소,헬리패드)

    public void ItemSwap(SG_ItemDragScript _itemDragScript, int _GiveSlotCount, int _AcceptSlotCount,
                         SG_ItemSlot _giveSlots, SG_ItemSlot _accepSlots)
    {
        giveItemDragScript = _itemDragScript;
        ThisSameSlot(_GiveSlotCount, _AcceptSlotCount);  // 슬롯이 같은지 확인후 다를때만 Swap이 이루어지게 만들어주는 함수


        if (sameSlot == false)
        {
            //스왑시 필요한 로직
            SearchGiveSlot(_GiveSlotCount, _giveSlots);     // 주는슬롯을 찾아서 아이템을 저장하는 함수
            SerchAccepSlots(_AcceptSlotCount, _accepSlots); // 받는 슬롯을 찾고 아이템을 조건에 따라받고 검수하는 함수로 넘겨주는 함수
            GiveExchangeAfter();  // 아이템을 정상적으로 넘겼을때에 준 슬롯을 초기화 하는 함수
            AcceptExchangeAfter();// 아이템을 정상적으로 넘겼을때 받은슬롯이 정상적으로 출력되도록 해주는 함수            
        }

        InitialValue();       // 초기값으로 돌아와야하는 변수를 초기값으로 돌려주는 함수


    }   // ItemSwap()

    // 같은 슬롯에 놓았는지 -> ex) 주는슬롯 = 1 , 받는 슬롯 = 1 일경우

    private void ThisSameSlot(int _GiveSlotCount, int _AcceptSlotCount)
    {
        if (_GiveSlotCount == _AcceptSlotCount)
        {
            sameSlot = true;
        }
        else { /*PASS*/ }
    }

    // 주는 슬롯을 찾아서 아이템을 변수에 저장하는 함수
    public void SearchGiveSlot(int _GiveSlotCount, SG_ItemSlot _giveSlots)
    {

        #region _GiveSlotCount가 플레이어일 경우
        if (_GiveSlotCount > 1 && _GiveSlotCount < 20) // 플레이어가 주는 슬롯일때에
        {
            // 이번엔 부모의 부모의 부모 3번 찾아야함 
            //1 -> Grid
            tempTrans001 = _giveSlots.transform.parent.GetComponent<Transform>();
            //2->InventoryImg
            tempTrans002 = tempTrans001.transform.parent.GetComponent<Transform>();
            //3 -> Inventory
            giveInvenClass = tempTrans002.transform.parent.GetComponent<SG_Inventory>();

            for (byte i = 0; i < giveInvenClass.slots.Length; i++)    //슬롯들을 싹 뒤지면서 주는 Slot의 고유번호 찾기
            {
                if (giveInvenClass.slots[i].slotCount == _GiveSlotCount) // 고유번호를 찾았을때 들어갈 조건문
                {
                    // 아이템 정보를 임시로 집어넣어줌
                    moveItem = giveInvenClass.slots[i].item;
                    tempItemCount = giveInvenClass.slots[i].itemCount;
                    giveSlotCount = i;
                    break;
                }
            }
            return; // 찾아서 아이템의 정보를 넣어주었다면 return으로 함수 즉시탈출
        }
        else { /*PASS*/ }
        #endregion _GiveSlotCount가 플레이어일 경우

        #region _GiveSlotCount가 산장창고일 경우
        if (_GiveSlotCount > 99 && _GiveSlotCount < 120)
        {
            // 이번엔 부모의 부모의 부모 3번 찾아야함
            //1 -> Grid
            tempTrans001 = _giveSlots.transform.parent.GetComponent<Transform>();
            //2 -> InventoryImg
            tempTrans002 = tempTrans001.transform.parent.GetComponent<Transform>();
            //3 -> Inventory
            giveInvenClass = tempTrans002.transform.parent.GetComponent<SG_Inventory>();

            for (byte i = 0; i < giveInvenClass.slots.Length; i++)    //슬롯들을 싹 뒤지면서 주는 Slot의 고유번호 찾기
            {
                if (giveInvenClass.slots[i].slotCount == _GiveSlotCount) // 고유번호를 찾았을때 들어갈 조건문
                {
                    // 아이템 정보를 임시로 집어넣어줌
                    moveItem = giveInvenClass.slots[i].item;
                    tempItemCount = giveInvenClass.slots[i].itemCount;
                    giveSlotCount = i;
                    break;
                }
            }
            return; // 찾아서 아이템의 정보를 넣어주었다면 return으로 함수 즉시탈출
        }
        else { /*PASS*/ }
        #endregion _GiveSlotCount가 산장창고일 경우

    }   //SerchGiveSlot()

    //받는 슬롯을 찾아서 아이템을넣어주는 함수
    public void SerchAccepSlots(int _AcceptSlotCount, SG_ItemSlot _accepSlots)
    {
        #region 플레이어가 받는 슬롯일때
        if (_AcceptSlotCount > 1 && _AcceptSlotCount < 20) // 플레이어가 받는 Slot일때에
        {
            // 이번엔 부모의 부모의 부모 3번 찾아야함
            //1 -> Grid
            tempTrans001 = _accepSlots.transform.parent.GetComponent<Transform>();
            //2 -> InventoryImg
            tempTrans002 = tempTrans001.transform.parent.GetComponent<Transform>();
            //3 -> Inventory
            acceptInvenClass = tempTrans002.transform.parent.GetComponent<SG_Inventory>();

            for (byte i = 0; i < acceptInvenClass.slots.Length; i++)    //슬롯들을 싹 뒤지면서 주는 Slot의 고유번호 찾기
            {
                if (acceptInvenClass.slots[i].slotCount == _AcceptSlotCount) // 고유번호를 찾았을때 들어갈 조건문
                {
                    //아이템이 비어있지 않고 넣는아이템과 들어가 있는 아이템이 같지 않을때
                    if (acceptInvenClass.slots[i].item != null && acceptInvenClass.slots[i].item != moveItem)
                    {
                        isSwap = false;
                    }
                    else if (acceptInvenClass.slots[i].item == moveItem)
                    {
                        acceptInvenClass.slots[i].itemCount = acceptInvenClass.slots[i].itemCount + tempItemCount;
                        accepSlotCount = i;
                        ItemExamine();
                    }
                    else if (acceptInvenClass.slots[i].item == null)
                    {
                        // 아이템 정보를 집어넣어줌
                        acceptInvenClass.slots[i].item = moveItem;
                        acceptInvenClass.slots[i].itemCount = tempItemCount;
                        accepSlotCount = i;
                        ItemExamine();
                        Debug.LogFormat("For_accepSlotCount -> {0}", accepSlotCount);
                    }
                    //Debug.LogFormat("Accept() Give -> {0} Accept -> {1}", giveSlotCount, accepSlotCount);
                    break;
                }

            }
            return; // 찾아서 슬롯에 아이템을 넣어주었다면 return으로 함수 즉시탈출
        }
        else { /*PASS*/ }
        #endregion 플레이어가 받는 슬롯일때

        #region 산장창고가 받는 슬롯일떄
        if (_AcceptSlotCount > 99 && _AcceptSlotCount < 120) // 플레이어가 받는 Slot일때에
        {
            // 이번엔 부모의 부모의 부모 3번 찾아야함
            //1 -> Grid
            tempTrans001 = _accepSlots.transform.parent.GetComponent<Transform>();
            //2 -> InventoryImg
            tempTrans002 = tempTrans001.transform.parent.GetComponent<Transform>();
            //3 -> Inventory
            acceptInvenClass = tempTrans002.transform.parent.GetComponent<SG_Inventory>();

            for (byte i = 0; i < acceptInvenClass.slots.Length; i++)    //슬롯들을 싹 뒤지면서 주는 Slot의 고유번호 찾기
            {
                if (acceptInvenClass.slots[i].slotCount == _AcceptSlotCount) // 고유번호를 찾았을때 들어갈 조건문
                {
                    //아이템이 비어있지 않고 넣는아이템과 들어가 있는 아이템이 같지 않을때
                    if (acceptInvenClass.slots[i].item != null && acceptInvenClass.slots[i].item != moveItem)
                    {
                        isSwap = false;
                    }
                    else if (acceptInvenClass.slots[i].item == moveItem)
                    {
                        acceptInvenClass.slots[i].itemCount = acceptInvenClass.slots[i].itemCount + tempItemCount;
                        //Debug.LogFormat("받은얘의 아이템 갯수 -> {0}", acceptInvenClass.slots[i].itemCount);
                        accepSlotCount = i;
                        ItemExamine();
                    }
                    else if (acceptInvenClass.slots[i].item == null)
                    {
                        // 아이템 정보를 집어넣어줌
                        acceptInvenClass.slots[i].item = moveItem;
                        acceptInvenClass.slots[i].itemCount = tempItemCount;
                        accepSlotCount = i;
                        ItemExamine();
                    }
                    //Debug.LogFormat("Accept() Give -> {0} Accept -> {1}", giveSlotCount, accepSlotCount);
                    break;
                }
            }
            return; // 찾아서 슬롯에 아이템을 넣어주었다면 return으로 함수 즉시탈출
        }
        else { /*PASS*/ }

        #endregion 산장창고가 받는 슬롯일때

        #region 발전소가 받는 슬롯일때

        if (_AcceptSlotCount > 119 && _AcceptSlotCount < 125) // 발전소가 받는 Slot일때에
        {
            isMissionInven = true;

            // 아이템 갯수 몇개를 원하는지 알기위해 찾아오는 클래스
            tempMissionInitClass = _accepSlots.GetComponent<SG_PowerStationItemInIt>();

            // 이번엔 부모의 부모의 부모 3번 찾아야함
            //1 -> Grid
            tempTrans001 = _accepSlots.transform.parent.GetComponent<Transform>();
            //2 -> InventoryImg
            tempTrans002 = tempTrans001.transform.parent.GetComponent<Transform>();
            //3 -> Inventory
            acceptInvenClass = tempTrans002.transform.parent.GetComponent<SG_Inventory>();

            for (byte i = 0; i < acceptInvenClass.slots.Length; i++)    //슬롯들을 싹 뒤지면서 주는 Slot의 고유번호 찾기
            {
                if (acceptInvenClass.slots[i].slotCount == _AcceptSlotCount) // 고유번호를 찾았을때 들어갈 조건문
                {
                    //아이템이 비어있지 않고 넣는아이템과 들어가 있는 아이템이 같지 않을때
                    if (acceptInvenClass.slots[i].item != null && acceptInvenClass.slots[i].item != moveItem)
                    {
                        isSwap = false;
                    }
                    else if (acceptInvenClass.slots[i].item == moveItem)
                    {
                        acceptInvenClass.slots[i].itemCount = acceptInvenClass.slots[i].itemCount + tempItemCount;
                        //Debug.LogFormat("받은얘의 아이템 갯수 -> {0}", acceptInvenClass.slots[i].itemCount);
                        accepSlotCount = i;
                        // 이곳에서 다른 함수를 만들어서 체크 해야할거같음
                        MissionItemExamine();
                    }

                    //Debug.LogFormat("Accept() Give -> {0} Accept -> {1}", giveSlotCount, accepSlotCount);
                    break;
                }
            }
            return; // 찾아서 슬롯에 아이템을 넣어주었다면 return으로 함수 즉시탈출
        }
        else { /*PASS*/ }

        #endregion 발전소가 받는 슬롯일때

        #region 헬리패드가 받는 슬롯일때

        if (_AcceptSlotCount > 129 && _AcceptSlotCount < 136) // 핼리패드가 받는 Slot일때에
        {
            isMissionInven = true;

            // 아이템 갯수 몇개를 원하는지 알기위해 찾아오는 클래스
            tempMissionInitClass = _accepSlots.GetComponent<SG_PowerStationItemInIt>();

            // 이번엔 부모의 부모의 부모 3번 찾아야함
            //1 -> Grid
            tempTrans001 = _accepSlots.transform.parent.GetComponent<Transform>();
            //2 -> InventoryImg
            tempTrans002 = tempTrans001.transform.parent.GetComponent<Transform>();
            //3 -> Inventory
            acceptInvenClass = tempTrans002.transform.parent.GetComponent<SG_Inventory>();

            for (byte i = 0; i < acceptInvenClass.slots.Length; i++)    //슬롯들을 싹 뒤지면서 주는 Slot의 고유번호 찾기
            {
                if (acceptInvenClass.slots[i].slotCount == _AcceptSlotCount) // 고유번호를 찾았을때 들어갈 조건문
                {
                    //아이템이 비어있지 않고 넣는아이템과 들어가 있는 아이템이 같지 않을때
                    if (acceptInvenClass.slots[i].item != null && acceptInvenClass.slots[i].item != moveItem)
                    {
                        isSwap = false;
                    }
                    else if (acceptInvenClass.slots[i].item == moveItem)
                    {
                        acceptInvenClass.slots[i].itemCount = acceptInvenClass.slots[i].itemCount + tempItemCount;
                        //Debug.LogFormat("받은얘의 아이템 갯수 -> {0}", acceptInvenClass.slots[i].itemCount);
                        accepSlotCount = i;
                        // 이곳에서 다른 함수를 만들어서 체크 해야할거같음
                        MissionItemExamine();
                    }

                    //Debug.LogFormat("Accept() Give -> {0} Accept -> {1}", giveSlotCount, accepSlotCount);
                    break;
                }
            }
            return; // 찾아서 슬롯에 아이템을 넣어주었다면 return으로 함수 즉시탈출
        }
        else { /*PASS*/ }

        #endregion 헬리패드가 받는 슬롯일떄
    }   //SerchAccepSlots


    // 아이템 갯수가 넘는지 넘지 않는지 검사하는 함수
    private void ItemExamine()
    {
        //Debug.Log("아이템 갯수 체크하는 함수가 잘 들어와지는가?");
        Debug.LogFormat("accepSlotCount -> {0}", accepSlotCount);
        //Debug.LogFormat("AccepSlot ItemCount -> {0}", acceptInvenClass.slots[accepSlotCount].itemCount);
        //Debug.LogFormat("AccInvenClassNotNull? -> {0}",acceptInvenClass != null);
        if (acceptInvenClass.slots[accepSlotCount].itemCount > 3)    // 받는쪽의 아이템이 3개가 넘는지 체크
        {
            //Debug.LogFormat("아이템이 3개가 넘어가서 조건에 잘들어왔는가?");
            int tempItemCount001 = acceptInvenClass.slots[accepSlotCount].itemCount;
            int tempItemCount002 = tempItemCount001 - 3;

            acceptInvenClass.slots[accepSlotCount].itemCount = 3;
            //Debug.Log($"카운트 3개로 조정후 갯수 -> {acceptInvenClass.slots[accepSlotCount].itemCount}");

            isPassExamine = false;
            ItemExanineAfter(tempItemCount002);

        }
        else { /*PASS*/ }

    }

    // 발전소와 헬리페드는 아이템을 3개이상 요구할수 있기때문에 따로 체크
    private void MissionItemExamine()
    {
        // 슬롯에 있는 count가 WantCount 보다 큰지 체크 해야하고 그뒤에 넘는 만큼 위 함수 처럼 return 해주면 될듯
        //tempSlotClass
        if (tempMissionInitClass.wantItemCount < acceptInvenClass.slots[accepSlotCount].itemCount)
        {
            //Debug.LogFormat("아이템이 초과하는 식에 들어 왔나?");
            int tempItemCount001 = acceptInvenClass.slots[accepSlotCount].itemCount;
            int tempItemCount002 = tempItemCount001 - tempMissionInitClass.wantItemCount;

            acceptInvenClass.slots[accepSlotCount].itemCount = tempMissionInitClass.wantItemCount;
            //Debug.Log($"카운트 3개로 조정후 갯수 -> {acceptInvenClass.slots[accepSlotCount].itemCount}");

            isPassExamine = false;
            ItemExanineAfter(tempItemCount002);
            tempMissionInitClass.ItemTextUpdate();      // 아이템 CountText 업데이트 해주는 함수
            tempMissionInitClass.CheckSucceseMission(); // 미션 갯수 충족했는지 체크하는함수
        }
        else
        {
            tempMissionInitClass.ItemTextUpdate();      // 아이템 CountText 업데이트 해주는 함수
            tempMissionInitClass.CheckSucceseMission(); // 미션 갯수 충족했는지 체크하는함수
        }
    }

    // 아이템 검사 함수에서 불통하면 들어올 함수
    // Give Item Canvas위치 원래대로해줄거고 아이템에 대한 정보와 출력을 처리해줄거임
    private void ItemExanineAfter(int _itemCount)
    {
        giveItemDragScript.SetTransformParent();
        giveInvenClass.slots[giveSlotCount].item = moveItem;
        giveInvenClass.slots[giveSlotCount].itemCount = _itemCount;
        giveInvenClass.slots[giveSlotCount].MoveItemSet();
    }

    // 아이템을주고 후처리해주는 함수 -> 슬롯 초기화
    public void GiveExchangeAfter()
    {
        if (isSwap == false)
        { return; }
        else { /*PASS*/ }

        if (giveInvenClass.slots[giveSlotCount].item == acceptInvenClass.slots[accepSlotCount].item)
        {
            if (isPassExamine == true)
            {
                giveInvenClass.slots[giveSlotCount].DisconnectedItem();
                //isPassExamine = false;
            }
            else
            {
                isPassExamine = true;
            }
        }
        else { /*PASS*/ }
        //}
        //else { Debug.LogError("Invalid giveSlotCount: " + giveSlotCount); }
    }


    // 아이템을받고 후처리해주는 함수 -> 슬롯이 다시 받은아이템을 출력하기
    public void AcceptExchangeAfter()
    {
        if (isSwap == false) { return; }    // 같은 인벤이면 다시 출력할 필요 X
        else { /*PASS*/ }

        if (isMissionInven == true) { return; } // 발전소,헬리패드는 따로 ItemImage를 관리하기 때문에 오류방지 return
        else { /*PASS*/ }

        acceptInvenClass.slots[accepSlotCount].MoveItemSet();
    }

    // 마지막에 무조건적으로 다시 원래대로 초기화 되어야 하는 변수를 원래대로 해주는 함수
    private void InitialValue()
    {
        isSwap = true;
        sameSlot = false;
        isMissionInven = false;
    }


}   //NameSpace 
