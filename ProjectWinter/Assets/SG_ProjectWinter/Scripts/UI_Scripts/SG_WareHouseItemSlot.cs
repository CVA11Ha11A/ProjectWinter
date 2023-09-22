using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SG_WareHouseItemSlot : MonoBehaviour
{
    // 23.09.10 아래 는 플레이어의 ItemSlot을 그대로 가져온것임
    // 아래내용을 이제 산장창고 슬롯으로 잘 바꿔서 사용해야 함
    // 23.09.10 한번 했고 산장에 사용하는데에 큰 문제가 있어보이지 않기에 일단 사용

    public SG_Item item;    // 아이템의 정보가 들어있는 곳
    public int itemCount;   // 획득한 아이템의 갯수
    public Image itemImage; // 획득한 아이템의 이미지
    public int wareHouseSlotCount;

    // 필요한 컴포넌트
    [SerializeField]
    private TextMeshProUGUI text_Count;
    [SerializeField]
    private GameObject itemCountImg;


    // 이미지의 투명도 조절
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }


    // 아이템 획득
    public void AddItem(SG_Item _item, int _count = 1)
    {
        //TODO : 아이템 스크립트 만든후 아이템 얻었을떄에
        //          추가해주는함수 에 넣어주어야함
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;

        if (item.itemType != SG_Item.ItemType.Weapon)
        {
            itemCountImg.SetActive(true);
            text_Count.text = itemCount.ToString();
        }
        else
        {
            text_Count.text = "0";
            itemCountImg.SetActive(false);
        }

        SetColor(1);
    }

    // 아이템 개수 조정
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();

        if (itemCount <= 0)
        {
            ClearSlot();
        }
    }

    // 아이템 슬롯 초기화
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        text_Count.text = "0";
        itemCountImg.SetActive(false);
    }

}
