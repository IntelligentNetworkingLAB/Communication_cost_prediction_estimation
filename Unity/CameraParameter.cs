using UnityEngine;

public class CameraParameter:MonoBehaviour
{
    public Camera n_camera;

    private void Start()
    {
        // 해상도 정보
        float imageWidth = n_camera.pixelWidth;
        float imageHeight = n_camera.pixelHeight;

        Debug.Log("imageWidth: " + imageWidth);
        Debug.Log("imageHeight: " + imageHeight);

        // 주점 (cx, cy) 계산
        float cx = imageWidth / 2.0f;
        float cy = imageHeight / 2.0f;

        // 수평 및 수직 FOV
        float fovY = n_camera.fieldOfView;
        // float fovX = 2 * Mathf.Atan(Mathf.Tan(fovY * Mathf.Deg2Rad / 2) * maincamera.aspect) * Mathf.Rad2Deg;
        float fovX = 0.5f * imageWidth / Mathf.Tan(n_camera.fieldOfView * 0.5f * Mathf.Deg2Rad);

        // 초점 거리 (fx, fy) 계산
        float fy = (imageHeight / 2) / Mathf.Tan(fovY * Mathf.Deg2Rad / 2);
        float fx = (imageWidth / 2) / Mathf.Tan(fovY * Mathf.Deg2Rad / 2);

        // 카메라의 이동 벡터 (Translation Vector)
        Vector3 translationVector = n_camera.transform.position;

        // 카메라의 회전 행렬 (Rotation Matrix)
        Quaternion rotation = n_camera.transform.rotation;
        Matrix4x4 rotationMatrix = Matrix4x4.Rotate(rotation);

        // 카메라의 변환 행렬 (Transformation Matrix)
        Matrix4x4 transformationMatrix = Matrix4x4.TRS(translationVector, rotation, Vector3.one);

        // 출력
        Debug.Log(n_camera.name + "Camera Intrinsics:");
        Debug.Log("cx: " + cx);
        Debug.Log("cy: " + cy);
        Debug.Log("fx: " + fx);
        Debug.Log("fy: " + fy);
        Debug.Log("T: " + translationVector);
        Debug.Log("R: " + rotationMatrix);
        Debug.Log(transformationMatrix);
    }
}