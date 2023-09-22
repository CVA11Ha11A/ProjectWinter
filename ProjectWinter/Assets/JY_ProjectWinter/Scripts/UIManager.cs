using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public enum PositionState
    {
        North,
        South,
        East,
        West
    }

    public PositionState positionState;

    // 싱글톤 접근용 프로퍼티
    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
            }

            return m_instance;
        }
    }

    private static UIManager m_instance; // 싱글톤이 할당될 변수

    public Image[] miniMapImg;  // 미니맵 이미지 배열 (차례대로 북,남,동,서)
    private Color standingColor = new Color(0.443f, 0.443f, 0.443f);
    private Color basicColor = new Color (1f, 1f, 1f);
    public Text powerStationText;
    public Text heliPadText;
    public Text escapeText;

    private void Start()
    {
        positionState = PositionState.North;
    }

    public void UpdatePowerStaionText()
    {
        powerStationText.text
            = string.Format("발전소 상태 : 수리 완료");
    }

    public void UpdateHeliPadText()
    {
        heliPadText.text = string.Format("헬리패드 상태 : 수리 완료");
    }

    public void UpdateEscapeText()
    {
        escapeText.text = string.Format("탈출 차량 접근 중.");
    }

    public string FormatNoticeText(Transform transform_)
    {
        if(transform_.CompareTag("Operate"))
        {
            //text_.text = string.Format("작동하기");
            return "작동하기";
        }
        else if(transform_.CompareTag("Repair"))
        {
            //text_.text = string.Format("수리하기");
            return "수리하기";
        }
        else if(transform_.CompareTag("Box"))
        {
            return "열어보기";
        }
        return "알림 내용";
    }

    public void ChangeMiniMap()
    {
        for (int i = 0; i < miniMapImg.Length; i++)
        {
            miniMapImg[i].color = basicColor;
        }
        switch (positionState)
        {
            case PositionState.North:
                miniMapImg[0].color = standingColor;
                break;
            case PositionState.South:
                miniMapImg[1].color = standingColor;
                break;
            case PositionState.East:
                miniMapImg[2].color = standingColor;
                break;
            case PositionState.West:
                miniMapImg[3].color = standingColor;
                break;
            default:
                break;
        }
    }
}
