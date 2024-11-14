using UnityEngine;

public class Change_Coord:MonoBehaviour
{
    public Camera cameraa;
    public Transform objectTransform;

    private void Start()
    {
        /*// 월드 좌표를 가져오기
        Vector3 objectWorldPosition = objectTransform.position;
        Debug.Log("Object World Position: " + objectWorldPosition);

        // 카메라의 뷰 행렬을 이용해 카메라 좌표계로 변환
        Vector3 objectCameraPosition = cameraa.worldToCameraMatrix.MultiplyPoint(objectWorldPosition);
        Debug.Log("Object Camera Position(카메라 행렬): " + objectCameraPosition);

        Debug.Log("Camera transformationMatrix: "+cameraa.worldToCameraMatrix);

        // 카메라의 로컬 좌표계로 직접 변환
        Vector3 objectCameraPosition2 = cameraa.transform.InverseTransformPoint(objectWorldPosition);
        //Debug.Log("Object Camera Position(직접 변환): " + objectCameraPosition2);

        Matrix4x4 transformationMatrix = Matrix4x4.TRS(cameraa.transform.position, cameraa.transform.rotation, Vector3.one).inverse;
        Debug.Log("transformationMatrix: " + transformationMatrix);

        Vector4 homogeneousPoint = new Vector4(objectCameraPosition.x, objectCameraPosition.y, objectCameraPosition.z, 1.0f);

        Vector4 worldPoint = transformationMatrix * homogeneousPoint;

        Debug.Log("Camera Point in World Coordinates: " + new Vector3(worldPoint.x, worldPoint.y, worldPoint.z));*/

        Vector3 cameraPosition = new Vector3(-9.5f, 2.8f, 14.5f);
        Vector3 cameraRotation = new Vector3(11f, 135f, 0f);

        // 월드 좌표
        Vector3 worldPosition = new Vector3(0f, 0f, 0f);

        // 카메라의 회전 행렬 생성
        Quaternion rotation = Quaternion.Euler(cameraRotation);
        Matrix4x4 rotationMatrix = Matrix4x4.Rotate(rotation);

        // 카메라 위치와의 차이 계산
        Vector3 toWorldPosition = worldPosition - cameraPosition;

        // 카메라 좌표계로 변환
        Vector3 cameraPositionInCameraSpace = rotationMatrix.inverse.MultiplyPoint3x4(toWorldPosition);

        Debug.Log("Camera Position in Camera Coordinates: " + cameraPositionInCameraSpace);

        //Vector3 cp = new Vector3(-4.4f, 0.5f, 18.3f);

        // 월드 좌표로 변환
        Vector3 worldPosition2 = cameraPosition + rotationMatrix.MultiplyPoint3x4(cameraPositionInCameraSpace);

        Debug.Log("World Position from Camera Coordinates: " + worldPosition2);
    }
}