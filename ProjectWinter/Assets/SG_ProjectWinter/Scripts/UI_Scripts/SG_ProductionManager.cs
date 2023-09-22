using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG_ProductionManager : MonoBehaviour
{

    private bool isCanCraftItem;    // 아이템 조건이 맞지않다면 false가 되어서 함수를 나가게할 변수    

    private int tempItemp001Count;
    private int tempItemp002Count;


    public void StartProduction(SG_Inventory _PlayerInven, SG_WorkStationRecipeImage _ProductionRecipe) // 제작 버튼 클릭시 들어올 함수
    {

        // 23.09.22 매니저에 매개변수 까지 잘들어왔음
        //Debug.Log("매니저에 잘들어왔나?");
        //Debug.LogFormat("매개변수는 잘 왔나? player -> {0}     Recipe -> {1}", _PlayerInven.name, _ProductionRecipe.itemRecipe.madeItem.itemName);
        ResetVariable();    // 변수 초기화 해주는 함수
        CheckMaterial001(_PlayerInven, _ProductionRecipe);


    }

    // 첫번째 재료아이템 가지고 있는지,갯수 있는지 확인하는 함수
    private void CheckMaterial001(SG_Inventory _PlayerInven, SG_WorkStationRecipeImage _ProductionRecipe) // 첫번째 재료아이템 가지고 있는지,갯수 있는지 확인
    {
        for(int i = 0; i < _PlayerInven.slots.Length; i++)
        {

        }

    }


    private void ResetVariable()    // 변수 초기화 해주는 함수    매니저 실행시,종료시 총 2번 초기화
    {
        isCanCraftItem = true;
    }
   

}
