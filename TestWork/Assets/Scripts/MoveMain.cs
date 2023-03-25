using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMain : MonoBehaviour
{
    private Vector3 point;
    
    private float speed = 100f,projectileSpeed=200f;
    Vector2 planeNormVector;
    private Vector2 startPosition,ppoPosition;
    private float angle,k;
    private Transform  planeTransformSin, planeTransform,intersectionPointTransform, addPointTransform;
    public GameObject planePrefab, marker, ppo, intersectionPointPrefab, addPointPrefab;
    private float sinA = 30f,sinB = 0.05f;
    private Vector2 pointOfIntersection;
    private GameObject intersectionPoint,planeObj,addPoint;
    private float time;
    // private bool allowContinue = true;

    void Start()
    {
       
       ppoPosition=ppo.GetComponent<Transform>().position;
        ChangeTrajectory();
    }
    private void CreateTrajectory()
    {
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
                Instantiate(marker, point, Quaternion.identity);
            }
            CreatePlane();
              
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
    private void ClearTrajectory(){
      var  marks = GameObject.FindGameObjectsWithTag("Mark");
     
        for (int i = 0; i < marks.Length; i++)
        {
            Destroy(marks[i]);
        }
    }
    private void CreatePlane(){
        angle = Mathf.Atan2(planeNormVector.y, planeNormVector.x) * Mathf.Rad2Deg;

        // _transform3.rotation = Quaternion.Euler(0, 0, angle);
        intersectionPoint = Instantiate(intersectionPointPrefab, new Vector2(0, 0), Quaternion.identity);
        intersectionPointTransform = intersectionPoint.GetComponent<Transform>() as Transform;

        addPoint = Instantiate(addPointPrefab, new Vector2(0, 0), Quaternion.identity);
        addPointTransform = addPoint.GetComponent<Transform>() as Transform;

        planeObj = Instantiate(planePrefab, startPosition, Quaternion.Euler(0, 0, angle));
        planeTransform = planeObj.GetComponent<Transform>();
        
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


            collision(planeTransform.position);
            if (planeTransform.position.x > 400)
            {
                Destroy(planeObj);
                ClearTrajectory();
                Destroy(intersectionPoint);
                Destroy(addPoint);
                ChangeTrajectory();
                return;
            }
            Vector2 pos1 = planeTransform.position;
            Vector2 pos2 = ppo.transform.position;

        
       
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

    private void ChangeTrajectory()
    {
        Invoke("CreateTrajectory", .9f);
        

        // Invoke("CreateTrajectorySin", .9f);
       


    }
    private void collision(Vector2 targetPosition)
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
            Debug.Log(difference.magnitude + "=====" + i);
            if (difference.magnitude > 3)
            {
                processTargetPosition = newTargetPosition;
            }else{
                allowContinue = false;
            }
            if(i>10){
                allowContinue = false;
            }
            intersectionPointTransform.position = newTargetPosition;
            addPointTransform.position = controlTargetPosition;
                      
        }
        
    }
    



  
}

// 1 / (Math.Atan(x2) * 180 / Math.PI))

// radians = Math.Atan(result);


// angle = radians * (180 / Math.PI);

// speedV = Random.Range(0.5f, 3); // при старте зададим диапазон скорости от 0.5 до 3
