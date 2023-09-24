using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG_ProductionManager : MonoBehaviour
{

    // 아래 두개의 Gameobject 는 아이템을 집어넣어줄때에 빈슬롯이 없으면 Instance를 해주기 위해 만든 변수
    private GameObject madeItemObj;
    private GameObject madeItemObjClone;

    // 아이템 Instance 할떄에 스폰될 Position 과 Rotaition
    Vector3 spawnPosition;
    Quaternion spawnRotation;

    private bool isCanCraftItem;    // 아이템 조건이 맞지않다면 false가 되어서 함수를 나가게할 변수    

    private int tempItemp001Count;
    private int tempItemp002Count;

    // 불필요한 for문을 줄이기위한 변수
    private int tempItem001SlotCount;   // 재료아이템1 이 존재했을때에 아이템 슬롯의 좌표를 찍어줄 변수
    private int tempItem002SlotCount;   // 재료아이템2 이 존재했을때에 아이템 슬롯의 좌표를 찍어줄 변수

    private bool checkMaterial001MethodClear;   // 첫번째 검수 통과 했는지 체크할 bool 변수
    private bool checkMaterial002MethodClear;   // 두번째 검수 통과 했는지 체크할 bool 변수

    private bool checkItemEnoughCount;          // 첫번째 두번째 재료가 동일할시 아이템 2개가 존재하는지 체크하는 함수

    private int inItCount;        // 코루틴으로 한프레임 쉬도록 만들때의 슬롯의 인덱스를저장할 변수

    private Coroutine itemSetUpdatecoroutine;    // 코루틴 박싱
    private Coroutine weaponColorCoroutine; // 무기 컬러 조절 코루틴

    public void StartProduction(SG_Inventory _PlayerInven, SG_WorkStationRecipeImage _ProductionRecipe) // 제작 버튼 클릭시 들어올 함수
    {
        ResetVariable();    // 변수 초기화 해주는 함수

        CheckMaterial001(_PlayerInven,_ProductionRecipe);  // 플레이어 인벤토리에 레시피 첫번째 재료가 존재하는지 체크하는 함수
        if (checkMaterial001MethodClear == false) { Debug.Log("첫번째 검수 실패"); return; }  else { /*PASS*/ } // 첫번째 아이템이 없다면 제작을 빠져나가도록 return
        

        CheckMaterial002(_PlayerInven,_ProductionRecipe);  // 플레이어 인벤토리에 레시피 두번쨰 재료가 존재하는지 체크하는 함수
        if (checkMaterial002MethodClear == false) { Debug.Log("두번째 검수 실패"); return; }  else { /*PASS*/ } // 두번째 아이템이 없다면 제작을 빠져나가도록 return

        CheckItemEnoughCount(_PlayerInven,_ProductionRecipe); // 첫번째 재료 두번째 재료가 동일할시 아이템이 2개 필요할텐데 그때에 플레이어 인벤토리에 아이템이 2개가 존재하는지 체크하는 함수
        if (checkItemEnoughCount == false)        { Debug.Log("동일한 재료이고 갯수부족으로 실패"); return; }  else { /*PASS*/ } // 재료아이템이 동일한데 재료갯수가 부족하면 return

        DeductPlayerItemCount(_PlayerInven,_ProductionRecipe);// 플레이어의 슬롯에서 재료아이템을 제작에 필요한 만큼 차감해주는 함수
        CleanUpPlayerSlot(_PlayerInven);                      // 아이템 소모로 플레이어 아이템의 갯수가 0개라면 슬롯 비워주는 함수
        InItMadeItem(_PlayerInven, _ProductionRecipe);        // 만든 아이템을 빈슬롯이 존재한다면 플레이어 슬롯에 넣어주고 없다면 플레이어 위치에 Instance해주는 함수


        //ResetVariable();    // 변수 초기화 해주는 함수


    }   // StartProduction()

    // 첫번째 재료아이템 가지고 있는지,갯수 있는지 확인하는 함수
    private void CheckMaterial001(SG_Inventory _PlayerInven, SG_WorkStationRecipeImage _ProductionRecipe) // 첫번째 재료아이템 가지고 있는지,갯수 있는지 확인
    {
        for(int i = 0; i < _PlayerInven.slots.Length; i++)
        {
            if (_PlayerInven.slots[i].item == _ProductionRecipe.itemRecipe.item001)
            {
                checkMaterial001MethodClear = true;
                tempItem001SlotCount = i;
                return;
            }
            else { checkMaterial001MethodClear = false; }
        }

    }   // CheckMaterial001()

    // 두번째 재료아이템 가지고 있는지,갯수 있는지 확인
    private void CheckMaterial002(SG_Inventory _PlayerInven, SG_WorkStationRecipeImage _ProductionRecipe) // 두번째 재료아이템 가지고 있는지,갯수 있는지 확인
    {
        for (int i = 0; i < _PlayerInven.slots.Length; i++)
        {
            if (_PlayerInven.slots[i].item == _ProductionRecipe.itemRecipe.item002)
            {
                checkMaterial002MethodClear = true;
                tempItem002SlotCount = i;
                return;
            }
            else { checkMaterial002MethodClear = false; }
        }

    }   // CheckMaterial002()

    // 첫번째 재료 두번째 재료가 동일할시 아이템이 2개 필요할텐데 그때에 플레이어 인벤토리에 아이템이 2개가 존재하는지 체크하는 함수
    private void CheckItemEnoughCount(SG_Inventory _PlayerInven, SG_WorkStationRecipeImage _ProductionRecipe)
    {
        if (_ProductionRecipe.itemRecipe.item001 == _ProductionRecipe.itemRecipe.item002)    // 레시피의 아이템이 같은지 확인
        {
            for (int i = 0; i < _PlayerInven.slots.Length; i++)  // 플레이어 슬롯만큼 돌면서 슬롯체크
            {
                if (_PlayerInven.slots[i].item == _ProductionRecipe.itemRecipe.item001) // 슬롯의 아이템이 재료와 같은경우
                {
                    if (_PlayerInven.slots[i].itemCount < 2)    // 아이템 갯수가 2개 이상있는지 확인  아이템이 0~1 개일때의 false하며 break
                    {
                        checkItemEnoughCount = false;
                        break;
                    }
                    else { /*PASS*/ }
                }
                else { /*PASS*/ }
            }
        }
        else { /*PASS*/ }   // 레시피가 아이템 1 아이템 2가 동일하지않으면 Pass

    }   // CheckItemEnoughCount()

    // 재료 아이템을 차감해주는 함수
    private void DeductPlayerItemCount(SG_Inventory _PlayerInven, SG_WorkStationRecipeImage _ProductionRecipe)
    {
        _PlayerInven.slots[tempItem001SlotCount].itemCount -= 1;    // 첫번째 아이템 재료 차감

        _PlayerInven.slots[tempItem002SlotCount].itemCount -= 1;    // 두번째 아이템 재료 차감
    }   // DeductPlayerItemCount()

    private void CleanUpPlayerSlot(SG_Inventory _PlayerInven)   // 아이템 소모로 플레이어 아이템의 갯수가 0개라면 슬롯 비워주는 함수
    {
        // 첫번째재료소모에 사용된 슬롯 청소
        if (_PlayerInven.slots[tempItem001SlotCount].itemCount <= 0)
        {
            _PlayerInven.slots[tempItem001SlotCount].DisconnectedItem();
        }
        else { /*PASS*/ }

        // 아이템 이 같을때에 아이템카운트가 소모되어 첫슬롯 청소에 슬롯의 아이템이 Null 되었을때를 대비하는 예외처리
        if (_PlayerInven.slots[tempItem002SlotCount].item == null)  // 2번째 재료의 슬롯 아이템이 비어있다면 그냥 넘기기
        {
            return;
        }
        else { /*PASS*/ }

        // 두번째재료소모에 사용된 슬롯 청소
        if (_PlayerInven.slots[tempItem002SlotCount].itemCount <= 0)
        {
            _PlayerInven.slots[tempItem002SlotCount].DisconnectedItem();
        }
        else { /*PASS*/ }
    }   // CleanUpPlayerSlot()


    // 플레이어에게 만들어진 아이템을 주는 함수
    private void InItMadeItem(SG_Inventory _PlayerInven, SG_WorkStationRecipeImage _ProductionRecipe) // 플레이어에게 만들어진 아이템을 주는 함수
    {
        for(int i = 0; i < _PlayerInven.slots.Length; i++)
        {
            if (_PlayerInven.slots[i].item == null) //슬롯이 비어있다면
            {
                _PlayerInven.slots[i].item = _ProductionRecipe.itemRecipe.madeItem; // 완성 아이템 넣어주기
                _PlayerInven.slots[i].itemCount = _ProductionRecipe.itemRecipe.madeItemCount;   // 완성된 아이템 갯수 기입
                inItCount = i;
                _PlayerInven.slots[i].ImageObjInstance();   // 자식오브젝트로 Item Image Instace후 자동으로 필요한 Component 넣어주는 함수 Call                
                itemSetUpdatecoroutine = StartCoroutine(PlayerItemSetUpdate(_PlayerInven));
                Debug.LogFormat("만든게 무기인가? -> {0}", _ProductionRecipe.itemRecipe.madeItem.itemType == SG_Item.ItemType.Weapon);
                if (_ProductionRecipe.itemRecipe.madeItem.itemType == SG_Item.ItemType.Weapon)  // 만든 아이템이 무기일경우 들어갈함수
                {
                    //_PlayerInven.slots[i].WeaponColorSet(); // 아이템 CountText,CountImage 투명도 조절해주는 함수
                    weaponColorCoroutine = StartCoroutine(WeaponColorUpdate(_PlayerInven));
                }
                else { /*PASS*/ }
                return; // 빈슬롯에 넣어주었으면 함수 탈출
            }
            else { /*PASS*/ }
        }

        // 이아래로 온다면 슬롯이 빈곳이 없다는 의미
        madeItemObj = _ProductionRecipe.itemRecipe.madeItem.itemPrefab;

        // 스폰될 위치를 인벤토리 열었던 플레이어의 위치로 잡아줌
        spawnPosition = _PlayerInven.slots[0].slotTopParentObj.transform.position;
        spawnRotation = _PlayerInven.slots[0].slotTopParentObj.transform.rotation;

        // 만든 아리템을 플레이어의 위치로 Instance
        madeItemObjClone = Instantiate(madeItemObj, spawnPosition, spawnRotation);
        


    }   // InItMadeItem()




    private void ResetVariable()    // 변수 초기화 해주는 함수    매니저 실행시,종료시 총 2번 초기화
    {
        isCanCraftItem = true;
        checkMaterial001MethodClear = true;
        checkMaterial002MethodClear = true;
        checkItemEnoughCount = true;
    }
   
    IEnumerator PlayerItemSetUpdate(SG_Inventory _PlayerInven)  // 플레이어 아이템표시 업데이트를 1프레임 뒤에 해줄 코루틴
    {
        yield return null;
        _PlayerInven.slots[inItCount].MoveItemSet();
    }
    IEnumerator WeaponColorUpdate(SG_Inventory _PlayerInven)
    {
        yield return null;
        _PlayerInven.slots[inItCount].WeaponColorSet();
    }

}
