using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SG_WorkStationRecipeImage : MonoBehaviour, IPointerClickHandler
{
    public SG_ItemRecipe itemRecipe;
    private SG_WorkStationContentControler workStationContentControlerClass;

    private Image madeItemImage;        //완성아이템의 이미지
    private Image materialItemImage001; //재료아이템의 이미지    
    private Image materialItemImage002; //재료아이템의 이미지
    private Image thisImage;    //레시피 뒷 배경이미지

    private Color defaultColor; // Defaul Color 204 204 204 204
    private Color onClickColor; // OnClick Color 102 102 102 204

    public int recipeCount;

    public event System.Action<int> RecipeListClickEvent;




    public bool isClickState = false;

    private void Awake()
    {
        AwakeInIt();
    }

    void Start()
    {
        StartInIt();
    }

    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)  // 클릭시 1회 호출
    {       
        RecipeListClickEvent?.Invoke(recipeCount);      
    }


    private void AwakeInIt()    // Awake단계에서 넣어줄 변수
    {
        workStationContentControlerClass = transform.parent.GetComponent<SG_WorkStationContentControler>();
        madeItemImage = transform.Find("ItemImage").GetComponent<Image>();
        materialItemImage001 = transform.Find("NeedItemImage001").GetComponent<Image>();
        materialItemImage002 = transform.Find("NeedItemImage002").GetComponent<Image>();
        thisImage = GetComponent<Image>();        
    }

    private void StartInIt()    // Start단계에서 넣어줄 변수
    {
        madeItemImage.sprite = itemRecipe.madeItem.itemImage;
        materialItemImage001.sprite = itemRecipe.item001.itemImage;
        materialItemImage002.sprite = itemRecipe.item002.itemImage;

        defaultColor = new Color(0.8f, 0.8f, 0.8f, 0.8f);
        onClickColor = new Color(0.4f, 0.4f, 0.4f, 0.8f);

        workStationContentControlerClass.RecipeListSetColorEvent += SetImageColor;
    }

    public void SetImageColor() //클릭되있는지 확인하고 상태에 따라서 색이바뀌는 함수
    {
        if (isClickState == true)
        {            
            thisImage.color = onClickColor;
        }
        else if (isClickState == false)
        {            
            thisImage.color = defaultColor;
        }
        
    }


}   //NameSpacve
