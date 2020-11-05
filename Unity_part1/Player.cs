using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//Scene 저장 name space,File->Build Setting에서 Load 할 Scene 추가  

public class Player : MonoBehaviour
{
    Rigidbody rigid;
    public float jumpPower=10;
    public int itemCount = 0;//score 변수  
    bool isJump;
    AudioSource audio; //Sound를 담당하는 component ->AudioSource ,Audio Clip은 재생될 Sound file,free sound는 asset store에서 purchase
    public GameManager1 manager;
    void Awake()
    {
        isJump = false;
        rigid = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>(); // AudioSource component 얻기 
   
    }

    private void Update()
    {
        if(Input.GetButtonDown("Jump")&&!isJump) //Space bar 누를 시 점프 이동 
        {
            isJump = true;
            rigid.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
        
        }
        
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal"); //왼 ,오
        float v = Input.GetAxisRaw("Vertical");// 위 아래
        rigid.AddForce(new Vector3(h, 0, v), ForceMode.Impulse);//x축 에 horizontal,z축에 Vertical ,순간적으로 이동 할때 Impulse
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Floor") // 충돌되있는 놈 이름이 Floor 이면 점프 상태 변경
        {
            isJump = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //if (other.name == "Item") //Player과 부딪친 놈의 이름이 Item일 때 
        if(other.tag=="Item") // name property can't be duplicated so we should use tag property  
        {
            
            itemCount++; //-> score  증가 
            audio.Play(); // 해당 Audio Clip 실행 
            other.gameObject.SetActive(false); //other->Item.gameObject.SetActive(bool):오브젝트 활성화 함수 
        }
        else if(other.tag=="Finish")
        {
            if(manager.TotalItemCount==itemCount)
            {
                //GameClear
                SceneManager.LoadScene("Example1_1");
            }
            else
            {
                SceneManager.LoadScene("Example1_0"); //Scence 불러오기
                //Restart
            }
        }
    }
}

