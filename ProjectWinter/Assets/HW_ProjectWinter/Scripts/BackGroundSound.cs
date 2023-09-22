using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public class BackGroundSound : MonoBehaviour
{
    public VideoPlayer video;
    public Slider volumeSlider; // 슬라이더 오브젝트

    private void Start()
    {
        // 슬라이더의 값이 변경될 때 이벤트 핸들러 등록
        volumeSlider.onValueChanged.AddListener(OnVolumeSliderChanged);
    }

    // 슬라이더 값이 변경될 때 호출되는 메서드
    void OnVolumeSliderChanged(float value)
    {
        // 슬라이더 값으로 비디오 플레이어의 볼륨 조절
        video.SetDirectAudioVolume(0, value);
    }
}
