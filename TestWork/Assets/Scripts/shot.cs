using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    private GameObject pointOfIntersection, projectile, containerTrajectory, traceContainer;
    public GameObject addPoint, containerPrefab, projectilePrefab,tracePrefab,cam,cannon,shotAnim;
    private MoveMain moveMain;
    private Vector2 ppoPosition,point,oldVector,targetNorm;
    private bool isLock = false;
    private bool permissionToReset = true;
    private int countShots = 10;
    private int rateOfFire = 600;


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
                Vector2 randomVector = new Vector2(UnityEngine.Random.Range(-0.8f, 0.8f), UnityEngine.Random.Range(-0.3f, 0.3f));
                Vector2 posVector = new Vector2(projectiles[i].transform.position.x, projectiles[i].transform.position.y);
                if((projectiles[i].transform.position-transform.position).magnitude>17)
                {
                    var trace = Instantiate(tracePrefab, posVector + randomVector, Quaternion.identity);
                    trace.transform.parent = traceContainer.transform;
                }
                
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
        targetNorm = target.normalized;

        float targetAngle = Mathf.Atan2(targetNorm.y, targetNorm.x) * Mathf.Rad2Deg;
        float currentAngle = cannon.transform.rotation.eulerAngles.z;
        float deltaAngle = targetAngle - currentAngle;
        Vector3 ppoPositionGlobal3 = transform.TransformPoint(new Vector2(0,0));
        Vector2 ppoPositionGlobal = new Vector2(ppoPositionGlobal3.x, ppoPositionGlobal3.y);
        cannon.transform.RotateAround(ppoPositionGlobal, new Vector3(0,0,1), deltaAngle);

        containerTrajectory = Instantiate(containerPrefab, new Vector2(0, 0), Quaternion.identity);
        containerTrajectory.name = "containerTrajectory";
        for (int i = 15; i < 300; i += 5)
        {
            GameObject elem = Instantiate(addPoint, new Vector2(0, 0), Quaternion.identity);
            elem.transform.parent = containerTrajectory.transform;
            elem.transform.position = targetNorm * i + ppoPosition;
        }

        if ((Input.GetKey(KeyCode.Space)&&!isLock)){           
            isLock = true;
            

            StartCoroutine(shot());      
        }

        if (!Input.GetKey(KeyCode.Space) && isLock && permissionToReset){
            isLock = false;
        }       
    }

    private IEnumerator shot()
    {
        permissionToReset = false;
        for (int i = 0; i < countShots; i++)
        {
            traceContainer = Instantiate(containerPrefab, new Vector2(0, 0), Quaternion.identity);
            traceContainer.name = "ProjectileContainer";
            float angle = Mathf.Atan2(targetNorm.y, targetNorm.x) * Mathf.Rad2Deg;
            projectile = Instantiate(projectilePrefab, ppoPosition, Quaternion.Euler(0, 0, angle));
            projectile.transform.parent = traceContainer.transform;
            var shotAnimation = Instantiate(shotAnim, cannon.transform);

            Debug.Log(60f / rateOfFire);
            yield return new WaitForSeconds(60f/rateOfFire);
        }
        permissionToReset = true;
    }
}
