using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shot : MonoBehaviour
{
    private GameObject pointOfIntersection, projectile;
    public GameObject addPoint, containerPrefab, projectilePrefab;

    private Vector2 ppoPosition,point;
    bool isLock = false;
    

    private void Start()
    {
        Vector3 ppoPositionV3 = GetComponent<Transform>().position;
        ppoPosition = new Vector2(ppoPositionV3.x, ppoPositionV3.y);
        
    }
    void FixedUpdate()
    {
        pointOfIntersection = GameObject.Find("pointOfIntersection(Clone)");
        if (!pointOfIntersection)
        {
            return;
        }
        Vector3 targetMiddle = pointOfIntersection.GetComponent<Transform>().position;
        Vector2 targetAbs = new Vector2(targetMiddle.x, targetMiddle.y);
        Vector2 target = targetAbs - ppoPosition;
        Vector2 targetNorm = target.normalized;
        // GameObject projectileTrajectoryContainer = Instantiate(containerPrefab, new Vector2(0, 0), Quaternion.identity);

        if ((Input.GetKey(KeyCode.Space)&&!isLock)){
            
            isLock = true;     
            
            
            GameObject container = Instantiate(containerPrefab, new Vector2(0, 0), Quaternion.identity);
            for (int i = 1; i < target.magnitude; i+=5)
            {
                GameObject elem=Instantiate(addPoint, new Vector2(0,0), Quaternion.identity);
                elem.transform.parent = container.transform;
                elem.transform.position  = targetNorm * i + ppoPosition;
            }
            float angle = Mathf.Atan2( targetNorm.y, targetNorm.x) * Mathf.Rad2Deg;
            projectile = Instantiate(projectilePrefab, ppoPosition , Quaternion.Euler(0, 0, angle));
            // projectile.transform.parent = container.transform;
            // projectile.GetComponent<Transform>().Translate(new Vector2(1, 0) * 80 * Time.fixedDeltaTime);
            
            // projectile.transform.position=ppoPosition;
            // projectile.transform.Rotate = new Vector3(0, 0, 75);

        }
        if (!Input.GetKey(KeyCode.Space) && isLock){
            isLock = false;
        }
        if(projectile){
            projectile.GetComponent<Transform>().Translate(new Vector2(1, 0) * 80 * Time.fixedDeltaTime);

        }
        
    }
}
// new Vector2(0, 0)
// ppoPosition