using UnityEngine;

public class CameraParameter:MonoBehaviour
{
    public Camera n_camera;

    private void Start()
    {
        // �ػ� ����
        float imageWidth = n_camera.pixelWidth;
        float imageHeight = n_camera.pixelHeight;

        Debug.Log("imageWidth: " + imageWidth);
        Debug.Log("imageHeight: " + imageHeight);

        // ���� (cx, cy) ���
        float cx = imageWidth / 2.0f;
        float cy = imageHeight / 2.0f;

        // ���� �� ���� FOV
        float fovY = n_camera.fieldOfView;
        // float fovX = 2 * Mathf.Atan(Mathf.Tan(fovY * Mathf.Deg2Rad / 2) * maincamera.aspect) * Mathf.Rad2Deg;
        float fovX = 0.5f * imageWidth / Mathf.Tan(n_camera.fieldOfView * 0.5f * Mathf.Deg2Rad);

        // ���� �Ÿ� (fx, fy) ���
        float fy = (imageHeight / 2) / Mathf.Tan(fovY * Mathf.Deg2Rad / 2);
        float fx = (imageWidth / 2) / Mathf.Tan(fovY * Mathf.Deg2Rad / 2);

        // ī�޶��� �̵� ���� (Translation Vector)
        Vector3 translationVector = n_camera.transform.position;

        // ī�޶��� ȸ�� ��� (Rotation Matrix)
        Quaternion rotation = n_camera.transform.rotation;
        Matrix4x4 rotationMatrix = Matrix4x4.Rotate(rotation);

        // ī�޶��� ��ȯ ��� (Transformation Matrix)
        Matrix4x4 transformationMatrix = Matrix4x4.TRS(translationVector, rotation, Vector3.one);

        // ���
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