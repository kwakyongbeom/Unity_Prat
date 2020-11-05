 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // Start is called before the first frame update
    public float rotateSpeed; //Unity 환경에서 동적으로 수정하기 위해서 
    
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);//(0,1,0) 축을 기준으로 world space를 roateteSpeed 만큼의 속도로 회전 
    }
  
 



}
