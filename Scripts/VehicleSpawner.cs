using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    public int randomLoop, randomPrefab, randomDirection;
    public GameObject bus_1,bus_2,car_1,car_2,car_3,car_4,police_car,taxi;
    public List<GameObject> prefabList, carList;
    private Vector3 position;
    private bool validPosition = false;
    // Start is called before the first frame update
    void Start()
    {
        prefabList.Add(bus_1);
        prefabList.Add(bus_2);
        prefabList.Add(car_1);
        prefabList.Add(car_2);
        prefabList.Add(car_3);
        prefabList.Add(car_4);
        prefabList.Add(police_car);
        prefabList.Add(taxi);
        randomLoop = Random.Range(5,10);
        for(int i =0 ; i< randomLoop;i++){
            randomDirection = Random.Range(0,2);
            if (randomDirection == 0){
                // SpawnVehicle(-50f,-30f);
            }else{
                // SpawnVehicle(30f,50f);
            }
        }
    }

    void SpawnVehicle(float leftMost, float rightMost){
            randomPrefab=Random.Range(0,8);
            Vector3 halfExtend = new Vector3();
            validPosition = false;
            float lane1, lane2;
            int spawnAttempts = 0;
            if(randomPrefab == 0 || randomPrefab == 1){
                halfExtend = new Vector3(3.1f,0.625f,0.7f);
            } 
            else{
                halfExtend = new Vector3(2.3f,0.4f,0.5f);
            }
            if(randomDirection==0){ // going left -1.4 -3.5
                lane1 = - 1.4f;
                lane2 = - 3.5f;
                // while(!validPosition && spawnAttempts < 10)
                // {
                //     spawnAttempts++;
                //     position = new Vector3(Random.Range(-45.0f,45.0f),0.5f, Random.Range(0, 2) == 0 ? -lane1 : -lane2);
                //     validPosition = true;
                    
                //     //bus width z 1.4f, length x 4.2f,  y 1.25f
                //     //car 1f,2.6,0.8f

                //     Collider[] colliders = Physics.OverlapBox(position,halfExtend);
                //     foreach(Collider col in colliders)
                //     {
                //         if(col.tag == "Vehicle")
                //         {
                //             validPosition = false;
                //         }
                //     }
                // }
            }
            else{ // going right
                lane1 = 1.4f;
                lane2 = 3.5f;
                
                // if(validPosition){
                //     GameObject.Instantiate(prefabList[randomPrefab],position,Quaternion.Euler(new Vector3(0,-90,0)));
                // }
            }
            validPosition = false;
            spawnAttempts = 0;
            while(!validPosition && spawnAttempts < 10)
            {
                spawnAttempts++;
                position = new Vector3(Random.Range(leftMost,rightMost),0.5f, Random.Range(0, 2) == 0 ? lane1 : lane2);
                validPosition = true;
                Collider[] colliders = Physics.OverlapBox(position,halfExtend);
                foreach(Collider col in colliders)
                {
                    if(col.tag == "Vehicle")
                    {
                        validPosition = false;
                    }
                }
            }
            if(validPosition){
                GameObject vehicle = GameObject.Instantiate(prefabList[randomPrefab],position,Quaternion.Euler(new Vector3(0,90,0)));
                if((randomPrefab == 0 || randomPrefab == 1) && randomDirection != 0){
                    vehicle.SendMessage("SetSpeed",Random.Range(-3.5f,-4.5f));
                    vehicle.transform.Rotate(new Vector3(0,180,0));
                }
                else if ((randomPrefab == 0 || randomPrefab == 1) && randomDirection == 0){
                    vehicle.SendMessage("SetSpeed",Random.Range(3.5f,4.5f));
                }
                else if (randomPrefab != 0 && randomPrefab != 1 && randomDirection != 0){
                    vehicle.SendMessage("SetSpeed",Random.Range(-5.0f,-7.0f));
                    vehicle.transform.Rotate(new Vector3(0,180,0));
                }
                else if (randomPrefab != 0 && randomPrefab != 1 && randomDirection == 0){
                    vehicle.SendMessage("SetSpeed",Random.Range(5.0f,7.0f));
                }
                carList.Add(vehicle);
            }
    }
    // Update is called once per frame
    void Update()
    {
        if(Time.frameCount % 60 == 0){
            randomDirection = Random.Range(0,2);
            if (randomDirection == 0){
                // SpawnVehicle(-50f,-30f);
            }else{
                // SpawnVehicle(30f,50f);
            }
        }
        if(carList.Count >= 20){
            Destroy(carList[carList.Count-20]);
        }
    }
}
