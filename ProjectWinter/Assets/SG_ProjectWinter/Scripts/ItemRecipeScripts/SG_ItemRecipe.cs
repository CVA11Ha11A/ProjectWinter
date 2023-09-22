using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SG_ItemRecipe", menuName = "SG_New Item/SG_ItemRecipe")]
public class SG_ItemRecipe : ScriptableObject
{
    public SG_Item madeItem;        //아이템 완성본
    public int madeItemCount = 1;   //줄갯수

    public SG_Item item001;         //아이템 재료 1
    public int itemCount001 = 1;    //필요갯수

    public SG_Item item002;         //아이템 재료2
    public int itemCount002 = 1;    //필요갯수
}
