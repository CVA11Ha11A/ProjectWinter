using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SG_New Item", menuName = "SG_New Item/SG_item")]
public class SG_Item : ScriptableObject
{



    public string itemName; //아이템의 이름
    public Sprite itemImage; //아이템의 이미지

    public GameObject itemPrefab; // 아이템의 프리팹
    public GameObject craftingPrefab;   // 제작대에서 제작시 Instance될경우 나올 아이템

    public string weaponType; //무기 유형

    public ItemType itemType;

    public enum ItemType
    {
        // 무기,사용템,재료템,기타탬
        Weapon,
        Used,
        Ingredient,
        ETC
    }

}
