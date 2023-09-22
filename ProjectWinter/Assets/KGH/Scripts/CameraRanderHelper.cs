using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRanderHelper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Canvas canvas = GetComponent<Canvas>();

        if (canvas != null)
        {
            // Canvas의 렌더링 카메라를 설정합니다.
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = Camera.main; // 또는 다른 원하는 카메라로 설정합니다.
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
