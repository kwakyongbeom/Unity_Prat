using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    Rigidbody2D rigid;
    public int nextMove;
    Animator anim;
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Invoke("Think", 5); // 5초 후 Think 라는 이름의 함수 실행
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y); //  nextMove라는 변수가 계속 변함 ,y는 현재 값을 계속 씀

        Vector2 fronVec = new Vector2(rigid.position.x + nextMove * 0.2f, rigid.position.y); //낭떠러지 체크 vector는 현재 x좌표에서 MoveRange의 0.2배 앞을 보고 체크한다.
        Debug.DrawRay(fronVec, Vector3.down, new Color(0, 1, 0));//Ray를 밑 방향으로 Green color로 그림 Debug때문에 Game 화면에는 안보임
        RaycastHit2D rayHit = Physics2D.Raycast(fronVec, Vector3.down, 1, LayerMask.GetMask("Floor"));// RaycastHit-> ray가 닿은 object, Raycast는 물리적인 요소이기 때문에 Physics에 있음,parm1:ray출발점,parm2:방향,parm3:사정거리,parm4:Floor의 Layer name을 갖고있는 놈과 만나는 Ray
        if(rayHit.collider==null)//만약 현재 감지된 Floor가 없다 즉 낭 떠러지다 
        {
            Turn();//방향 전환 함수 실행 
        }
    }
    void Think()
    {
        nextMove = Random.Range(-1, 2); // -1~1 까지

     
        anim.SetInteger("Walk Speed", nextMove);//animator의 상태를 변경하기위해 Walk Speed의 값을 바꿔준다 
        if(nextMove!=0)
        {
            spriteRenderer.flipX = nextMove == 1;//nextMove가 1이면 실행하고 -면 실행 x  
        }
        float nextThinkTIme = Random.Range(2f, 5f);// 2.0~4.99999 까지 
        Invoke("Think", nextThinkTIme); //재귀 함수 
    }

    void Turn()
    {
        nextMove *= -1;//방향을 바꿈
        spriteRenderer.flipX = nextMove == 1;//nextMove가 1이면 실행하고 -면 실행 x  
        CancelInvoke();//현재 기다리고 있는 Invoke 캔슬 
        Invoke("Think", 5);//다시 실행 
    }
}
