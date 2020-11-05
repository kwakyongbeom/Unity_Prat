using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // Start is called before the first frame update
    Transform playerTransform;
    Vector3 Offset;
    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;    //주어진 태그로 오브젝트 검색(FindGameObjectWithTag)
        Offset = transform.position - playerTransform.position; //현재 카메라 위치와 Player 사이의 Vector 값 
    }

    // Update is called once per frame
    void LateUpdate()//Update 이후 실행됨 Camera or UI 같은거는 여기서 사용 
    {
        transform.position = playerTransform.position + Offset; // 현재 Player위치와 위에서 계산한 고정 Offset값을 더해 일정 거리를 유지하여 카메라가 이동 
    }
}
