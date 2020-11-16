using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    Rigidbody2D rigidbody;
    public bool isTouchTop;
    public bool isTouchBottom;
    public bool isTouchRight;
    public bool isTouchLeft;
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {

        float h = Input.GetAxisRaw("Horizontal"); // move Horizontal axis ,오른쪽일 때는 +1,왼쪽일 때는 -1  
        
        if((isTouchRight&&h==1)||(isTouchLeft&&h==-1)) //만약 오른쪽 경계를 건들였는데 계속 오른쪽 이동하거나 왼쪽 경계를 건들였는데 계속 왼쪽으로 이동할 시 
        {
            h = 0;
        }
        
        float v = Input.GetAxisRaw("Vertical"); // move Vertical axis ,위로는 +1,아래로는 -1

        if ((isTouchTop && v == 1) || (isTouchBottom && v == -1)) //만약 위쪽 경계를 건들였는데 계속 위쪽으로 이동하거나 아랫쪽 경계를 건들였는데 계속 아랫쪽으로 이동할 시 
        {
            v = 0;
        }

        Vector3 curPos = transform.position; // Current Position
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime; //nextMove
            
        transform.position = curPos + nextPos; //Change Position 


        if(Input.GetButtonDown("Horizontal")||Input.GetButtonUp("Horizontal")) //왼쪽 또는 오른쪽이 눌렸을 때는 -1,+1을 Input 파라미터로 전달 대신 h는 float이기 때문에 강제 casting 그리고 버튼이 다시 올라오면 0 값을 전달 
        {
            anim.SetInteger("Input", (int)h); 
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Border") //Border 태그와 만나면 isTouch 속성을 true로 바꿈
        {
           
            switch(collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = false; //상태 다시 false로변경
                    break;
                case "Bottom":
                    isTouchBottom = false; //상태 다시 false로변경
                    break;
                case "Left":
                    isTouchLeft = false;//상태 다시 false로변경 
                    break;
                case "Right":
                    isTouchRight = false; //상태 다시 false로변경
                    break;
            }
        }
    }
}
