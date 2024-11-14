using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MovingScript : MonoBehaviour
{
    [Header("Moving Settings")]
    public Transform[] waypoints; // ��������Ʈ �迭
    public float moveSpeed = 0.05f; // �̵� �ӵ� (1�ʿ� 1����)
    public float rotationSpeed = 0.05f;
    private int currentWaypointIndex = 0; // ���� ��������Ʈ �ε���
    private float distanceToMove; // ���� �̵��ؾ� �� �Ÿ�
    private Vector3 startPosition; // ���� ��ġ
    private Vector3 targetPosition; // ��ǥ ��ġ
    private Quaternion startRotation; // ���� ȸ��
    private Quaternion targetRotation; // ��ǥ ȸ��

    public Color waypointColor = Color.red;

    private void Start()
    {
        // �̵� ���� �ʱ�ȭ
        if (waypoints.Length == 0) return;

        // ù ��° ��������Ʈ ����
        startPosition = transform.position;
        targetPosition = waypoints[currentWaypointIndex].position;

        // ù ��° ��������Ʈ ȸ�� ����
        startRotation = transform.rotation;
        targetRotation = waypoints[currentWaypointIndex].rotation;

        // 5�ʸ��� ��ġ ���
        InvokeRepeating("PrintHumanPosition", 5f, 5f);
    }

    private void Update()
    {
        if (waypoints.Length == 0) return;

        // �̵��� �Ÿ� ���
        distanceToMove = moveSpeed * Time.deltaTime; // 1�ʿ� 1���� �̵�

        // �̵� �Ÿ� ���
        float distanceRemaining = Vector3.Distance(transform.position, targetPosition);
        if (distanceRemaining > distanceToMove)
        {
            // ��ǥ ��ġ�� �������� ���� ���
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, distanceToMove);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            // ��ǥ ��ġ�� ������ ���
            transform.position = targetPosition;
            transform.rotation = targetRotation;

            // ���� ��������Ʈ�� �̵�
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            startPosition = targetPosition;
            targetPosition = waypoints[currentWaypointIndex].position;
            startRotation = targetRotation;
            targetRotation = waypoints[currentWaypointIndex].rotation;
        }
    }

    // 5�ʸ��� ��ġ ���
    private void PrintHumanPosition()
    {
        Vector3 position = transform.position;
        Debug.Log($"{gameObject.name}: ({position.x}, {position.y}, {position.z})");
    }

    // ����׿� ��� �ð�ȭ
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
