using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
//using static System.Net.Mime.MediaTypeNames;

public class NetworkManager : MonoBehaviourPunCallbacks
{


    public AudioClip backgroundMusic; // 배경음악으로 사용할 오디오 클립
    private AudioSource audioSource;

    public Text severText;
    public InputField roomInput, nickNameInput;

    public AudioClip buttonClickSoundClip; // 버튼 클릭 사운드를 재생할 오디오 클립


    private List<string> playerNicknames = new List<string>();




    void Awake() => Screen.SetResolution(1280  , 720, false);

    //void Start()
    //{
    //    RoomOptions roomOptions = new RoomOptions();
    //    roomOptions.CustomRoomPropertiesForLobby = new string[] { "IsReady" }; // Custom Property 동기화를 활성화할 속성 지정
    //roomOptions.CustomRoomProperties = new Hashtable() { { "IsReady", false } };

    //    // 방에 입장하거나 생성할 때 RoomOptions 설정
    //    PhotonNetwork.JoinOrCreateRoom("RoomName", roomOptions, TypedLobby.Default);
    //}


    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.loop = true; // 배경음악을 무한 반복 재생하도록 설정
        audioSource.Play();
    }
    void Update()
    {
    }



    public void Connect()
    {
        AudioSource.PlayClipAtPoint(buttonClickSoundClip, Camera.main.transform.position);
        PhotonNetwork.ConnectUsingSettings();
    } 

    public override void OnConnectedToMaster()
    {
        severText.text = "Connected Server";
        Debug.Log("서버접속완료");
        PhotonNetwork.LocalPlayer.NickName = nickNameInput.text;
    }



    public void Disconnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause) => print("연결끊김");



    public void JoinLobby() => PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby() => print("로비접속완료");



    public void CreateRoom() 
    {
        AudioSource.PlayClipAtPoint(buttonClickSoundClip, Camera.main.transform.position);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "IsReady" }; // Custom Property 동기화를 활성화할 속성 지정
        PhotonNetwork.CreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 6 });
    }

    public void JoinRoom()
    {
        AudioSource.PlayClipAtPoint(buttonClickSoundClip, Camera.main.transform.position);
        PhotonNetwork.JoinRoom(roomInput.text);
    }

    public void JoinOrCreateRoom()
    {
        PhotonNetwork.JoinOrCreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 6 }, null);
    }


    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    public override void OnCreatedRoom() => print("방만들기완료");

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("HW_WaitingRoom");
        print("방참가완료");

    }



    public override void OnCreateRoomFailed(short returnCode, string message) => print("방만들기실패");

    public override void OnJoinRoomFailed(short returnCode, string message) => print("방참가실패");

    public override void OnJoinRandomFailed(short returnCode, string message) => print("방랜덤참가실패");



    [ContextMenu("정보")]
    void Info()
    {
        if (PhotonNetwork.InRoom)
        {
            print("현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
            print("현재 방 인원수 : " + PhotonNetwork.CurrentRoom.PlayerCount);
            print("현재 방 최대인원수 : " + PhotonNetwork.CurrentRoom.MaxPlayers);

            string playerStr = "방에 있는 플레이어 목록 : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
            print(playerStr);
        }
        else
        {
            print("접속한 인원 수 : " + PhotonNetwork.CountOfPlayers);
            print("방 개수 : " + PhotonNetwork.CountOfRooms);
            print("모든 방에 있는 인원 수 : " + PhotonNetwork.CountOfPlayersInRooms);
            print("로비에 있는지? : " + PhotonNetwork.InLobby);
            print("연결됐는지? : " + PhotonNetwork.IsConnected);
        }
    }
}