using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    private GameObject pointOfIntersection, projectile, containerTrajectory, traceContainer;
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
            var projectiles = GameObject.FindGameObjectsWithTag("projectile");

            for (int i = 0; i < projectiles.Length; i++)
            {
                if(projectiles[i].transform.position.x<0||projectiles[i].transform.position.x>440||
                projectiles[i].transform.position.y < 0 || projectiles[i].transform.position.y > 220)
                {
                    Destroy(projectiles[i].transform.parent.gameObject);
                    continue;
                }
                projectiles[i].transform.Translate(new Vector2(1, 0) * moveMain.projectileSpeed * Time.fixedDeltaTime);
                Vector2 randomVector = new Vector2(Random.Range(-0.8f, 0.8f), Random.Range(-0.3f, 0.3f));
                Vector2 posVector = new Vector2(projectiles[i].transform.position.x, projectiles[i].transform.position.y);
                var trace=Instantiate(tracePrefab, posVector + randomVector, Quaternion.identity);
                trace.transform.parent = traceContainer.transform;
            }

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
        containerTrajectory = Instantiate(containerPrefab, new Vector2(0, 0), Quaternion.identity);
        containerTrajectory.name = "containerTrajectory";
        for (int i = 1; i < 300; i += 5)
        {
            GameObject elem = Instantiate(addPoint, new Vector2(0, 0), Quaternion.identity);
            elem.transform.parent = containerTrajectory.transform;
            elem.transform.position = targetNorm * i + ppoPosition;
        }

        if ((Input.GetKey(KeyCode.Space)&&!isLock)){           
            isLock = true;
            traceContainer = Instantiate(containerPrefab, new Vector2(0, 0), Quaternion.identity);
            traceContainer.name = "ProjectileContainer";
            float angle = Mathf.Atan2( targetNorm.y, targetNorm.x) * Mathf.Rad2Deg;
            projectile = Instantiate(projectilePrefab, ppoPosition , Quaternion.Euler(0, 0, angle));           
            projectile.transform.parent=traceContainer.transform;
        }

        if (!Input.GetKey(KeyCode.Space) && isLock){
            isLock = false;
        }       
    }
}
