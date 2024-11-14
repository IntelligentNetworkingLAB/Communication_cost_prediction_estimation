using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScreenshotManager : MonoBehaviour
{
    public Camera[] screenshotCameras; // ��ũ������ ���� ī�޶� �迭
    public float captureInterval = 5f; // ��ũ���� �Կ� ���� (�� ����)

    private int resWidth;
    private int resHeight;
    private string path;

    private void Start()
    {
        // �ػ� ���� �� ���� ��� ����
        resWidth = Screen.width;
        resHeight = Screen.height;
        path = Application.dataPath + "/ScreenShot/";
        Debug.Log(path);

        // ���� �ð����� ��ũ���� �Կ�
        StartCoroutine(CaptureScreenshots());
    }

    private IEnumerator CaptureScreenshots()
    {
        while (true)
        {
            yield return new WaitForSeconds(captureInterval);

            foreach (Camera cam in screenshotCameras)
            {
                // RenderTexture ���� �� ����
                RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
                cam.targetTexture = rt;

                // ��ũ���� ����
                SaveScreenShot(cam, rt);

                // RenderTexture ����
                cam.targetTexture = null;
                RenderTexture.active = null;

                // ���ҽ� ����
                Destroy(rt);
            }

            Debug.Log("Screenshots captured.");
        }
    }

    private void SaveScreenShot(Camera cam, RenderTexture rt)
    {
        // Texture2D ����
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);

        // ī�޶�κ��� ������ �� �б�
        cam.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        screenShot.Apply();

        // ���� ��� �� �̸� ����
        string name = path + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + "_" + cam.name + ".png";

        // PNG�� ����
        byte[] bytes = screenShot.EncodeToPNG();
        File.WriteAllBytes(name, bytes);

        // Texture2D ���ҽ� ����
        Destroy(screenShot);
    }
}
