using System.Collections;
using System.Collections.Generic;
using Tobii.Gaming;
using UnityEngine;

public class GazeEvent : MonoBehaviour
{
    public int Stage = 0;
    public float time = 0; //UI 관련 시간
    public float T_time = 0;
    public float seeTime = 0;
    public float stayTime = 0;
    public bool timerOn = false;
    public bool IslightOn = false;
    public bool IsSee = false;
    public bool lightOff = false;
    public bool TotalTimer = false;
    public bool IsSeeR = false;
    public bool FirstLightOn = false;
    public List<float> FirstTimelist;


    //IEnumerator enumerator;

    private static GazeEvent ge;
    public static GazeEvent GE
    {
        get { return ge; }
    }

    private void Awake()
    {
        ge = GetComponent<GazeEvent>();
    }

    void Start()
    {

    }

    //수정 : 공 1초 stay 기능 삭제, 한번 보면 다음 단계
    //총 싸이클 5초, 1초보면 4초, 2초보면 3초 뒤에 공 뜨는 식으로
    //5*12 = 총 60초로 진행할 것

    void Update()
    {
        TimeCheck();

        if (IsSeeR) //이름이 동일하고 본거 체크 되었다면
        {
            stayTime += Time.deltaTime;
        }


        if (seeTime > 5 && (IslightOn == true)) //5초 이후
        {
            IslightOn = false;
            lightOff = true;
            IsSeeR = false;
            FirstTimelist.Add(0);
            stayTime = 0;
            AddSaveXResult();

            GameManager.GM.LightOff();

        }


        /*if (seeTime > 3 && (IslightOn == true)) //3초 안보면 2초 깜빡
        {
            enumerator = GameManager.GM.LightOn();
            StartCoroutine(enumerator);
            Debug.Log("깜빡깜빡");
            IslightOn = false;
        }*/
    }

    private void OnTriggerEnter(Collider other) //최초 시선 측정
    {

        if (other.gameObject.name == GameManager.GM.RandomName)
        {
            //StopCoroutine(enumerator);
            //other.transform.GetComponent<MeshRenderer>().material.color = Color.red;

            if (seeTime != 0 && timerOn && seeTime <= 5f) //0~5초 안에 보면
            {
                IsSee = true;
                IsSeeR = true;
                AddSaveOResult();

                FirstTimelist.Add(seeTime);
                UIManager.UI._time = seeTime;
                UIManager.UI.ViewTime();
                Debug.Log("SeeTime : " +seeTime); //최초 시간 출력
                timerOn = false;

                other.GetComponent<MeshRenderer>().material = GameManager.GM.White;
                GameManager.GM.StartCoroutine("RandomColor");
                UIManager.UI._stage++;
                if (UIManager.UI._stage != 13) UIManager.UI.ViewStage();
            }          
        }
    }

    //이거 우선 삭제
    private void OnTriggerStay(Collider other) //응시(우선 1초)
    {
        #region 주석
        //Debug.Log(other.gameObject.name);

        /* 
        if (other.gameObject.name == GameManager.GM.RandomName) //빨간 공 쳐다보면
        {
             time += Time.deltaTime;

             //여기서 Time이 1초 넘어가면
             if (time >= 1)
             {
                 //여기에 O들어가야대고...
                 Debug.Log("1초 응시완료");
                 other.GetComponent<MeshRenderer>().material = GameManager.GM.White;
                 GameManager.GM.StartCoroutine("RandomColor");
                 UIManager.UI._stage++;
                 if (UIManager.UI._stage != 31) UIManager.UI.ViewStage();
                 time = 0f;
             }
         }
         */
        #endregion

        if (other.gameObject.name == GameManager.GM.RandomName && IsSeeR) //이름이 동일하고 본거 체크 되었다면
        {
            stayTime += Time.deltaTime;
        }

        #region UI
        //if (other.gameObject.name == "Start")
        //{
        //    time += Time.deltaTime;

        //    //여기서 Time이 1초 넘어가면
        //    if (time >= 1)
        //    {
        //        UIManager.UI.GameStart();
        //        time = 0f;
        //    }
        //}
        //else if (other.gameObject.name == "ReStart")
        //{
        //    time += Time.deltaTime;

        //    //여기서 Time이 1초 넘어가면
        //    if (time >= 1)
        //    {
        //        UIManager.UI.ReStart();
        //        time = 0f;
        //    }
        //}
        //else if (other.gameObject.name == "Pause")
        //{
        //    time += Time.deltaTime;

        //    //여기서 Time이 1초 넘어가면
        //    if (time >= 1)
        //    {
        //        UIManager.UI.Pause();
        //        time = 0f;
        //    }
        //}
        //else if (other.gameObject.name == "Continue")
        //{

        //    time += Time.deltaTime;

        //    //여기서 Time이 1초 넘어가면
        //    if (time >= 1)
        //    {
        //        UIManager.UI.Continue();
        //        time = 0f;
        //    }
        //}
        //else if (other.gameObject.name == "Exit")
        //{
        //    time += Time.deltaTime;

        //    //여기서 Time이 1초 넘어가면
        //    if (time >= 1)
        //    {
        //        UIManager.UI.Exit();
        //        time = 0f;
        //    }
        //}
        #endregion
    }
    //시선속도 측정

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == GameManager.GM.RandomName)
            IsSeeR = false;
    }
    void AddSavedData()
    {
        GazePoint gazePoint = TobiiAPI.GetGazePoint();
        //시선 좌표 Data 
        //1920x1080 => 기존 : 좌측하단 0,0 기준
        //기준 변경 => 중앙(960,540)을 0,0으로 변경    
        Dyna Data = new Dyna();
        Data.t_Time = T_time.ToString();
        Data.EyeLocation_x = (gazePoint.Screen.x - 12 - 960).ToString();
        Data.EyeLocation_y = (gazePoint.Screen.y - 12 - 540).ToString();

        if (!IsSee && !lightOff)
            Data.CheckPoint = "";
        else if (!IsSee && lightOff) //보지 않고
            Data.CheckPoint = "X";
        else
            Data.CheckPoint = GameManager.GM.RandomName;

        Dyna2 Data2 = new Dyna2();

        if (FirstLightOn)
        {
            Data2.LightOn = GameManager.GM.RandomName;
            FirstLightOn = false;
        }
        else
        {
            Data2.LightOn = "";
        }
            
        //Debug.Log(temp.EyeLocation_x + "        " + temp.EyeLocation_y);wldud980416!!
        Dyna_Data.Dyna.Add(Data);
        Dyna_Data.Dyna2.Add(Data2);

        IsSee = false;
        lightOff = false;
    }

    public void AddSaveOResult()
    {
        Stage++;
        DynaR temp = new DynaR();
        temp.Stage = Stage.ToString();
        temp.Response_Time = seeTime.ToString();
        temp.O_X = "O";
        temp.BallNum = GameManager.GM.Overlap[Stage - 1].ToString();
        //temp.StayTime = stayTime.ToString();
        Dyna_Result.DynaR.Add(temp);

        stayTime = 0f;
    }

    void AddSaveXResult()
    {
        Stage++;
        DynaR temp = new DynaR(); //틀림, 반응시간 추가...
        temp.Stage = Stage.ToString();
        temp.Response_Time = "0";
        temp.O_X = "X";
        temp.BallNum = GameManager.GM.Overlap[Stage - 1].ToString();
        Dyna_Result.DynaR.Add(temp);
    }

    void TimeCheck()
    {
        if (TotalTimer) //전체 소요 시간
        {
            T_time += Time.deltaTime;
            AddSavedData();
        }

        if (timerOn)
        {
            seeTime += Time.deltaTime; //볼때까지의 시간 측정
        }

    }

}
