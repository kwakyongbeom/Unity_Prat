using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxSpeed;
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

    private void Update()
    {
        //Stop Speed
        if(Input.GetButtonUp("Horizontal"))//만약 x축 방향 버튼이 띄어지면  A,D ,<- ->
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f,rigid.velocity.y);//버튼이 때어지면 속도를 normailized=1로 
            
        }
        
        if(Input.GetButtonDown("Horizontal"))
        {
            Debug.Log(Input.GetAxisRaw("Horizontal"));//현재보고 있는 방향과 같은 방향의 키가  입력되면 1 반대방향 키가 입력되면 -1 
            sRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;//만약 현재 보고있는 방향과 반대방향의 키가 입력되면 flipX가 발동되어 방향 전환 
            
        }

        if(Mathf.Abs(rigid.velocity.x)<0.3)//만약 속도의 절대값의 크기가 0.3 보다 작으면 멈춰있는 것으로 인식
        {
            anim.SetBool("isWalking", false);// animator에서 Bool type 의 isWalking 이란 변수의 값을 false로 바꿈 
        }
        else
        {
            anim.SetBool("isWalking", true);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
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
    }
}
