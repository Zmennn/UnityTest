using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPlane : MonoBehaviour
{

    public GameObject explosionAnimation;
    private MoveMain moveMain;
    

    private void Start()
    {
        GameObject cam = GameObject.Find("Camera");
        moveMain = cam.GetComponent<MoveMain>();       
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        
        if(other.gameObject.name=="projectile(Clone)"){
            Instantiate(explosionAnimation,transform.position,Quaternion.identity);
            moveMain.DestroyPlane();
            Destroy(other.gameObject.transform.parent.gameObject);
        }
        
    }
}

