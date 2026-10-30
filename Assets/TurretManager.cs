using UnityEngine;

public class TurretManager : MonoBehaviour
{
    public GameObject laserTurret;
    public GameObject projectileTurret;
    public ProjectileTurret projectileTurretData;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            bool laserActive = laserTurret.activeInHierarchy;
            laserTurret.SetActive(!laserActive);
            projectileTurret.SetActive(laserActive);
        }

        if (Input.GetKeyDown(KeyCode.E) && projectileTurret.activeInHierarchy)
            projectileTurretData.useLowAngle = !projectileTurretData.useLowAngle;
    }
}
