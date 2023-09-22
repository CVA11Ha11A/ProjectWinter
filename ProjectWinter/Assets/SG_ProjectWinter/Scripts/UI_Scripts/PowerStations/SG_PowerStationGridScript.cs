using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG_PowerStationGridScript : MonoBehaviour
{
    [SerializeField]
    private GameObject slot;

    private GameObject slotClone;
    private Transform topParentTrans;

    // 원하는 아이템 슬롯을 몇개를 만들지 알려줄 변수
    private int makeSlotCount;  // 1 ~ 2 들어갈 예정

    private short repetitionMakeSlotCount = 1;  // MakeSlot 함수를 재귀함수로 사용하기 위해 변수 선언
    

    private void Awake()
    {

        //Debug.Log(makeSlotCount);
    }
    private void Start()
    {
        SerchTopParentTrans();
        makeSlotCount = Random.Range(1, 3);  // 1 ~ 2 랜덤한 숫자를 삽입
        MakeSlot(); // 슬롯을 랜덤하게 만들어서 자식오브젝트로 넣는 함수
    }

    private void SerchTopParentTrans() // 최상위 부모 오브젝트 찾는 로직
    {
        topParentTrans = transform;

        while (topParentTrans.parent != null)
        {
            topParentTrans = topParentTrans.parent;
        }
    }

    private void MakeSlot() // 슬롯을 랜덤하게 만들어서 자식오브젝트로 넣는 함수
    {
        if (topParentTrans.CompareTag("PowerStation"))  // 발전소일때 Instance Slot
        {
            slotClone = Instantiate(slot);
            slotClone.transform.SetParent(this.transform);

            if (repetitionMakeSlotCount < makeSlotCount)
            {
                repetitionMakeSlotCount++;
                MakeSlot();
            }
            else { /*PASS*/ }
        }        
        else if(topParentTrans.CompareTag("HeliPad"))   // 헬리패드일때 Instance Slot
        {
            slotClone = Instantiate(slot);
            slotClone.transform.SetParent(this.transform);
        }
        else { /*PASS*/ }

    }   // MakeSlot()

}   //NameSpace
