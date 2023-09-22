using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections;

public class ChatManager : MonoBehaviourPunCallbacks
{
    public Text chatText;
    public InputField chatInput;
    private string playerName;
    public Scrollbar scrollbar; // 스크롤바



    private void Start()
    {
        playerName = PhotonNetwork.LocalPlayer.NickName;
    }

    private void Update()
    {
        chatInput.ActivateInputField();

        if (Input.GetKeyDown(KeyCode.Return) && !string.IsNullOrEmpty(chatInput.text))
        {
            string message = playerName + ": " + chatInput.text;
            SendChatMessage(message);
            chatInput.text = "";

        }

    }

    private void SendChatMessage(string message)
    {
        photonView.RPC("ReceiveChatMessage", RpcTarget.All, message);
    }

    [PunRPC]
    private void ReceiveChatMessage(string message)
    {
        chatText.text += "\n" + message;

        // 스크롤바 값을 최대로 조정하여 아래로 스크롤
        scrollbar.value = 0f;

    }



}
