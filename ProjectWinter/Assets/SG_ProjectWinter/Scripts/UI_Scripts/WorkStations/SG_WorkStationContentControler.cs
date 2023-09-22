using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SG_WorkStationContentControler : MonoBehaviour
{
    private GameObject itemRecipeListParent;    // 아이템레시피의 부모 오브젝트 -> this

    public SG_WorkStationRecipeImage[] itemRecipeList;  // 제작가능한 레시피들을 담아둘 배열

    public event System.Action RecipeListSetColorEvent; // 클릭한 레시피색을 변경해줄 이벤트

    private Transform topParentTrans;               // 최상위 부모를 넣어줄 변수
    private SG_WorkStationControler topParentClass; // topParent가 가지고 있는 Class Inventory과 같은 역활을 해주는것을 가지고있음
    public SG_Inventory playerInventory;            // 아이템 제작을할때 재료를 소비할 플레이어의 인벤토리

    private SG_ProductionManager productionManager; // 제작 메니저의 함수를 호출하기 위한 변수

    private int itemRecipeListCount;    // 아이템레시피 리스트 보내줄떄 정확히 찍어보내주기위한 변수


    void Awake()
    {
        AwakeInIt();        // Awake 단계에서 기입할 변수
        SetRecipeCount();   // 아이템 레시피 리스트에 고유번호 넣어줌
        EventSubscriber();   // 아이템 레시피의 이벤트를 구독해주는 함수
    }

    void Start()
    {
        StartInIt();        //Start 단계에서 기입할 변수
        
    }
    
    void Update()
    {
        
    }

    public void AwakeInIt()
    {
        itemRecipeListParent = GetComponent<Transform>().gameObject;        
        itemRecipeList = itemRecipeListParent.GetComponentsInChildren<SG_WorkStationRecipeImage>();
    }

    public void StartInIt() // Start지점에서 넣어줄 것
    {
        productionManager = FindAnyObjectByType<SG_ProductionManager>();
    }

    private void SetRecipeCount()   // 레시피의 고유번호넣어주는 함수
    {
        // 번호에 I번을 넣어서 배열그대로 쓸수 있을거같음
        for(int i =0; i < itemRecipeList.Length; i++)
        {
            itemRecipeList[i].recipeCount = i;
        }
    }

    public void EventSubscriber() // 이벤트를 구독해주는 함수
    {
        for(int i =0; i < itemRecipeList.Length; i++)
        {
            itemRecipeList[i].RecipeListClickEvent += RecipeListClickEventCall;
        }
    }

    public void RecipeListClickEventCall(int _ListCount)
    {
        for(int i = 0; i < itemRecipeList.Length; i++)
        {
            if (itemRecipeList[i].recipeCount == _ListCount)
            {
                itemRecipeList[i].isClickState = true;
                //Debug.LogFormat("현재 눌려있는 아이템 -> {0}", itemRecipeList[i].itemRecipe.madeItem.itemName);
            }
            else
            {
                itemRecipeList[i].isClickState = false;
            }
        }

        RecipeListSetColorEvent?.Invoke();  // 컬러 bool 값에 따라바뀌는 함수 실행

    }



    private void SerchTopParentTrans()  // 최상위 부모오브젝트 찾는 함수
    {
        topParentTrans = transform;

        while(topParentTrans.parent != null)
        {
            topParentTrans = topParentTrans.parent.transform;
        }
    }

    public void LetProduction() //제작버튼을 누르면 실행될 함수
    {
        SerchTopParentTrans();
        SerchMakeItemList();    // 버튼 누른 시점의 클릭상태가 true되어있는 레시피 추출

        topParentClass = topParentTrans.GetComponent<SG_WorkStationControler>();

        playerInventory = topParentClass.playerInventory;   // 최상위부모가 담아두고 있던 플레이어의 인벤토리 버튼으로 토스

        productionManager.StartProduction(playerInventory, itemRecipeList[itemRecipeListCount]);    // 아이템 제작 조건으로 출발

    }

    private void SerchMakeItemList()
    {
        for (int i = 0; i < itemRecipeList.Length; i++)
        {
            if (itemRecipeList[i].isClickState == true)
            {
                itemRecipeListCount = i;
                break;
            }
            else { /*PASS*/ }

        }
    }

}   //NameSpace
