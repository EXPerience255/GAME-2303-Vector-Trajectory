using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurret : MonoBehaviour
{
    [SerializeField] LayerMask targetLayer;
    [SerializeField] GameObject crosshair;
    [SerializeField] float baseTurnSpeed = 3;
    [SerializeField] GameObject gun;
    [SerializeField] Transform turretBase;
    [SerializeField] Transform barrelEnd;
    [SerializeField] LineRenderer line;
    [SerializeField] int maxBounces;

    List<Vector3> laserPoints = new List<Vector3>();

    void Update()
    {
        TrackMouse();
        TurnBase();

        laserPoints.Clear();
        laserPoints.Add(barrelEnd.position);
        GenerateLine(barrelEnd.position, barrelEnd.forward);

        line.positionCount = laserPoints.Count;
        for(int i = 0; i < line.positionCount; i++)
        {
            line.SetPosition(i, laserPoints[i]);
        }
    }

    private void GenerateLine(Vector3 stepPosition, Vector3 stepDirection, int bounces = 0)
    {
        if (bounces > maxBounces) return;

        if (Physics.Raycast(stepPosition, stepDirection, out RaycastHit hit, 1000.0f, targetLayer))
        {
            laserPoints.Add(hit.point);

            float dot = Vector3.Dot(stepDirection, hit.normal) * -2;
            Vector3 reflect = new Vector3
            {
                x = stepDirection.x + hit.normal.x * dot,
                y = stepDirection.y + hit.normal.y * dot,
                z = stepDirection.z + hit.normal.z * dot
            };
            reflect = reflect.normalized;

            GenerateLine(hit.point, reflect, bounces + 1);
        }
        else laserPoints.Add(stepPosition + stepDirection * 1000);
    }

    void TrackMouse()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(cameraRay, out RaycastHit hit, 1000, targetLayer))
        {
            crosshair.transform.forward = hit.normal;
            crosshair.transform.position = hit.point + hit.normal * 0.1f;
        }
    }

    void TurnBase()
    {
        Vector3 directionToTarget = (crosshair.transform.position - turretBase.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToTarget.x, directionToTarget.y, directionToTarget.z));
        turretBase.transform.rotation = Quaternion.Slerp(turretBase.transform.rotation, lookRotation, Time.deltaTime * baseTurnSpeed);
    }
}
