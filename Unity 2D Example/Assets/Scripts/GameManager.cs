using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int health;
    public Player player;
    public GameObject[] Stages; //스테이지를 관리하는 배열 
    public Image[] UIhealth; //UI(이미지)를 담을 변수
    public Text UIPoint;// UI(텍스트)를 담을 변수
    public Text UIStage;// UI(텍스트)를 담을 변수 
    public GameObject UIRetry;// UI(버튼)을 담을 변수
    public void NextStage()
    {
        if(stageIndex<Stages.Length-1) //다음 스테이지가 있을 시
        {
            Stages[stageIndex].SetActive(false);//전 스테이지 active 끄지
            stageIndex++;
            Stages[stageIndex].SetActive(true); //다음 스테이지 active 키기
            PlayerReposition(); // 자리 리스폰
            PlayerReposition(); // 자리 리스폰

            UIStage.text = "STAGE" + (stageIndex + 1);
        }
        else //게임 끝났을 때
        {
            Time.timeScale = 0;//시간 멈추기
            Text btnText = UIRetry.GetComponentInChildren<Text>(); //버튼의 자식객체인 Text를 불러오기위해 Children이 붙음 
            btnText.text = "Game Clear";// Retry인 기본 텍스트를 Game Clear로 변경
            Debug.Log("게임 클리어");
            UIRetry.SetActive(true); //버튼 보이기 
        }
       
        totalPoint += stagePoint;
        stagePoint = 0;
    }
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        UIPoint.text = (totalPoint + stagePoint).ToString();//Point UI에  point변수에 값을 문자열로 변경하여 display
    }
    public void HealthDown() //HP 감소 함수 
    {
        if(health>1)
        {
            health--;
            UIhealth[health].color = new Color(1, 0, 0, 0.4f);//해당하는 목숨 값을 어둡게 
        }
        else
        {
            
            UIhealth[0].color = new Color(1, 0, 0, 0.4f); //마지막 목숨 꺼지게 
            player.OnDie();
            UIRetry.SetActive(true); // Retry 버튼 보여주기 
            
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")// 현재 GameManager의 collider의 역할은 낭떠러지 이기 때문에 Player와 부딪힌다는 것은 Player가 낭떠러지로 떨어졌다는 뜻 
        {
           if(health>1)
            {
                PlayerReposition();//리스폰 
            }
            HealthDown();
            
        }
    }
    void PlayerReposition()
    {
        player.transform.position= new Vector3(0, 2, -1);//리스폰 지역으로 돌려놓음
        player.VelocityZero();
    }
    public void Reset()
    {
        Time.timeScale = 1; //시간멈춘 것을 복구 
        SceneManager.LoadScene(0);//1번째 scene 시작 
    }
}
