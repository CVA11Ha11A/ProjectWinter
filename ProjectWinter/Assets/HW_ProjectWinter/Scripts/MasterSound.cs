using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterSound : MonoBehaviour
{
    public Slider masterVolumeSlider; // 마스터 볼륨을 조절하는 슬라이더
    private float initialMasterVolume; // 초기 마스터 볼륨 설정

    void Start()
    {
        // 초기 마스터 볼륨을 저장
        initialMasterVolume = AudioListener.volume;

        // 슬라이더의 값을 초기 마스터 볼륨으로 설정
        masterVolumeSlider.value = initialMasterVolume;

        // 슬라이더의 값이 변경될 때마다 이벤트 핸들러 호출
        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeSliderChanged);
    }

    // 슬라이더 값이 변경될 때 호출되는 메서드
    void OnMasterVolumeSliderChanged(float value)
    {
        // 슬라이더 값을 AudioListener.volume에 설정하여 마스터 볼륨 조절
        AudioListener.volume = value;
    }
}
