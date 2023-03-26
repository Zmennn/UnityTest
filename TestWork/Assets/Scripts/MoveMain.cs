using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMain : MonoBehaviour
{
    private Vector3 point;   
    public float speed = 20f,projectileSpeed=150f;
    Vector2 planeNormVector;
    private Vector2 startPosition,ppoPosition;
    private float angle,k;
    private Transform  planeTransformSin, planeTransform, addPointTransform;
    public GameObject planePrefab, marker, ppo, intersectionPointPrefab, addPointPrefab,containerPrefab,tracePrefab;
    private float sinA = 30f,sinB = 0.05f;
    private Vector2 pointOfIntersection;
    private GameObject intersectionPoint,planeObj,addPoint,planeContainer;
    private float time;


    void Start()
    {       
       ppoPosition=ppo.GetComponent<Transform>().position;
        ChangeTrajectory();
    }
    private void CreateLin()
    {
        planeContainer = Instantiate(containerPrefab, new Vector2(0, 0), Quaternion.identity);
        planeContainer.name = "planeContainer";

        var b = UnityEngine.Random.Range(50, 170);
        var _maxY = 190 - b;
        var _minY = 40 - b;
        var vectorY = UnityEngine.Random.Range(_minY, _maxY);
        planeNormVector = new Vector2(400, vectorY).normalized;
        k = planeNormVector.y / planeNormVector.x;
        startPosition = new Vector2(0, b);
        for (int i = 0; i <= 400; i += 5)
            {               
            point.x = i;
            point.y = i*k+b;
            GameObject elem=Instantiate(marker, point, Quaternion.identity);
            elem.name = "planeTrajectoryMark";
            elem.transform.parent = planeContainer.transform;
            }
     
        intersectionPoint = Instantiate(intersectionPointPrefab, new Vector2(0, 0), Quaternion.identity);
        intersectionPoint.transform.parent = planeContainer.transform;

        angle = Mathf.Atan2(planeNormVector.y, planeNormVector.x) * Mathf.Rad2Deg;
        planeObj = Instantiate(planePrefab, startPosition, Quaternion.Euler(0, 0, angle));
        planeObj.name = "plane";
        planeTransform = planeObj.transform;
        planeTransform.parent = planeContainer.transform;
    }
    private void CreateTrajectorySin(){
        var b = 120;
        for (int i = 0; i <= 400; i += 2)
        {
            point.x = i;
            point.y =  Mathf.Sin(i * sinB)* sinA + b;
            Instantiate(marker, point, Quaternion.identity);
        }
        
    }
    private void CreatePlaneSin(){
        startPosition = new Vector2(0, 120);
        planeTransformSin = Instantiate(planePrefab, startPosition, Quaternion.Euler(0, 0, 0)).GetComponent<Transform>() as Transform;
        
    }


    private void FixedUpdate()
    {
        if (planeObj)
        {
            planeTransform.Translate(new Vector2(1, 0) * speed * Time.fixedDeltaTime);
            for (short i = 0; i < 5;i++)
            {
                Vector2 randomVector = new Vector2(Random.Range(-.8f, .8f), Random.Range(-2.3f, 2.3f));
                Vector2 posVector = new Vector2(planeTransform.position.x, planeTransform.position.y);
                var trace = Instantiate(tracePrefab, posVector + randomVector + new Vector2(-9, -9 * k), Quaternion.identity);
                trace.transform.parent = planeContainer.transform;
            }

            pointCollision(planeTransform.position);
            if (planeTransform.position.x > 400)
            {
                DestroyPlane();
                return;
            }     
        }
        else if (planeTransformSin) {
            // time += Time.fixedDeltaTime;
            float vectorY = sinA * Mathf.Cos(Time.fixedDeltaTime * sinB) * sinB;
            // // Debug.Log(time+"---"+vectorY);
            var planeNormVector = new Vector2(Time.fixedDeltaTime, vectorY).normalized;    
            angle =Mathf.Atan2(planeNormVector.x, planeNormVector.y) * Mathf.Rad2Deg ;
            //     var angle2 = Mathf.Abs(angle);
                Debug.Log(angle+"=====");
                planeTransformSin.Rotate(new Vector3(0, 0, angle)* Time.fixedDeltaTime,Space.World);
                planeTransformSin.Translate(new Vector3(1, 0, 0) * 50 * Time.fixedDeltaTime);
            // planeTransformSin.position = startPosition + transform.up * Mathf.Sin(Time.time * 5f) * 50f;
        }
   }

   public void DestroyPlane(){
        Destroy(planeContainer);
        ChangeTrajectory();
   }

    private void ChangeTrajectory()
    {
        Invoke("CreateLin", .9f);
        // Invoke("CreateTrajectorySin", .9f);
    }
    private void pointCollision(Vector2 targetPosition)
    {
        bool allowContinue = true;
        int i = 0;
        Vector2 currentPlanePosition = planeTransform.position;
        Vector2 processTargetPosition = targetPosition;
        while (allowContinue)
        {
            //розрахунок точки зустрічі
            Vector2 targetDirection = processTargetPosition - ppoPosition;
            float distance = targetDirection.magnitude;
            float flayTime = distance / projectileSpeed;
            Vector2 targetDrive = planeNormVector * flayTime * speed;
            Vector2 newTargetPosition = currentPlanePosition + targetDrive;

            //перевірка на влучання
            Vector2 newTargetDirection = newTargetPosition - ppoPosition;           
            float newDistance = newTargetDirection.magnitude;
            float newFlayTime = newDistance / projectileSpeed;
            Vector2 newTargetDrive = planeNormVector * newFlayTime * speed;
            Vector2 controlTargetPosition = currentPlanePosition + newTargetDrive;
            
           
            Vector2 difference = controlTargetPosition - newTargetPosition;
            i++;
            if (difference.magnitude > 2)
            {
                processTargetPosition = newTargetPosition;
            }else{
                allowContinue = false;
                Color colorGreen = new Color(1f, 230f, 0.00f, 1.00f);

                if(newTargetPosition.x<-10f||newTargetPosition.x >420)
                {
                    intersectionPoint.SetActive(false);
                } else
                {
                    intersectionPoint.SetActive(true);
                    intersectionPoint.transform.position = newTargetPosition;
                    intersectionPoint.GetComponent<SpriteRenderer>().color = colorGreen;
                }                
            }
            if(i>10){
                allowContinue = false;
                Color colorRed = new Color(190f, 0f, 0f, 1.0f);

                if (newTargetPosition.x < -10f || newTargetPosition.x > 420)
                {
                    intersectionPoint.SetActive(false);
                }
                else
                {
                    intersectionPoint.SetActive(true);
                    intersectionPoint.transform.position = newTargetPosition;
                    intersectionPoint.GetComponent<SpriteRenderer>().color = colorRed;
                }
            }                       
        }        
    }
    



  
}

