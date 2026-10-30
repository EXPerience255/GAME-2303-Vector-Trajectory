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

    List<Vector3> laserPoints = new List<Vector3>();

    void Update()
    {
        TrackMouse();
        TurnBase();

        laserPoints.Clear();
        laserPoints.Add(barrelEnd.position);

        if (Physics.Raycast(barrelEnd.position, barrelEnd.forward, out RaycastHit hit, 1000.0f, targetLayer))
        {
            laserPoints.Add(hit.point);

            float dot = Vector3.Dot(barrelEnd.forward, hit.normal) * -2;
            Vector3 reflect = new Vector3
            {
                x = barrelEnd.forward.x + hit.normal.x * dot,
                y = barrelEnd.forward.y + hit.normal.y * dot,
                z = barrelEnd.forward.z + hit.normal.z * dot
            };
            reflect = reflect.normalized;

            if (Physics.Raycast(hit.point, reflect, out RaycastHit hit2, 1000.0f, targetLayer)) laserPoints.Add(hit2.point);
            else laserPoints.Add(hit.point + reflect * 1000);
        }
        else laserPoints.Add(barrelEnd.position + barrelEnd.forward * 1000);

        line.positionCount = laserPoints.Count;
        for(int i = 0; i < line.positionCount; i++)
        {
            line.SetPosition(i, laserPoints[i]);
        }
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
