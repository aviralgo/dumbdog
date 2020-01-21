using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rigidbody;
    private bool tagOnVehicle= false;
    public GameObject gObj = null;
    public Vector3 touch;
    private Vector3 velo;
    private float distanceBtw;
    private List<Collider> vehiclesInRange;
    private Collider bestCollider;
    private Vector3 currentPosition, oldPosition;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // if(Input.GetMouseButtonDown(0)){
        //     Ray mouseRay = GenerateMouseRay();
        //     RaycastHit hit;
        //     if(Physics.Raycast(mouseRay.origin, mouseRay.direction, out hit))
        //     {
        //         gObj = hit.transform.gameObject;
        //     }
        // }else 
        if (Input.GetMouseButton(0)){
            // Ray mouseRay = GenerateMouseRay();
            // RaycastHit hit;
            // if(Physics.Raycast(mouseRay.origin, mouseRay.direction, out hit))
            // {
            //     gObj = hit.transform.gameObject;
            // }
            // if(gObj && gObj.name == "Player"){
                
                Vector3 newPos;
                touch = Input.mousePosition;   
                // Debug.Log("Touch 1 "+touch);
                Ray castPoint = Camera.main.ScreenPointToRay(touch);
                RaycastHit hit_touch;
                if (Physics.Raycast(castPoint, out hit_touch, Mathf.Infinity))
                {
                    newPos = new Vector3(hit_touch.point.x,0.5f,hit_touch.point.z);
                    // distanceBtw = gObj.transform.position-new Vector3(newPos.x, gObj.transform.position.y,newPos.z);
                    // gObj.GetComponent<Rigidbody>().MovePosition(Vector3.MoveTowards(gObj.transform.position,new Vector3(newPos.x, gObj.transform.position.y,newPos.z),0.15f));
                    gameObject.GetComponent<Rigidbody>().MovePosition(Vector3.MoveTowards(gameObject.transform.position,newPos,5f * Time.deltaTime));
                }
                // velo = (gameObject.transform.position - oldPosition)*15;
            // }
        }else if (Input.GetMouseButtonUp(0)){
            // rigidbody.velocity = velo; 
            tagOnVehicle = false;
            touch = Input.mousePosition;
            // Debug.Log("Touch 2 "+touch);
            Ray castPoint = Camera.main.ScreenPointToRay(touch);
            RaycastHit hit_touch;
            if (Physics.Raycast(castPoint, out hit_touch, Mathf.Infinity))
            {
                touch = new Vector3(hit_touch.point.x,0.5f,hit_touch.point.z);
            }
            // Ray mouseRay = new Ray(gameObject.transform.position, gameObject.transform.position - touch);
            Ray mouseRay = new Ray(touch, touch-gameObject.transform.position);
            Debug.Log(mouseRay.direction);
            rigidbody.velocity = new Vector3(mouseRay.direction.x * 5, 0, mouseRay.direction.z * 5);
            distanceBtw = Vector3.Distance(gameObject.transform.position,touch);
            // gameObject.GetComponent<Rigidbody>().MovePosition(Vector3.MoveTowards(gameObject.transform.position,touch,0.15f));
            // velo = gameObject.GetComponent<Rigidbody>().velocity;
            // Debug.Log("Distance "+distanceBtw);
            if (distanceBtw < 5){
                // StartCoroutine(MoveToPosition(touch));
                // gameObject.GetComponent<Rigidbody>().AddForce(touch,ForceMode.VelocityChange);
                // velo = gameObject.GetComponent<Rigidbody>().velocity;
            // }
            // else{
                Collider[] colliders = Physics.OverlapSphere(touch, 2f);
                float bestDistance = 5.1f;
                foreach(Collider col in colliders){
                    if(col.tag == "Vehicle"){
                        float distance = Vector3.Distance(gameObject.transform.position, col.transform.position);
                        if (distance < bestDistance)
                        {
                            bestDistance = distance;
                            bestCollider = col;
                            tagOnVehicle = true;
                        }
                    }
                }
            }
            // if(!tagOnVehicle){
                // gameObject.GetComponent<Rigidbody>().MovePosition(Vector3.MoveTowards(gameObject.transform.position,touch,0.15f));
                // gameObject.GetComponent<Rigidbody>().AddForce(touch,ForceMode.VelocityChange);
                // velo = gameObject.GetComponent<Rigidbody>().velocity;
            // }
            // vehiclesInRange.Sort(delegate(Collider a, Collider b) {
            //     return (gameObject.transform.position - a.GetComponentInParent<Transform>().position).CompareTo(gameObject.transform.position - b.GetComponentInParent<Transform>().position);
            // }
            if(tagOnVehicle){
                StartCoroutine(MoveToVehicle(bestCollider));
            }
            // else{
            //     tagOnVehicle = false;
            //     // gameObject.GetComponent<Rigidbody>().velocity = velo;
            //     gObj = null;
            // }
        }
        // if(Vector3.Distance(gameObject.transform.position,touch) < 1){
        //     rigidbody.velocity = velo;
        // }
    }
    void LateUpdate(){
        Debug.Log("LU CALLED");
        oldPosition = gameObject.transform.position;
    }
    public IEnumerator MoveToPosition(Vector3 position){
        while(true){
            if(Vector3.Distance(gameObject.transform.position,position)<0.5f){
                rigidbody.velocity = velo;
                yield break;
            }
            else{
                rigidbody.MovePosition(Vector3.MoveTowards(gameObject.transform.position,position,0.15f));
                velo = rigidbody.velocity;
            }
        }
    }
    public IEnumerator MoveToVehicle(Collider pCol)
    {
        VehicleControl gObject = pCol.GetComponentInParent<VehicleControl>();
        Vector3 vehicleTransform = pCol.GetComponentInParent<Transform>().position;
        while(true){
            if(Input.GetMouseButton(0) || gObject == null){
                // rigidbody.velocity = velo;
                yield break;
            } else {
                Vector3 finalPosition = pCol.GetComponentsInParent<Transform>()[0].transform.position;
                if((gameObject.transform.position.z > pCol.GetComponentsInParent<Transform>()[0].position.z + (pCol.bounds.size.z/2))){
                    if(gObject.speed > 0){
                        finalPosition = new Vector3(finalPosition.x - 2f,finalPosition.y,finalPosition.z + (pCol.bounds.size.z/2) + 0.5f);
                    }
                Debug.Log("Reached here");
                } else 
                if((gameObject.transform.position.z < pCol.GetComponentsInParent<Transform>()[0].position.z - (pCol.bounds.size.z/2)) && gObject.speed > 0){
                    finalPosition = new Vector3(finalPosition.x - 2f,finalPosition.y,finalPosition.z - (pCol.bounds.size.z/2) - 0.5f);
                Debug.Log("Reached here");
                }else 
                if((gameObject.transform.position.z > pCol.GetComponentsInParent<Transform>()[0].position.z + (pCol.bounds.size.z/2)) && gObject.speed < 0){
                    finalPosition = new Vector3(finalPosition.x + 2f,finalPosition.y,finalPosition.z + (pCol.bounds.size.z/2) + 0.5f);
                Debug.Log("Reached here");
                }else 
                if((gameObject.transform.position.z < pCol.GetComponentsInParent<Transform>()[0].position.z - (pCol.bounds.size.z/2)) && gObject.speed < 0){
                    finalPosition = new Vector3(finalPosition.x + 2f,finalPosition.y,finalPosition.z - (pCol.bounds.size.z/2) - 0.5f);
                Debug.Log("Reached here");
                }
                gameObject.GetComponent<Rigidbody>().MovePosition(Vector3.MoveTowards(gameObject.transform.position,finalPosition,15f * Time.deltaTime));
                // velo = gObj.GetComponent<Rigidbody>().velocity;
                // velo = gObject.GetComponent<Rigidbody>().velocity;
                yield return null;
            }
        }
    }

    Ray GenerateMouseRay(){
        Vector3 mousePosFar = new Vector3(Input.mousePosition.x,Input.mousePosition.y,Camera.main.farClipPlane);
        Vector3 mousePosNear = new Vector3(Input.mousePosition.x,Input.mousePosition.y,Camera.main.nearClipPlane);
    
        Vector3 mousePosF = Camera.main.ScreenToWorldPoint(mousePosFar);
        Vector3 mousePosN = Camera.main.ScreenToWorldPoint(mousePosNear);

        Ray mouseRay = new Ray(mousePosN, mousePosF - mousePosN); 
        return mouseRay;
    }
}
