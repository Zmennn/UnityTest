using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMain : MonoBehaviour
{
    private Vector3 point;
    
    private float speed = 100f;
    Vector3 normVector;
    Vector3 startPosition;
    private float angle;
    private Transform planeTr, planeTrSin;
    public GameObject planePrefab,marker;
    private float sinA = 30f,sinB = 0.05f;
   
    // private float arg;
    private float time;

    void Start()
    {
        ChangeTrajectory();
    }
    private void CreateTrajectory()
    {
            var b = UnityEngine.Random.Range(50, 170);
            var _maxY = 190 - b;
            var _minY = 40 - b;
            var vectorY = UnityEngine.Random.Range(_minY, _maxY);
            normVector = new Vector3(400, vectorY, 0).normalized;
            var k = normVector.y / normVector.x;
            startPosition = new Vector3(0, b, 0);
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
        angle = Mathf.Atan2(normVector.y, normVector.x) * Mathf.Rad2Deg;

        // _transform3.rotation = Quaternion.Euler(0, 0, angle);
        planeTr = Instantiate(planePrefab, startPosition, Quaternion.Euler(0, 0, angle)).GetComponent<Transform>() as Transform;     
    }
    private void CreatePlaneSin(){
        startPosition = new Vector3(0, 120, 0);
        planeTrSin = Instantiate(planePrefab, startPosition, Quaternion.Euler(0, 0, 0)).GetComponent<Transform>() as Transform;
        
    }


    private void FixedUpdate()
    {
        if (planeTr)
        {
            planeTr.Translate(new Vector3(1, 0, 0) * speed * Time.fixedDeltaTime);
            if (planeTr.position.x > 400)
            {
                Destroy(planeTr.gameObject);
                ClearTrajectory();
                ChangeTrajectory();
            }
        }
        else if (planeTrSin) {
            // time += Time.fixedDeltaTime;
            float vectorY = sinA * Mathf.Cos(Time.fixedDeltaTime * sinB) * sinB;
            // // Debug.Log(time+"---"+vectorY);
            var normVector = new Vector3(Time.fixedDeltaTime, vectorY, 0).normalized;    
            angle =Mathf.Atan2(normVector.x, normVector.y) * Mathf.Rad2Deg ;
            //     var angle2 = Mathf.Abs(angle);
                Debug.Log(angle+"=====");
                planeTrSin.Rotate(new Vector3(0, 0, angle)* Time.fixedDeltaTime,Space.World);
                planeTrSin.Translate(new Vector3(1, 0, 0) * 50 * Time.fixedDeltaTime);
            // planeTrSin.position = startPosition + transform.up * Mathf.Sin(Time.time * 5f) * 50f;
        }
   }

    private void ChangeTrajectory()
    {
        Invoke("CreateTrajectory", .9f);
        Invoke("CreatePlane", 1.71f);

        // Invoke("CreateTrajectorySin", .9f);
        // Invoke("CreatePlaneSin", 1.2f);


    }
    private void CollisionCalculator(){
        
    }


}

// 1 / (Math.Atan(x2) * 180 / Math.PI))

// radians = Math.Atan(result);


// angle = radians * (180 / Math.PI);

// speedV = Random.Range(0.5f, 3); // при старте зададим диапазон скорости от 0.5 до 3
