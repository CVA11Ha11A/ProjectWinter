using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SG_PlayerActionControler : MonoBehaviour
{

    [SerializeField]
    private float range;    //습득 거리

    private bool pickupActivated = false;   //습득 가능할 때 true

    private RaycastHit hitInfo; // 충돌체 저장

    // 아이템 레이어에만 반응 하도록 레이어 마스크 설정
    [SerializeField]
    private LayerMask itemLayerMask;
    // 웨어 하우스가 아니면 조건을 통과하지 못하게 해줄 LayerMask 선언
    [SerializeField]
    private LayerMask whreHouseMask;


    // 필요한 컴포넌트
    [SerializeField]
    private TextMeshProUGUI actionText;

    [SerializeField]
    private SG_Inventory theInventory;

    //창고
    // 창고를 열기위한  플레이어의 이벤트 선언 창고가 인식하면 창고를 열고 닫고 해줄 이벤트 델리게이트
    public delegate void WareHouseDelegate();
    public event WareHouseDelegate WareHouseEvent;

    // 발전기
    public delegate void PowerStationInventoryDelegate();
    // event를 델리게이트를 지정해서 델리게이트가 가지고 있는 void 의 리턴값과 매개변수가 없어야한다는 조건을넣어준샘이됨
    public event PowerStationInventoryDelegate PowerStationInventoryEvent;

    // 헬리페드
    public delegate void HeliPadInventoryDelegate();
    public event PowerStationInventoryDelegate HeliPadInventoryEvent;

    // 제작대
    public delegate void WorkStationOpenDelegate();
    public event WorkStationOpenDelegate WorkStationOpenEvent;

    public event System.Action<SG_Inventory> tossInventoryEvent;

    //public SG_Inventory inventoryClass;

    private void Awake()
    {

    }

    void Start()
    {
        theInventory.ItemDestroyEvent += ItemDestroy;
    }

    // Update is called once per frame
    void Update()
    {
        CheckItem();
        CheckOpenObj();
        TryAction();

    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckItem();
            CanPickUp();
        }
    }

    // 레이를 쏘아서 맞은것이 Item 이라는 Layer를 가지고 있다면통과하는 로직
    private void CheckItem()
    {
        if (Physics.Raycast
            (transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, itemLayerMask))
        {
            if (hitInfo.transform.CompareTag("Item"))
            {
                ItemInfoAppear();
            }
        }
        else { InfoDisappear(); }
    }


    // 레이를 쏘아서 아이템이라는 레이어 마스크가 있으면 아이템이름 E 키를 눌러라 라는 text 출력하는 함수
    private void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<SG_ItemPickUp>().item.itemName
            + " Get Input Key E";
    }

    // 아이템 줍고나면 E 를 누르라는 text 끄는 함수
    private void InfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }

    // 아이템을 인벤토리에 넣는 함수
    private void CanPickUp()
    {
        if (pickupActivated == true)
        {
            if (hitInfo.transform != null)
            {
                //Debug.Log("아이템 획득");
                theInventory.AcquireItem(hitInfo.transform.GetComponent<SG_ItemPickUp>().item);
                //Destroy(hitInfo.transform.gameObject);
                InfoDisappear();

            }
        }
    }

    // TODO : 위 함수가 발동 되었을때에 인벤토리에서 비어있는곳을 찾아 넣는데 들어갔는지 못들어갔는지 확인하고
    //   아이템이 잘 들어갔으면 인벤토리가 ActionControler의 Destroy함수를 발동해주고 들어가지 못했다면 return
    //      하도록 만들어봐야겠음

    // 아이템을 먹었다면 Distroy 해줄 함수
    public void ItemDestroy()
    {
        Destroy(hitInfo.transform.gameObject);
    }

    // ---------------------------------- 인벤토리 Open 함수 -----------------------------------

    private void CheckOpenObj()
    {
        if (Physics.Raycast
        (transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range))
        {

            if (hitInfo.transform.CompareTag("Warehouse"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //Debug.Log("레이를 쏘고 창고가 맞을때 E 를 눌러야한다는 조건이 잘들어오나?");
                    // 산장창고 열고 닫는 함수 부르기
                    WareHouseEvent?.Invoke();
                }
            }
            else { /*PASS*/ }

            //if (GameManager.instance.isRepairPowerStation == false)   // 미션 끝나면 발전소 못열게 할 if 문
            //{

            if (hitInfo.transform.CompareTag("PowerStation"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    PowerStationInventoryEvent?.Invoke();
                }
            }
            else { /*PASS*/ }
            //}   // 미션 끝나면 발전소 못열게 할 if 문

            //if (GameManager.instance.isRepairHeliPad == false)  // 미션 끝나면 헬리패드 못열게 할 if 문
            //{
            if (hitInfo.transform.CompareTag("HeliPad"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    HeliPadInventoryEvent?.Invoke();
                }
            }
            else { /*PASS*/ }
            //}   // 미션 끝나면 헬리패드 못열게 할 if 문

            if (hitInfo.transform.CompareTag("Workstation"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    WorkStationOpenEvent?.Invoke();
                    //Invoke한뒤에 action event로 불러오면 될듯
                    tossInventoryEvent?.Invoke(theInventory);
                }
            }
            else { /*PASS*/ }



        }   // Ray IF

    }   //CheckOpenObj()

}   // NAMESPACE
