using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shot : MonoBehaviour
{
    private GameObject pointOfIntersection, projectile, containerTrajectory;
    public GameObject addPoint, containerPrefab, projectilePrefab,tracePrefab,cam;
    private MoveMain moveMain;

    private Vector2 ppoPosition,point;
    bool isLock = false;
    

    private void Start()
    {
        Vector3 ppoPositionV3 = GetComponent<Transform>().position;
        ppoPosition = new Vector2(ppoPositionV3.x, ppoPositionV3.y);
        moveMain = cam.GetComponent<MoveMain>();
        
    }
    void FixedUpdate()
    {
        if (projectile)
        {
            projectile.transform.Translate(new Vector2(1, 0) * moveMain.projectileSpeed * Time.fixedDeltaTime);
            Vector2 randomVector = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            Vector2 posVector = new Vector2(projectile.transform.position.x, projectile.transform.position.y);
            Instantiate(tracePrefab, posVector + randomVector, Quaternion.identity);

        }

        if (containerTrajectory)
        {
            Destroy(containerTrajectory);
        }

        pointOfIntersection = GameObject.Find("pointOfIntersection(Clone)");

        if (!pointOfIntersection)
        {
            return;
        }

        Vector3 targetMiddle = pointOfIntersection.transform.position;
        Vector2 targetAbs = new Vector2(targetMiddle.x, targetMiddle.y);
        Vector2 target = targetAbs - ppoPosition;
        Vector2 targetNorm = target.normalized;
        // GameObject projectileTrajectoryContainer = Instantiate(containerPrefab, new Vector2(0, 0), Quaternion.identity);
        containerTrajectory = Instantiate(containerPrefab, new Vector2(0, 0), Quaternion.identity);
        for (int i = 1; i < 300; i += 5)
        {
            GameObject elem = Instantiate(addPoint, new Vector2(0, 0), Quaternion.identity);
            elem.transform.parent = containerTrajectory.transform;
            elem.transform.position = targetNorm * i + ppoPosition;
        }

        if ((Input.GetKey(KeyCode.Space)&&!isLock)){           
            isLock = true;                
            GameObject container = Instantiate(containerPrefab, new Vector2(0, 0), Quaternion.identity);

            float angle = Mathf.Atan2( targetNorm.y, targetNorm.x) * Mathf.Rad2Deg;
            projectile = Instantiate(projectilePrefab, ppoPosition , Quaternion.Euler(0, 0, angle));
        }
        
        if (!Input.GetKey(KeyCode.Space) && isLock){
            isLock = false;
        }
        
        
    }
}
// new Vector2(0, 0)
// ppoPosition