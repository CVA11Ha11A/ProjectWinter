using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
//using UnityEditor.XR;

public class WaitingRoomManager : MonoBehaviourPunCallbacks
{

    public Slider masterVolumeSlider; // 마스터 볼륨을 조절하는 슬라이더
    private float initialMasterVolume; // 초기 마스터 볼륨 설정

    public AudioClip buttonClickSoundClip; // 버튼 클릭 사운드를 재생할 오디오 클립


    public GameObject optionUI; // NeighbourUI 전투시작 할 때 오른쪽 위에 ui


    public Text nicknameBoxOne;
    public Text nicknameBoxOne2;
    public Text nicknameBoxOne3;
    public Text nicknameBoxOne4;
    public Text nicknameBoxOne5;
    public Text nicknameBoxOne6;
    public Text userCount;

    public Image[] targetImages; // 불투명도를 조절할 대상 이미지 배열


    private Color originalColor; // 초기 이미지 색상
    private Color transparentColor; // 클릭 후 색상 

    private List<Text> nicknameBoxes = new List<Text>();

    // 변수를 추가하여 모든 플레이어의 readyButton 상태를 추적합니다.
    private Dictionary<string, bool> playerReadyStates = new Dictionary<string, bool>();

    public bool readyButton = true;


    // Start is called before the first frame updated
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;


        // 초기 마스터 볼륨을 저장
        initialMasterVolume = AudioListener.volume;

        // 슬라이더의 값을 초기 마스터 볼륨으로 설정
        masterVolumeSlider.value = initialMasterVolume;

        // 슬라이더의 값이 변경될 때마다 이벤트 핸들러 호출
        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeSliderChanged);


        // 텍스트 UI를 리스트에 추가
        nicknameBoxes.Add(nicknameBoxOne);
        nicknameBoxes.Add(nicknameBoxOne2);
        nicknameBoxes.Add(nicknameBoxOne3);
        nicknameBoxes.Add(nicknameBoxOne4);
        nicknameBoxes.Add(nicknameBoxOne5);
        nicknameBoxes.Add(nicknameBoxOne6);

        // 플레이어 입장 및 퇴장과 관련된 이벤트 핸들러 등록
        PhotonNetwork.AddCallbackTarget(this);


        //// 초기 이미지 색상을 저장
        originalColor = targetImages[0].color;

        // 클릭후 색상 
        transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 170f);
        UpdateNicknameText();
        UpdateUserCount();     
    }

    public void SetPlayerReady(bool isReady)
    {
        ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
        customProperties["IsReady"] = isReady;
        PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);
    }


    public bool AreAllPlayersReady()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            object isReady;
            if (player.CustomProperties.TryGetValue("IsReady", out isReady))
            {
                if (!(bool)isReady)
                {
                    return false; // 하나 이상의 플레이어가 아직 준비하지 않음
                }
            }
            else
            {
                return false; // 플레이어의 준비 상태 Custom Property가 없거나 값을 가져올 수 없음
            }
        }
        return true; // 모든 플레이어가 준비 완료
    }



    // 다른 플레이어가 방에 입장할 때 호출되는 콜백
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // 새로운 플레이어가 방에 입장하면 이벤트가 호출됩니다.
        // 이때 새로운 플레이어의 닉네임을 Text UI에 할당하여 표시합니다.
        playerReadyStates[newPlayer.NickName] = false;

        UpdateNicknameText();
        UpdateUserCount();
    }

    // 다른 플레이어가 방에서 퇴장할 때 호출되는 콜백
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // 다른 플레이어가 방에서 퇴장하면 이벤트가 호출됩니다.
        // 이때 해당 플레이어의 Text UI를 초기화하여 비웁니다.
        ResetNicknameText(otherPlayer.NickName);
        UpdateUserCount();
        UpdateNicknameText();
        AllResetNicknameText();
    }

    // Text UI에 플레이어 닉네임 표시 업데이트 메서드
    void UpdateNicknameText()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (i < nicknameBoxes.Count)
            {
                nicknameBoxes[i].text = PhotonNetwork.PlayerList[i].NickName;
            }
        }
    }

    // Text UI를 초기화하는 메서드
    void ResetNicknameText(string playerNickname)
    {

        foreach (var text in nicknameBoxes)
        {
            if (text.text == playerNickname)
            {
                text.text = "검색 중....."; // 해당 플레이어의 Text UI를 비웁니다.
            }
        }
    }


    void AllResetNicknameText()
    {
        for (int i = PhotonNetwork.PlayerList.Length; i < 6; i++)
        {
            nicknameBoxes[i].text = "검색 중.....";
            targetImages[i].color = originalColor;
        }
    }

    public void UpdateUserCount()
    {
        userCount.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + "/6";
    }





    //public void SetPlayerReadyState(string playerNickname, bool isReady)
    //{
    //    playerReadyStates[playerNickname] = isReady;
    //}

    //public bool AreAllPlayersReady()
    //{
    //    foreach (var rc in playerReadyStates)
    //    {
    //        if (rc.Value)
    //        {
    //            return false; // 하나 이상의 플레이어가 아직 준비하지 않음
    //        }
    //    }
    //    return true; // 모든 플레이어가 준비 완료
    //}



    // 게임 시작 버튼을 누를 때 호출되는 메서드
    public void StartGame()
    {
        if (AreAllPlayersReady())
        {
            // 모든 플레이어가 준비 상태일 경우, 씬 전환 또는 게임 시작 로직을 수행합니다.
            // 예를 들어, 씬을 전환하려면 다음과 같이 사용합니다.
            AudioSource.PlayClipAtPoint(buttonClickSoundClip, Camera.main.transform.position);
            Debug.Log("게임 시작 조건 충족: 모든 플레이어가 준비 상태입니다.");
            PhotonNetwork.LoadLevel("HW_LoadingScene");
        }
        else
        {
            Debug.Log("게임 시작 조건 미충족: 아직 모든 플레이어가 준비 상태가 아닙니다.");
            // 아직 모든 플레이어가 준비 상태가 아님을 메시지로 표시하거나 다른 동작을 수행합니다.
        }
    }






    // Update is called once per frame
    void Update()
    {

    }

    public void GetReadyButton()
    {

        string localPlayerNickname = PhotonNetwork.LocalPlayer.NickName;
        int playerIndex = GetPlayerIndex(localPlayerNickname);

        if (playerIndex >= 0 && playerIndex < targetImages.Length)
        {
            if (readyButton == true)
            {
                // 클릭 후 이미지 색상
                photonView.RPC("SetImageColor", RpcTarget.AllBuffered, playerIndex);
                readyButton = false;
                AudioSource.PlayClipAtPoint(buttonClickSoundClip, Camera.main.transform.position);
            }
            else if (readyButton == false)
            {
                photonView.RPC("UnSetImageColor", RpcTarget.AllBuffered, playerIndex);
                readyButton = true;
                AudioSource.PlayClipAtPoint(buttonClickSoundClip, Camera.main.transform.position);
            }
        }

        SetPlayerReady(!readyButton);

    }



    private int GetPlayerIndex(string playerNickname)
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i].NickName == playerNickname)
            {
                return i;
            }
        }
        return -1;
    }

    // RPC를 통해 이미지의 색상을 설정하는 메서드
    [PunRPC]
    public void SetImageColor(int playerIndex)
    {
        if (playerIndex >= 0 && playerIndex < targetImages.Length)
        {
            Image targetImageToUpdate = targetImages[playerIndex];

            if (targetImageToUpdate != null)
            {
                targetImageToUpdate.color = transparentColor;
            }
        }
    }



    // RPC를 통해 이미지의 색상을 설정하는 메서드
    [PunRPC]
    public void UnSetImageColor(int playerIndex)
    {
        if (playerIndex >= 0 && playerIndex < targetImages.Length)
        {
            Image targetImageToUpdate = targetImages[playerIndex];

            if (targetImageToUpdate != null)
            {
                targetImageToUpdate.color = originalColor;
            }
        }
    }



    public void LeftGame()
    {
        PhotonNetwork.Disconnect(); // 서버와의 연결을 끊음
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        // 서버와의 연결이 끊어지면 로비에 재접속
        readyButton = true;
        AudioSource.PlayClipAtPoint(buttonClickSoundClip, Camera.main.transform.position);
        PhotonNetwork.LoadLevel("HW_Lobby");
        PhotonNetwork.JoinLobby();
    }



    public void UIScale()
    {
        AudioSource.PlayClipAtPoint(buttonClickSoundClip, Camera.main.transform.position);

        Vector3 uiImageScale = optionUI.transform.localScale;

        Vector3 newScale = new Vector3(0.001f, 0.001f, 0.001f);

        optionUI.transform.localScale = newScale;
    }

    public void UIScale2()
    {
        AudioSource.PlayClipAtPoint(buttonClickSoundClip, Camera.main.transform.position);

        Vector3 uiImageScale = optionUI.transform.localScale;

        Vector3 newScale = new Vector3(5f, 5f, 5f);

        optionUI.transform.localScale = newScale;
    }


    // 슬라이더 값이 변경될 때 호출되는 메서드
    void OnMasterVolumeSliderChanged(float value)
    {
        // 슬라이더 값을 AudioListener.volume에 설정하여 마스터 볼륨 조절
        AudioListener.volume = value;
    }

}
