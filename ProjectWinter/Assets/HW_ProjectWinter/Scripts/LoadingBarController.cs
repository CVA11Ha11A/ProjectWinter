using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;

public class LoadingBarController : MonoBehaviour
{
    public AudioClip lodingMusic; // 배경음악으로 사용할 오디오 클립


    public Slider loadingSlider; // Slider UI 요소를 연결해주세요.
    public float loadingTime = 9f; // 로딩 시간 (9초 예제)

    private float currentTime = 0f;

    void Start()
    {
        AudioSource.PlayClipAtPoint(lodingMusic, Camera.main.transform.position);

        loadingSlider.value = 0f; // 초기에 Slider의 FillAmount를 0으로 설정
        StartCoroutine(StartLoading());
    }

    IEnumerator StartLoading()
    {
        while (currentTime < loadingTime)
        {
            currentTime += Time.deltaTime;
            float progress = currentTime / loadingTime;
            loadingSlider.value = progress; // Slider의 FillAmount를 진행 상황에 맞게 업데이트

            yield return null;
        }

        PhotonNetwork.LoadLevel("HW_PlayScene");

        // 로딩이 완료되면 다음 작업을 수행하거나 다음 씬으로 이동할 수 있습니다.
    }
}
