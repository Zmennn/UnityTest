using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMain : MonoBehaviour
{
    private Vector3 point;   
    public float speed = 80f,projectileSpeed=160f;
    Vector2 planeNormVector;
    private Vector2 startPosition,ppoPosition;
    private float angle,k;
    private Transform  planeTransformSin, planeTransform, addPointTransform;
    public GameObject planePrefab, marker, ppo, intersectionPointPrefab, addPointPrefab,containerPrefab,tracePrefab;
    private float magnitude,frequency;
    private Vector2 pointOfIntersection;
    private GameObject intersectionPoint,planeObj,addPoint,planeContainer;
    private float time;
    private string trajectoryType;


    void Start()
    {       
       ppoPosition=ppo.GetComponent<Transform>().position;
        ChangeTrajectory();
    }
    private void CreateLin()
    {
        trajectoryType = "lin";

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
     
        

        angle = Mathf.Atan2(planeNormVector.y, planeNormVector.x) * Mathf.Rad2Deg;
        planeTransform.Rotate(new Vector3(0, 0, angle));
        planeTransform.position = startPosition;

        // planeObj = Instantiate(planePrefab, startPosition, Quaternion.Euler(0, 0, angle));
        // planeObj.name = "plane";
        // planeTransform = planeObj.transform;
        // planeTransform.parent = planeContainer.transform;
    }

    private void CreateSin(){
        trajectoryType = "sin";
        
        int b = UnityEngine.Random.Range(70, 170);
        startPosition = new Vector2(0, b);
        int d1 = 170 - b;
        int d2 = b - 50;
        int delta=Mathf.Min(d1, d2);
        magnitude = UnityEngine.Random.Range(20, 20 + delta);
        frequency=UnityEngine.Random.Range(2, 10)*0.01f;

        for (int i = 0; i <= 400; i += 2)
        {
            point.x = i;
            point.y =  Mathf.Sin(i * frequency)* magnitude + b;
            GameObject elem = Instantiate(marker, point, Quaternion.identity);
            elem.name = "planeTrajectoryMark";
            elem.transform.parent = planeContainer.transform;
        }
     
        planeTransform.position = startPosition = new Vector2(0, b); ;
        angle = 0;
    }

    float x;
    private void FixedUpdate()
    {
        if (planeObj)
        {
            for (short i = 0; i < 5; i++)
            {
                Vector2 randomVector = new Vector2(Random.Range(-.8f, .8f), Random.Range(-2.3f, 2.3f));
                Vector2 posVector = new Vector2(planeTransform.position.x, planeTransform.position.y);
                var position = new Vector2(planeTransform.GetChild(0).transform.position.x, planeTransform.GetChild(0).transform.position.y);
                var trace = Instantiate(tracePrefab, position + randomVector, Quaternion.identity);
                trace.transform.parent = planeContainer.transform;
            }
            pointCollision(planeTransform.position);
            if (planeTransform.position.x > 400)
            {
                DestroyPlane();
                return;
            }

        }
        if (planeObj && trajectoryType == "lin")
        {
            planeTransform.Translate(new Vector2(1, 0) * speed * Time.fixedDeltaTime);         
        }
        else if (planeObj && trajectoryType == "sin")
        {
            float x = planeTransform.position.x;
            float y_prime = magnitude * frequency * Mathf.Cos(x * frequency); // похідна функції у точці x
            Vector2 tangent = new Vector2(x, y_prime).normalized;
            float angleCurrent = Mathf.Atan(Mathf.Cos(x * frequency) * frequency * magnitude) * Mathf.Rad2Deg;
            float angelDelta = angleCurrent - angle;
            angle = angleCurrent;
            planeTransform.Rotate(new Vector3(0, 0, angelDelta));
            planeTransform.Translate(new Vector2(1, 0) * speed * Time.fixedDeltaTime);
        }
   }

   public void DestroyPlane(){
        Destroy(planeContainer);
        ChangeTrajectory();
   }

   private void ChangeTrajectory(){
        Invoke("ChangeTrajectoryWait", 0.5f);
    }

    private void ChangeTrajectoryWait()
    {
        planeContainer = Instantiate(containerPrefab, new Vector2(0, 0), Quaternion.identity);
        planeContainer.name = "planeContainer";

        intersectionPoint = Instantiate(intersectionPointPrefab, new Vector2(0, 0), Quaternion.identity);
        intersectionPoint.transform.parent = planeContainer.transform;

        planeObj = Instantiate(planePrefab, startPosition, Quaternion.identity);
        planeObj.name = "plane";
        planeTransform = planeObj.transform;
        planeTransform.parent = planeContainer.transform;

        // CreateLin();
        CreateSin();

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

