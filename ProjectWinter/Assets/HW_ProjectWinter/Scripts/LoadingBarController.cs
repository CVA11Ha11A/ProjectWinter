using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;

public class LoadingBarController : MonoBehaviour
{
    public AudioClip lodingMusic; // ����������� ����� ����� Ŭ��


    public Slider loadingSlider; // Slider UI ��Ҹ� �������ּ���.
    public float loadingTime = 9f; // �ε� �ð� (9�� ����)

    private float currentTime = 0f;

    void Start()
    {
        AudioSource.PlayClipAtPoint(lodingMusic, Camera.main.transform.position);

        loadingSlider.value = 0f; // �ʱ⿡ Slider�� FillAmount�� 0���� ����
        StartCoroutine(StartLoading());
    }

    IEnumerator StartLoading()
    {
        while (currentTime < loadingTime)
        {
            currentTime += Time.deltaTime;
            float progress = currentTime / loadingTime;
            loadingSlider.value = progress; // Slider�� FillAmount�� ���� ��Ȳ�� �°� ������Ʈ

            yield return null;
        }

        PhotonNetwork.LoadLevel("HW_PlayScene");

        // �ε��� �Ϸ�Ǹ� ���� �۾��� �����ϰų� ���� ������ �̵��� �� �ֽ��ϴ�.
    }
}