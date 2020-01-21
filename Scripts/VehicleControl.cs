using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleControl : MonoBehaviour
{
    public float speed = 0;
    private Vector3 halfSize;
    private Collider[] colliders;
    // private bool isMoving = false;
    private float startTime,timePast,distanceCovered;
    private Collider nextVehicleCollider;

    // Start is called before the first frame update
    void Start()
    {
        halfSize = GetComponentsInChildren<Collider>()[1].bounds.size;
        Debug.Log("Half size : "+halfSize);
        Debug.Log(new Vector3(halfSize.x * 4,halfSize.y,halfSize.z));
    }

    // Update is called once per frame
    void Update()
    {
        if(speed != 0){
            gameObject.GetComponentsInChildren<Transform>()[4].transform.Rotate(new Vector3(speed,0,0));
            gameObject.GetComponentsInChildren<Transform>()[5].transform.Rotate(new Vector3(speed,0,0));
            gameObject.GetComponentsInChildren<Transform>()[2].transform.Rotate(new Vector3(speed,0,0));
            gameObject.GetComponentsInChildren<Transform>()[3].transform.Rotate(new Vector3(speed,0,0)); 
            // Debug.Log("half size "+halfSize+" /2 "+halfSize/2);
            if(speed > 0){
                colliders = Physics.OverlapBox(new Vector3(transform.position.x + (halfSize.x/2),transform.position.y,transform.position.z),new Vector3(halfSize.x + 3f ,halfSize.y,halfSize.z));
            }
            else if (speed < 0 ){
                colliders = Physics.OverlapBox(new Vector3(transform.position.x - (halfSize.x/2),transform.position.y,transform.position.z),new Vector3(halfSize.x + 3f ,halfSize.y,halfSize.z));
            }
            foreach(Collider col in colliders)
            {
                if(col.tag == "Vehicle")
                {
                    // Debug.Log(new Vector3(transform.position.x - (halfSize.x/2),transform.position.y,transform.position.z));
                    // Debug.Log(new Vector3(halfSize.x * 2 * Mathf.Abs(speed),halfSize.y,halfSize.z));
                    // gameObject.GetComponent<Rigidbody>().velocity = col.transform.parent.GetComponent<Rigidbody>().velocity;
                    StartCoroutine("MoveToPosition",col);
                    // gameObject.GetComponent<Rigidbody>().velocity = Vector3.Lerp(gameObject.GetComponent<Rigidbody>().velocity,col.transform.parent.GetComponent<Rigidbody>().velocity,Time.deltaTime);
                    // Debug.Log("Reached HEre "+col.transform.parent.GetComponent<Rigidbody>().velocity);
                    // nextVehicleCollider = col;
                    // isMoving = true;
                    // startTime = Time.time;
                }
            }   
        } 
        if(gameObject.GetComponent<Rigidbody>().velocity.x == 0){
            speed = 0;
        }
    }
    public IEnumerator MoveToPosition(Collider pCol)
    {
      var t = 0f;
       while(t < 1 && gameObject != null && pCol != null)
       {
             t += Time.deltaTime / (240/speed) ;
             gameObject.GetComponent<Rigidbody>().velocity = Vector3.Lerp(gameObject.GetComponent<Rigidbody>().velocity,pCol.transform.parent.GetComponent<Rigidbody>().velocity,Time.deltaTime);
             yield return null;
      }
    }
    void SetSpeed(float pSpeed){
        speed = pSpeed;
        gameObject.GetComponent<Rigidbody>().velocity = new Vector3(speed,0,0);
    }
}
