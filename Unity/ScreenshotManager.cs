using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScreenshotManager : MonoBehaviour
{
    public Camera[] screenshotCameras; // 스크린샷을 찍을 카메라 배열
    public float captureInterval = 5f; // 스크린샷 촬영 간격 (초 단위)

    private int resWidth;
    private int resHeight;
    private string path;

    private void Start()
    {
        // 해상도 설정 및 파일 경로 설정
        resWidth = Screen.width;
        resHeight = Screen.height;
        path = Application.dataPath + "/ScreenShot/";
        Debug.Log(path);

        // 일정 시간마다 스크린샷 촬영
        StartCoroutine(CaptureScreenshots());
    }

    private IEnumerator CaptureScreenshots()
    {
        while (true)
        {
            yield return new WaitForSeconds(captureInterval);

            foreach (Camera cam in screenshotCameras)
            {
                // RenderTexture 생성 및 설정
                RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
                cam.targetTexture = rt;

                // 스크린샷 저장
                SaveScreenShot(cam, rt);

                // RenderTexture 해제
                cam.targetTexture = null;
                RenderTexture.active = null;

                // 리소스 해제
                Destroy(rt);
            }

            Debug.Log("Screenshots captured.");
        }
    }

    private void SaveScreenShot(Camera cam, RenderTexture rt)
    {
        // Texture2D 생성
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);

        // 카메라로부터 렌더링 및 읽기
        cam.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        screenShot.Apply();

        // 파일 경로 및 이름 설정
        string name = path + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + "_" + cam.name + ".png";

        // PNG로 저장
        byte[] bytes = screenShot.EncodeToPNG();
        File.WriteAllBytes(name, bytes);

        // Texture2D 리소스 해제
        Destroy(screenShot);
    }
}
