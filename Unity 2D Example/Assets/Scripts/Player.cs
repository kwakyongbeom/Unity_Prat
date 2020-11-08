using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxSpeed;
    public float jumPower;
    Rigidbody2D rigid;
    SpriteRenderer sRenderer;
    Animator anim;
    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update() //보통 키의 입력에 의한 상태변화가 담겨 있음 
    {
        //Stop Speed
        if(Input.GetButtonUp("Horizontal"))//만약 x축 방향 버튼이 띄어지면  A,D ,<- ->
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f,rigid.velocity.y);//버튼이 때어지면 속도를 normailized=1로 
            
        }

        
        if(Input.GetButtonDown("Horizontal")) //만약 x축 방향 버튼이 눌려지면  A,D ,<- ->
        {
            
            sRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;//만약 현재 보고있는 방향과 반대방향의 키가 입력되면 flipX가 발동되어 방향 전환 
            
        }
        if (Input.GetButtonUp("Jump")&& !anim.GetBool("isJumping")) //만약 Space bar 눌려지고 현재 상태가 점프 상태가 아니라면[무한 점프 방지]  
        {
            rigid.AddForce(Vector2.up * jumPower, ForceMode2D.Impulse); //Up 방향으로 점프 진행
            anim.SetBool("isJumping", true); //isJumping 변수 값을 true로 변경

        }

        if (Mathf.Abs(rigid.velocity.x)<0.3)//만약 속도의 절대값의 크기가 0.3 보다 작으면 멈춰있는 것으로 인식
        {
            anim.SetBool("isWalking", false);// animator에서 Bool type 의 isWalking 이란 변수의 값을 false로 바꿈 
        }
        else
        {
            anim.SetBool("isWalking", true);
        }
    }
    // Update is called once per frame
    void FixedUpdate() //순간 순간 상태를 제어할 메소드 
    {
       
        float h = Input.GetAxisRaw("Horizontal");// A,D  <-,-> 방향키 입력 받음(Horizontal), W,S,위,아래 입력받음(Vertical)
        rigid.AddForce(Vector2.right * h,ForceMode2D.Impulse);
    
        if(rigid.velocity.x>maxSpeed) // if current right direction velociy is over the maxSpeed 
        {
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y); //then velocity is changed to maxSpeed
        }
        else if(rigid.velocity.x<(maxSpeed*(-1))) // if current left direction velociy is over the maxSpeed
        {
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y); //then velocity is changed to maxSpeed 
        }


        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1,LayerMask.GetMask("Floor"));// RaycastHit-> ray가 닿은 object, Raycast는 물리적인 요소이기 때문에 Physics에 있음,parm1:ray출발점,parm2:방향,parm3:사정거리,parm4:Floor의 Layer name을 갖고있는 놈과 만나는 Ray
        Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));//Ray를 밑 방향으로 Green color로 그림 Debug때문에 Game 화면에는 안보임
        if(rigid.velocity.y<0) //y값의 - 즉, 떨어지는 순간에 
        {
            
            if (rayHit.collider != null) //어떤 것에 맞았다면 
            {
               
                if (rayHit.distance < 0.5f) //Ray의 거리가 0.3보다 작으면 
                {
                    Debug.Log("NN");
                    anim.SetBool("isJumping", false);//isJumping이라는 Boolean 변수를 false로 바꿈 
                }
            }
        }
     
    }
}
