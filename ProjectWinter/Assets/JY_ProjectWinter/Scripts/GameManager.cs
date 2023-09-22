using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using UnityEngine.SocialPlatforms.Impl;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static GameManager instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<GameManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }

    public static GameManager m_instance; // 싱글톤이 할당될 static 변수

    public GameObject playerPrefab; // 생성할 플레이어 캐릭터 프리팹
    public GameObject survivorRolePanel;    // 생존자 역할일 경우 킬 UI
    public GameObject traitorRolePanel;     // 배신자 역할일 경우 킬 UI
    public ProgressManager progressManager; // 진행도에 따른 스크립트 온오프 관리 스크립트

    public TMP_Text timerText;      // 눈보라 제한시간 텍스트
    public float limitTime = 900.0f; // 15분은 900초
    private float currentTime;

    public int traitorNum;      // 배신자 역할의 ActorNumber
    public GameObject[] playerObjects;  // 플레이어 오브젝트들 모음
    public List<int> escapePlayerList;  // 탈출하는 사람들의 액터넘버 리스트

    public bool isLimitOver = false;
    public bool isRepairPowerStation = false;
    public bool isRepairHeliPad = false;
    public bool isCallHeli = false;

    // 주기적으로 자동 실행되는, 동기화 메서드
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 바꿔서 사용하기
        //// 로컬 오브젝트라면 쓰기 부분이 실행됨
        //if (stream.IsWriting)
        //{
        //    // 네트워크를 통해 score 값을 보내기
        //    stream.SendNext(score);
        //}
        //else
        //{
        //    // 리모트 오브젝트라면 읽기 부분이 실행됨         

        //    // 네트워크를 통해 score 값 받기
        //    score = (int)stream.ReceiveNext();
        //    // 동기화하여 받은 점수를 UI로 표시
        //    UIManager.instance.UpdateScoreText(score);
        //}
    }
    void Awake()
    {
        //캐릭터 산장 앞에 생성 (좌표 x : -200f z : 287f)
        PhotonNetwork.Instantiate("Player", new Vector3(-200f, 1f, 287f), Quaternion.identity);
    }
    // Start is called before the first frame update
    void Start()
    {       
        //역할 부여하기
        if (PhotonNetwork.IsMasterClient)
        {
            traitorNum = Random.Range(1, PhotonNetwork.PlayerList.Length + 1);
            photonView.RPC("CastTraitor", RpcTarget.AllBuffered, traitorNum);
        }      
        //타이머 시작
        currentTime = limitTime;
        StartCoroutine(UpdateTimer());
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 플레이어마다 자신의 ActorNumber를 확인하여 배신자 번호일 경우 배신자 역할 판넬이 켜짐
    [PunRPC]
    public void CastTraitor(int traitorNum_)
    {
        traitorNum = traitorNum_;
        if (PhotonNetwork.LocalPlayer.ActorNumber == traitorNum_)
        {
            traitorRolePanel.SetActive(true);
        }
        else
        {
            survivorRolePanel.SetActive(true);
        }
    }

    private IEnumerator UpdateTimer()
    {
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime; // 시간을 줄입니다.

            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);

            string timeText = string.Format("{0:00}:{1:00}", minutes, seconds);

            timerText.text = timeText; // 타이머를 UI 텍스트로 출력합니다.

            yield return null; // 다음 프레임까지 대기
        }
        isLimitOver = true;
    }

    public void RepairPowerStation()
    {
        isRepairPowerStation = true;  
        UIManager.instance.UpdatePowerStaionText();
        progressManager.OpenRepairHeliPad();
    }

    public void RepairHelipad()
    {
        isRepairHeliPad = true;
        UIManager.instance.UpdateHeliPadText();
        progressManager.OpenCallHeliRadio();
    }

    public void CallHeli()
    {
        isCallHeli = true;
        UIManager.instance.UpdateEscapeText();
        progressManager.OpenHelicopter();
        playerObjects = GameObject.FindGameObjectsWithTag("Player");
    }
}
