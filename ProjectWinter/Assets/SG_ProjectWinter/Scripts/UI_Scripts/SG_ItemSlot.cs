using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SG_ItemSlot : MonoBehaviour
{

    public SG_Item item;    // 아이템의 정보가 들어있는 곳
    public int itemCount = 0;   // 획득한 아이템의 갯수
    public int slotCount = 0;   //아이템 슬롯의 고유번호 삽입될 변수

    public GameObject itemImagePrefab;

    private GameObject itemImageClone;

    // 아래 이미지는 아이템을 먹었을때에 Instance 되고 Instance 됬을떄에 자식오브젝트로 찾아서 넣어줄거임
    public Image itemImage; // 획득한 아이템의 이미지


    // 필요한 컴포넌트     
    public TextMeshProUGUI text_Count;
    public GameObject itemCountImg;

    public GameObject slotTopParentObj;

    private void Start()
    {
        slotTopParentObj = this.transform.parent.gameObject;
        // slot의 최상위 오브젝트 태그로 누군지 구별하기위해 최상위 오브젝트 삽입해주는 함수
        GetThisTopParentObj();  
    }


    // 이미지의 투명도 조절
    private void SetColor(float _alpha)
    {
        //Color color = itemImage.color;
        //color.a = _alpha;
        //itemImage.color = color;
    }


    // 아이템 획득
    public void AddItem(SG_Item _item, int _count = 1)
    {
        // 플레이어 가 아니라면 Inventory Script 에서 Return을 때리기 떄문에 조건문 삽입 X
    

        item = _item;
        itemCount = _count;
        //itemImage.sprite = item.itemImage;

        if (item.itemType != SG_Item.ItemType.Weapon)
        {
            if (itemImage == null)
            {
                ImageObjInstance();
            }

            //itemCountImg.SetActive(true);
            text_Count.text = itemCount.ToString();
            itemImage.sprite = item.itemImage;
        }
        else
        {
            text_Count.text = "0";
            //itemCountImg.SetActive(false);
        }

        SetColor(1);
    }

    // 아이템 개수 조정
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();
        //Debug.Log("아이템 +=");

        if (itemCount <= 0)
        {
            ClearSlot();
        }
    }

    // ItemImageObj를 인스턴스후 슬롯의 자식오브젝트로 넣어주는 함수
    private void ImageObjInstance()
    {
        itemImageClone = Instantiate(itemImagePrefab, this.transform.position, Quaternion.identity);
        itemImageClone.transform.SetParent(this.transform);

        ChildSet();
    }

    // 아이템 Image 클론 만든후 자식의 자식의 자식 까지 찾아서 집어넣어주기
    
    private void ChildSet()
    {
        
        GameObject tempObj;
        tempObj = this.gameObject.transform.GetChild(0).gameObject;
        itemImage = tempObj.GetComponent<Image>();

        GameObject tempObj002;
        tempObj002 = tempObj.gameObject.transform.GetChild(0).gameObject;
        itemCountImg = tempObj002.gameObject;

        // 맨위에 tempObj 재활용
        tempObj = tempObj002.gameObject.transform.GetChild(0).gameObject;
        text_Count = tempObj.GetComponent<TextMeshProUGUI>();
    }

    public void MoveItemSet()
    {        

        if(item != null && this.gameObject.transform.childCount > 0)
        {
            GameObject tempObj;
            tempObj = this.gameObject.transform.GetChild(0).gameObject;
            itemImage = tempObj.GetComponent<Image>();

            GameObject tempObj002;
            tempObj002 = tempObj.gameObject.transform.GetChild(0).gameObject;
            itemCountImg = tempObj002.gameObject;

            // 맨위에 tempObj 재활용
            tempObj = tempObj002.gameObject.transform.GetChild(0).gameObject;
            text_Count = tempObj.GetComponent<TextMeshProUGUI>();

            // 스프라이트와 텍스트 출력
            text_Count.text = itemCount.ToString();
            itemImage.sprite = item.itemImage;
        }
       
    }

    // 찾아넣은 아이템과의 연결 끊어주는 함수 -> 이미지,카운트텍스트,카운트이미지
    public void DisconnectedItem()
    {
        //text_Count.text = "0";
        text_Count = null;
        //itemImage.sprite = null;        
        itemImage = null;
        itemCountImg = null;
        item = null;
        itemCount = 0;
        Transform childTrans;
        if (this.transform.childCount > 0)
        {
            childTrans = this.transform.GetChild(0);
            Destroy(childTrans.gameObject);
            SG_ItemDragScript.allItemDragScrip.Remove(childTrans.GetComponent<SG_ItemDragScript>());
        }
    }

    // 아이템 슬롯 초기화
    public void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        text_Count.text = "0";
        // itemCountImg.SetActive(false);
    }


    private void GetThisTopParentObj()
    {        
        //최상위 부모 오브젝트 태그를 가져오기 위해 찾는 로직
        while (slotTopParentObj.transform.parent != null)
        {          
            slotTopParentObj = slotTopParentObj.transform.parent.gameObject;
        }
    }

}   // NameSpace
