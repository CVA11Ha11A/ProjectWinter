using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SG_PowerStationItemImageColorSet : MonoBehaviour
{
    private GameObject topParentObj;
    private Image itemCountImage;
    private TextMeshProUGUI itemCountText;

    Color offColor;
    Color halfColor;
    Color onColor;

    // Start is called before the first frame update
    void Start()
    {
        GetThisTopParentObj();  // 최상위 오브젝트를 가져옴
        SelfDiagnosis();        // 플레이어나 산장창고는 이기능이 필요없기에 스스로 스크립트 끄기
        ColorSet();             // 위에서 선언한 컬러들 색 선언해줌
        ColorOff();             // 헬리페드와 발전기는 아이템의 카운트 와 카운트 이미지가 필요없기때문에 색을 Off 해줌
        
    }





    private void GetThisTopParentObj()  //최상위 부모 오브젝트 태그를 가져오기 위해 찾는 로직
    {
        topParentObj = this.gameObject;
        
        while (topParentObj.transform.parent != null)   // 부모오브젝트가 없을때 까지 돎
        {
            topParentObj = topParentObj.transform.parent.gameObject;
        }
    }

    private void SelfDiagnosis() // 자신이 발전소나 헬리페드가 아니라면 사용할 필요가 없기에 스크립트 꺼줌
    {

        if (!topParentObj.CompareTag("PowerStation") && !topParentObj.CompareTag("HeliPad"))
        {
            enabled = false;
        }
        else { /*PASS*/ }
    }


    private void ColorSet()
    {
        onColor = new Color(1f, 1f, 1f, 1f);
        halfColor = new Color(1f, 1f, 1f, 0.5f);
        offColor = new Color(1f, 1f, 1f, 0f);
    }


    private void ColorOff()
    {
        // 발전소와 헬리페드는 아래서 다른 Text가 카운트 해주기 떄문에 A 값 조정으로 안보이도록함
        // 참조하다가 Null뜰경우를 대비해 색으로 조절
        if (topParentObj.CompareTag("PowerStation") || topParentObj.CompareTag("HeliPad"))
        {
            itemCountImage = transform.GetChild(0).GetComponent<Image>();
            itemCountText = itemCountImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

            itemCountImage.color = offColor;
            itemCountText.color = offColor;
        }
        else { /*PASS*/ }
    }
}
