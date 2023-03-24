using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMain : MonoBehaviour
{
    private Vector3 point;
    
    private float speed = 100f,projectileSpeed=800f;
    Vector2 planeNormVector;
    private Vector2 startPosition,ppoPosition;
    private float angle,k;
    private Transform  planeTransformSin;
    public GameObject planePrefab,marker,ppo,intersectionPointPrefab;
    private float sinA = 30f,sinB = 0.05f;
    private Vector2 pointOfIntersection;
    private GameObject intersectionPoint,planeObj;
    private float time;

    void Start()
    {
       intersectionPoint= Instantiate(intersectionPointPrefab, new Vector2(0,0), Quaternion.identity);
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
        planeObj = Instantiate(planePrefab, startPosition, Quaternion.Euler(0, 0, angle));     
    }
    private void CreatePlaneSin(){
        startPosition = new Vector2(0, 120);
        planeTransformSin = Instantiate(planePrefab, startPosition, Quaternion.Euler(0, 0, 0)).GetComponent<Transform>() as Transform;
        
    }


    private void FixedUpdate()
    {
        if (planeObj)
        {
            planeObj.GetComponent<Transform>().Translate(new Vector2(1, 0) * speed * Time.fixedDeltaTime);


            collision(planeObj.GetComponent<Transform>().position);
            if (planeObj.GetComponent<Transform>().position.x > 400)
            {
                Destroy(planeObj);
                ClearTrajectory();
                ChangeTrajectory();
                return;
            }
            Vector2 pos1 = planeObj.GetComponent<Transform>().position;
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
        Invoke("CreatePlane", 1.71f);

        // Invoke("CreateTrajectorySin", .9f);
        // Invoke("CreatePlaneSin", 1.2f);


    }
    private void collision(Vector2 targetPosition)
    {
        Vector2 targetDirection = targetPosition - ppoPosition;
        float distance=targetDirection.magnitude;
        float flayTime=distance/projectileSpeed;
        Vector2 targetDrive = targetDirection * flayTime;
        Vector2 newTargetPosition =(planeObj.GetComponent<Transform>().position)+targetDrive ;

    }
    



  
}

// 1 / (Math.Atan(x2) * 180 / Math.PI))

// radians = Math.Atan(result);


// angle = radians * (180 / Math.PI);

// speedV = Random.Range(0.5f, 3); // при старте зададим диапазон скорости от 0.5 до 3
