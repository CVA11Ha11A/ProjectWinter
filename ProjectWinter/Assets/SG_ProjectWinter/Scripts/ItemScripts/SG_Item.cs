using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "SG_New Item", menuName = "SG_New Item/SG_item")]
public class SG_Item : ScriptableObject
{



    public string itemName; //아이템의 이름

    public Sprite itemImage; //아이템의 이미지

    public GameObject itemPrefab; // 아이템의 프리팹    

    public string weaponType; //무기 유형

    public int itemDamage;  // 아이템의 데미지

    public int itemHealth;  // 아이템의 힐량

    public int itemWarmth; // 사용시 올라갈 플레이어 온도

    public int itemSatiety; // 사용시 올라갈 플레이어 포만감

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
