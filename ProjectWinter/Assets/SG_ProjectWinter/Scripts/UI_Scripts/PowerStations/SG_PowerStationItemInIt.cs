using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SG_PowerStationItemInIt : MonoBehaviour
{
    // 발전소,헬리페드가 원하는 아이템을 넣어주어야하는데
    // 이 스크립트에서 발전소와 헬리페드가 원하는 아이템을 랜덤으로 정해줄거임

    // 아이템 갯수 제한 3 ~ 5 개 사이로 랜덤으로 원하게 함
    // 헬리페드와 발전소가 원할 아이템 = 가스통,전자부품,기계부붐


    // [0] = 전자부품 , [1] 기게부품 , [2] 가스통
    public SG_Item[] itemLists; // 원하는 아이템을 추려서 넣을것임

    public GameObject itemImageObj;
    private GameObject itemImageObjClone;

    private SG_Inventory inventoryClass;

    [SerializeField]
    private TextMeshProUGUI wantItemCountText;  // 아이템의 카운트를 체크해줄 텍스트 Ex) 1 / 0
    private SG_ItemSlot itemSlotClass;  //  슬롯이 가지고 있는 아이템 클래스 sprite,item 넣어줄거임

    private Image itemImage;    // 아이템 Image를 Instace한뒤에 이 변수에 사용할수 있도록 넣어줄거임

    private Transform topParentTrans;   //최상위 부모를 알려줄 Trans

    public int wantItemCount;      // 랜덤이 선택한 원하는 아이템의 Count
    private int tempItemListCount;  // 랜덤이 선택한 배열의 Index 변수

    private bool isFirstOpen = false;   // 처음 열때에만 Instace되고 아이템 Count or Item Pcik 해주도록 해줄 변수
    private bool missionClear = false;  // 미션이 클리어 된다면 아이템 갯수 충족한지 부족한지 체크해줄 함수 넘기기위한 변수


    void Awake()
    {

    }

    void Start()
    {
        FirstInitialize();  // 변수에 Getcomponent 해야할것들이나 사용전 미리 삽입해야하는것들 넣어주는 함수
        FirstOpen();        // 발전소,헬리페드 처음 열때만 아이템이 랜덤으로 지정되도록 해주는 함수

    }


    private void FirstOpen()    // 발전소,헬리페드 처음 열때만 아이템이 랜덤으로 지정되도록 해주는 함수
    {
        if (isFirstOpen == false)
        {
            SerchTopParentTrans();  //최상위 부모오브젝트찾는 로직
            RamdomItemInIt();       // 넣어야할 아이템을 랜덤으로 정해주는 함수
            RandomItemCountInIt();  // 넣어야하는 아이템 목표치 3 ~ 5로 정해주는 함수
            ItemImageInIt();        // 넣어야하는 아이템의 정보가가지고 있는 스프라이트를 넣어주는 함수
            ItemTextUpdate();       // 현재 넣은 아이템의 갯수와 넣어야 하는 아이템의 갯수를 택스트로 보여주는 함수

        }
    }   // FirstOpen()


    private void FirstInitialize()
    {
        wantItemCountText = transform.Find("WantCountText").GetComponent<TextMeshProUGUI>();    // 자식오브젝트에서 이름으로 찾아옴

        itemSlotClass = GetComponent<SG_ItemSlot>();
    }

    private void SerchTopParentTrans() //최상위 부모오브젝트찾는 로직
    {
        topParentTrans = transform;

        while (topParentTrans.parent != null)
        {
            topParentTrans = topParentTrans.parent;
        }

    }

    private void RamdomItemInIt() // 넣어야할 아이템을 랜덤으로 정해주는 함수
    {
        if (topParentTrans.CompareTag("PowerStation"))
        {
            tempItemListCount = Random.Range(0, itemLists.Length);
            itemSlotClass.item = itemLists[tempItemListCount];
        }
        else if (topParentTrans.CompareTag("HeliPad"))
        {
            tempItemListCount = 2;  // 헬리페드는 가스통으로 고정
            itemSlotClass.item = itemLists[tempItemListCount];
        }
    }   // RamdomItemInIt()

    private void RandomItemCountInIt()  // 넣어야하는 아이템 목표치 3 ~ 5로 정해주는 함수
    {
        if (topParentTrans.CompareTag("PowerStation"))
        {
            wantItemCount = Random.Range(3, 6);
        }
        else if (topParentTrans.CompareTag("HeliPad"))
        {
            wantItemCount = Random.Range(5, 8);
        }

    }   // RandomItemCountInIt()

    private void ItemImageInIt()
    {
        // 이미지 가져오는것이 자식 오브젝트에 Image를 낳아서 거기다 넣는 식이기 떄문에 원래 있는 prefab 에서 조금 수정해서 
        // 얘만의 ItemImage를 instance해서 GetParent시키고 그거를 땡겨와서 출력해야할거같음 
        // ItemSlotClass.itemImage = 아이템 슬롯의 테두리임

        itemImageObjClone = Instantiate(itemImageObj);
        itemImageObjClone.transform.SetParent(this.transform);

        itemImage = itemImageObjClone.GetComponent<Image>();

        itemImage.sprite = itemSlotClass.item.itemImage;


    }   // ItemImageInIt()

    public void ItemTextUpdate()   // 현재 넣은 아이템의 갯수와 넣어야 하는 아이템의 갯수를 택스트로 보여주는 함수
    {
        #region Debug
        //Debug.LogFormat("wantItemCountText == null? -> {0}", wantItemCountText == null);
        //Debug.LogFormat("itemSlotClass.itemCount -> {0}", itemSlotClass.itemCount);
        //Debug.LogFormat("wantItemCount -> {0}", wantItemCount);
        #endregion Debug
        //재활용 가능
        wantItemCountText.text = itemSlotClass.itemCount.ToString() + " / " + wantItemCount.ToString();

    }   // ItemTextUpdate()

    // SwapManager에서 Swap이 되었을때에 호출해줄 함수
    public void CheckSucceseMission() // 아이템 갯수가 요구하는 만큼 충족하다면 true로 될것임
    {
        if (inventoryClass == null || inventoryClass == default)
        {
            inventoryClass = transform.parent.parent.parent.GetComponent<SG_Inventory>();
        }
        else { /*PASS*/ }

        if (missionClear == false)  // 미션이 클리어 된적이 없을떄에만 함수 조건 체크
        {
            if (topParentTrans.CompareTag("PowerStation"))
            {
                if (itemSlotClass.itemCount == wantItemCount)
                {
                    inventoryClass.CheckClearPowerStation();
                }
                else { /*PASS*/ }
            }

            else if(topParentTrans.CompareTag("HeliPad"))
            {
                if(itemSlotClass.itemCount == wantItemCount)
                {
                    inventoryClass.CheckClearHeliPad();
                }
            }
        }
        else { /*PASS*/ }
    }

}   // NameSpace
