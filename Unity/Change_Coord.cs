using UnityEngine;

public class Change_Coord:MonoBehaviour
{
    public Camera cameraa;
    public Transform objectTransform;

    private void Start()
    {
        /*// ���� ��ǥ�� ��������
        Vector3 objectWorldPosition = objectTransform.position;
        Debug.Log("Object World Position: " + objectWorldPosition);

        // ī�޶��� �� ����� �̿��� ī�޶� ��ǥ��� ��ȯ
        Vector3 objectCameraPosition = cameraa.worldToCameraMatrix.MultiplyPoint(objectWorldPosition);
        Debug.Log("Object Camera Position(ī�޶� ���): " + objectCameraPosition);

        Debug.Log("Camera transformationMatrix: "+cameraa.worldToCameraMatrix);

        // ī�޶��� ���� ��ǥ��� ���� ��ȯ
        Vector3 objectCameraPosition2 = cameraa.transform.InverseTransformPoint(objectWorldPosition);
        //Debug.Log("Object Camera Position(���� ��ȯ): " + objectCameraPosition2);

        Matrix4x4 transformationMatrix = Matrix4x4.TRS(cameraa.transform.position, cameraa.transform.rotation, Vector3.one).inverse;
        Debug.Log("transformationMatrix: " + transformationMatrix);

        Vector4 homogeneousPoint = new Vector4(objectCameraPosition.x, objectCameraPosition.y, objectCameraPosition.z, 1.0f);

        Vector4 worldPoint = transformationMatrix * homogeneousPoint;

        Debug.Log("Camera Point in World Coordinates: " + new Vector3(worldPoint.x, worldPoint.y, worldPoint.z));*/

        Vector3 cameraPosition = new Vector3(-9.5f, 2.8f, 14.5f);
        Vector3 cameraRotation = new Vector3(11f, 135f, 0f);

        // ���� ��ǥ
        Vector3 worldPosition = new Vector3(0f, 0f, 0f);

        // ī�޶��� ȸ�� ��� ����
        Quaternion rotation = Quaternion.Euler(cameraRotation);
        Matrix4x4 rotationMatrix = Matrix4x4.Rotate(rotation);

        // ī�޶� ��ġ���� ���� ���
        Vector3 toWorldPosition = worldPosition - cameraPosition;

        // ī�޶� ��ǥ��� ��ȯ
        Vector3 cameraPositionInCameraSpace = rotationMatrix.inverse.MultiplyPoint3x4(toWorldPosition);

        Debug.Log("Camera Position in Camera Coordinates: " + cameraPositionInCameraSpace);

        //Vector3 cp = new Vector3(-4.4f, 0.5f, 18.3f);

        // ���� ��ǥ�� ��ȯ
        Vector3 worldPosition2 = cameraPosition + rotationMatrix.MultiplyPoint3x4(cameraPositionInCameraSpace);

        Debug.Log("World Position from Camera Coordinates: " + worldPosition2);
    }
}