using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MovingScript : MonoBehaviour
{
    [Header("Moving Settings")]
    public Transform[] waypoints; // 웨이포인트 배열
    public float moveSpeed = 0.05f; // 이동 속도 (1초에 1미터)
    public float rotationSpeed = 0.05f;
    private int currentWaypointIndex = 0; // 현재 웨이포인트 인덱스
    private float distanceToMove; // 현재 이동해야 할 거리
    private Vector3 startPosition; // 시작 위치
    private Vector3 targetPosition; // 목표 위치
    private Quaternion startRotation; // 시작 회전
    private Quaternion targetRotation; // 목표 회전

    public Color waypointColor = Color.red;

    private void Start()
    {
        // 이동 관련 초기화
        if (waypoints.Length == 0) return;

        // 첫 번째 웨이포인트 설정
        startPosition = transform.position;
        targetPosition = waypoints[currentWaypointIndex].position;

        // 첫 번째 웨이포인트 회전 설정
        startRotation = transform.rotation;
        targetRotation = waypoints[currentWaypointIndex].rotation;

        // 5초마다 위치 출력
        InvokeRepeating("PrintHumanPosition", 5f, 5f);
    }

    private void Update()
    {
        if (waypoints.Length == 0) return;

        // 이동할 거리 계산
        distanceToMove = moveSpeed * Time.deltaTime; // 1초에 1미터 이동

        // 이동 거리 계산
        float distanceRemaining = Vector3.Distance(transform.position, targetPosition);
        if (distanceRemaining > distanceToMove)
        {
            // 목표 위치에 도달하지 않은 경우
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, distanceToMove);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = targetPosition;
            transform.rotation = targetRotation;

            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            startPosition = targetPosition;
            targetPosition = waypoints[currentWaypointIndex].position;
            startRotation = targetRotation;
            targetRotation = waypoints[currentWaypointIndex].rotation;
        }
    }

    private void PrintHumanPosition()
    {
        Vector3 position = transform.position;
        Debug.Log($"{gameObject.name}: ({position.x}, {position.y}, {position.z})");
    }

    void OnDrawGizmos()
    {
        if (waypoints.Length == 0) return;

        Gizmos.color = waypointColor;
        for (int i = 0; i < waypoints.Length; i++)
        {
            Vector3 startPosition = waypoints[i].position;
            Vector3 endPosition = waypoints[(i + 1) % waypoints.Length].position;
            Gizmos.DrawLine(startPosition, endPosition);
        }
    }
}
