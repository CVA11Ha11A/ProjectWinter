using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Photon.Pun;

public class PressEKey : MonoBehaviourPun
{
    public GameObject completeUi;   // 완료! UI
    public GameObject noticeUi;     // 알림 UI
    public Text noticeText;         // 알림 UI의 텍스트
    public Slider progressBar;      // E버튼 진행도
    public bool isComplete = false; // 수리나 작동을 완료했는지

    private bool isEPressed = false;// 누르고 있는지 확인
    private float ePressStartTime = 0f;
    private float ePressDuration = 1.5f; // 1.5초 동안 눌러야 함

    private float currentValue = 0f;     // 현재 Filled타입의 채워진 양
    private Coroutine fillingCoroutine;  
    

    void Start()
    {
        if (!transform.parent.CompareTag("Untagged"))
        {
            noticeText.text = string.Format("{0}", UIManager.instance.FormatNoticeText(transform.parent));
        }
        completeUi.SetActive(false);
        progressBar.value = 0;
        isComplete = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isComplete)
        {
            isEPressed = true;
            ePressStartTime = Time.deltaTime;

            if (fillingCoroutine != null)
            {
                StopCoroutine(fillingCoroutine);
            }

            fillingCoroutine = StartCoroutine(FillProgressBar());
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            isEPressed = false;
            if(progressBar.value < 100f)
            {
                progressBar.value = 0f;
            }
            StopCoroutine(fillingCoroutine);
        }
    }

    IEnumerator FillProgressBar()
    {
        float elapsedTime = 0f;

        while (elapsedTime < ePressDuration)
        {
            currentValue = Mathf.Lerp(0, 100, elapsedTime / ePressDuration);
            progressBar.value = currentValue;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (isEPressed)
        {
            currentValue = 100f;
            progressBar.value = currentValue;
            Desc001();
        }
        else
        {
            // E 키를 누르지 않고 1.5초가 지나면 초기화
            currentValue = 0f;
            progressBar.value = currentValue;
        }
    }

    public void Desc001()
    {
        photonView.RPC("CallCompleteUi", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void CallCompleteUi()
    {
        Debug.Log("E 키를 1.5초 동안 눌렀습니다!");
        isComplete = true;
        noticeUi.SetActive(false);
        completeUi.SetActive(true);
        Invoke("OffCompleteUi", 2.0f);
    }

    [PunRPC]
    public void OffCompleteUi()
    {
        noticeUi.SetActive(true);
        progressBar.value = 0f;
        isComplete = false;
        completeUi.SetActive(false);
    }
}
