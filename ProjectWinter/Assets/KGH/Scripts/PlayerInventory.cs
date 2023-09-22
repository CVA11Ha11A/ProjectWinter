using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SG_Item;
using static UnityEditor.Progress;

public class PlayerInventory : MonoBehaviour
{
    // 폐기예정
    public List<GameObject> inventory;

    private PlayerController controller;

    public SG_Item item;

    private MouseScroll mouseScroll;
    public SG_Inventory playerinventory;
    public float inven;        // 선택된 인벤슬롯

    public float hp;
    public float cold;
    public float hunger;

    public float damage;

    public int invenNumber;

    public bool weapomInHand = false;
    public bool foodInHand = false;

    //public bool 
    private void Start()
    {
        mouseScroll = transform.GetComponent<MouseScroll>();

        controller = transform.GetComponent<PlayerController>();

        playerinventory = gameObject.GetComponent<SG_Inventory>();

        item = gameObject.GetComponent<SG_Item>();
    }

    private void Update()
    {
        inven = mouseScroll.slot;
       
        for(int i = 0; i < inventory.Count; i++)
        {         
            if((int)inven - 1 == i)
            {
                inventory[i].SetActive(true);
                controller.inven = inventory[i].transform;
                invenNumber = i;

                if(inventory[i].transform.GetChild(0) != null)
                {
                    controller.itemInHand = inventory[i].transform.GetChild(0);
                }
                //else
                //{
                //    controller.itemInHand = null;
                //}
            }
            else
            {
                inventory[i].SetActive(false);
            }            
        }       

        if (playerinventory.slots[(int)inven].item != null)
        {
            if (playerinventory.slots[(int)inven].item.itemType == ItemType.Weapon)
            {
                Damage();
                weapomInHand = true;
                foodInHand = false;
            }
            else if (playerinventory.slots[(int)inven].item.itemType == ItemType.Used)
            {
                Heal();
                foodInHand = true;
                weapomInHand = false;
            }
            else
            {
                weapomInHand = false;
                foodInHand = false;
            }
        }
    }

    private void Heal()
    {
        if (item.itemName == "Carrot")
        {
            hp = 10;
            cold = 5;
            hunger = 20;
        }
    }

    private void Damage()
    {
        if (item.name == "")
        {
            damage = 0;
        }
    }
}
