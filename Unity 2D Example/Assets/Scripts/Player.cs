using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public AudioClip audioJump;// 점프 clip
    public AudioClip audioAttack; //공격 clip
    public AudioClip audioDamaged;//데미지 clip
    public AudioClip audioItem;//아이템 획득 clip
    public AudioClip audioDie;//죽음 clip
    public AudioClip audioFinish;//성공 clip 
    public GameManager gameManager;//Game 전체적인관리를 위해서 Manager생산  
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer sRenderer;
    BoxCollider2D boxcollider;
    Animator anim;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        boxcollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() //보통 키의 입력에 의한 상태변화가 담겨 있음 
    {
        //Stop Speed
        if(Input.GetButtonUp("Horizontal"))//만약 x축 방향 버튼이 띄어지면  A,D ,<- ->
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f,rigid.velocity.y);//버튼이 때어지면 속도를 normailized=1로 
            
        }

        
        if(Input.GetButton("Horizontal")) //만약 x축 방향 버튼이 눌려지면  A,D ,<- ->
        {
            
            sRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;//만약 현재 보고있는 방향과 반대방향의 키가 입력되면 flipX가 발동되어 방향 전환 
            
        }


        if (Input.GetButtonUp("Jump")&& !anim.GetBool("isJumping")) //만약 Space bar 눌려지고 현재 상태가 점프 상태가 아니라면[무한 점프 방지]  
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse); //Up 방향으로 점프 진행
            anim.SetBool("isJumping", true); //isJumping 변수 값을 true로 변경
            PlaySound("JUMP"); //점프상황 재생 함수 실행
           

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
                    
                    anim.SetBool("isJumping", false);//isJumping이라는 Boolean 변수를 false로 바꿈 
                }
            }
        }
     
    }
    private void OnCollisionEnter2D(Collision2D collision)//충돌 발생 시
    {
        if(collision.gameObject.tag=="Enemy") //부딪힌 놈의 tag가 "Enemy"면
        {
            //Attack
            if(rigid.velocity.y<0&&transform.position.y>collision.transform.position.y)//만약 y축 velocity가 -값(떨어지는 중),충돌시 Player의 y좌표가 충돌한 Enemy의 y좌표보다 크다면(밞음, 즉 Attack)
            {
                onAttack(collision.transform);
            }
            else//Damanged
            {
                OnDamaged(collision.transform.position);// OnDamaged 함수가 실행되면서 파라미터로는 부딪힌놈의 좌표를 넘겨줌
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)//isTrigger가 체크가 되어있는 놈이 부딪혔을 때
    {
        if(collision.gameObject.tag=="Coin")// 그놈의 tag가 Coin일때
        {
            gameManager.stagePoint += 100;
            collision.gameObject.SetActive(false);//비활성화
            PlaySound("ITEM"); //아이템상황 재생 함수 실행
        }
        else if(collision.gameObject.tag=="End")//부딪힌 놈이 tag가 Finish면 
        {
            gameManager.NextStage();
            PlaySound("FINISH"); //성공상황 재생 함수 실행
        }
    }
    void OnDamaged(Vector2 targetPos)
    {
        gameManager.HealthDown();//데미지 입을때 -1 됨
        PlaySound("DAMAGED"); //데미지상황 재생 함수 실행

        gameObject.layer = 11;//Layer를 11번(PlayerDamaged)로 바꿈 [무적모드]
        sRenderer.color = new Color(1, 1, 1, 0.4f);//반투명

        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;//Player와 Enemy의 충돌 값이 0보다 크면 왼쪽[플레이어 기준]에서 충돌 했으므로 1[튕겨나갈방향:오른쪽] 아니면 오른쪽에서 충돌 했으므로 -1[튕겨나갈방향:왼쪽]
        rigid.AddForce(new Vector2(dirc, 1) * 7, ForceMode2D.Impulse);//전해진 튕겨나갈 방향에 7배로 팅겨 나간다  

        Invoke("OffDamaged", 3); //3초후 무적모드 해제
    }

    void OffDamaged() //데미지 받은 상태 해제,무적 모드 해제
    {
        
        gameObject.layer = 10;// layer를 11[PlayerDamaged] -> 10[Player]로 변경 
        sRenderer.color = new Color(1, 1, 1, 1);//원래대로 
    }
    void onAttack(Transform enemy)
    {
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);//반발략 Reaction Force
        PlaySound("ATTACK"); //공격상황 재생 함수 실행
        Monster monster = enemy.GetComponent<Monster>();
        monster.OnDamaged();//Player는 공격이지만 Monster입장에서는 damage 받은 것
        gameManager.stagePoint += 150;
    }
 
    public void OnDie()
    {
        PlaySound("DIE"); //점프상황 재생 함수 실행
        sRenderer.color = new Color(1, 1, 1, 0.4f);//몬스터 반투명
        sRenderer.flipY = true;//개구리처럼 뒤집어짐 
        boxcollider.enabled = false;// collider 비활성화 즉 물리 작용이 없어지면서 떨어짐
    }

    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero; //낙하 속도를 0으로
    }
    void PlaySound(string action) //상황마다 해당 소리 재생
    {
        switch(action)
        {
            case "JUMP":
                audioSource.clip = audioJump;
                break;
            case"ATTACK":
                audioSource.clip = audioAttack;
                break;
            case "DAMAGED":
                audioSource.clip = audioDamaged;
                break;
            case "ITEM":
                audioSource.clip = audioItem;
                break;
            case "DIE":
                audioSource.clip = audioDie;
                break;
            case "FINISH":
                audioSource.clip = audioFinish;
                break;
        }
        audioSource.Play();
    }
}
