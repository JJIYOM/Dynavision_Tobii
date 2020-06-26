using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * UI 관련 스크립트
 */

public class UIManager : MonoBehaviour
{
    #region 변수
    public GameObject StartPanel;
    public GameObject ResultPanel;
    public GameObject PausePanel;
    public Text Stage;
    public int _stage = 1;
    public Text time;
    public float _time = 0f;
    public Text ResultTime;
    public Text[] ResponseTime;
    public float _result = 0f;
    #endregion

    #region Singleton
    private static UIManager _uiManager;
    public static UIManager UI
    {
        get { return _uiManager; }
    }

    void Awake()
    {
        _uiManager = GetComponent<UIManager>();
    }
    #endregion

    public void OnApplicationQuit() //게임 종료 시 데이터 CSV 출력
    {
        UpdateCarFile();
        UpdateCarFileR();
    }

    public void GameStart() //게임 시작
    {
        Time.timeScale = 1f;
        GazeEvent.GE.TotalTimer = true;
        StartPanel.SetActive(false);
        //CreateCarFile(); //파일 만들기
        //CreateCarFileR();
        GameManager.GM.StartCoroutine("RandomColor"); //코루틴 실행

        //GameManager.GM.RandomC(); //몇초뒤에 뜨는걸로 해야할듯 처음에는
    }

    public void ReStart() //다시 시작
    {
        SceneManager.LoadScene("Dynavision");
    }

    public void ViewStage() //Stage text 설정
    {
        Stage.text = "Stage : " + _stage + " / 12";
    }

    public void ViewTime() //시선 반응 시간 text 설정
    {
        time.text = string.Format("Time : {0:N3}", _time);
        //  = "Time : " + _time;
    }

    public void ViewResult() //결과 화면
    {
        ResultPanel.SetActive(true);
        for (int i = 0; i < GazeEvent.GE.FirstTimelist.Count; i++) //각 단계 반응 시간
        {
            ResponseTime[i].text = string.Format("{0:N3}", GazeEvent.GE.FirstTimelist[i]);
        }
        ResultTime.text = string.Format("평균시간 : {0:N3}", _result); //평균 시간
        Time.timeScale = 0f;
        //= "평균시간 : " + _result;      
    }

    public void Pause() //일시정지
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Continue() //계속하기
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Exit() //나가기
    {
        Application.Quit();
    }

    void CreateCarFile() //Saved_data_Dyna.csv 생성
    {
        if (!System.IO.File.Exists(Dyna_Data.getPath()) || new FileInfo(Dyna_Data.getPath()).Length == 0)
        {
            string filePath = Dyna_Data.getPath();
            StreamWriter outStream = System.IO.File.CreateText(filePath);

            Dyna temp = new Dyna
            {
                EyeLocation_x = "0",
                EyeLocation_y = "0"
            };
            Dyna_Data.Dyna.Add(temp);
            string str = Dyna_Data.Dyna[0].EyeLocation_x + "," + Dyna_Data.Dyna[0].EyeLocation_y;

            outStream.WriteLine("EyeLocation_x,EyeLocation_y");
            outStream.WriteLine(str);
            outStream.Close();
        }
        else
        {
            List<Dictionary<string, object>> data = CSVReader.Read(@Dyna_Data.getPath());

            for (var i = 0; i < data.Count; i++)
            {
                Dyna mt = new Dyna();
                mt.EyeLocation_x = data[i]["EyeLocation_x"].ToString();
                mt.EyeLocation_y = data[i]["EyeLocation_y"].ToString();

                Dyna_Data.Dyna.Add(mt);
            }
        }
    }

    void UpdateCarFile() //Saved_data_Dyna.csv에 값 덮어쓰기
    {
        string filePath = Dyna_Data.getPath();
        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine("Time,EyeLocation_x,EyeLocation_y,CheckPoint,LightOn");
        for (int i = 0; i < Dyna_Data.Dyna.Count; i++)
        {
            string str = Dyna_Data.Dyna[i].t_Time + "," + Dyna_Data.Dyna[i].EyeLocation_x + "," + Dyna_Data.Dyna[i].EyeLocation_y + "," + Dyna_Data.Dyna[i].CheckPoint + "," + Dyna_Data.Dyna2[i].LightOn;
            outStream.WriteLine(str);
        }

        outStream.Close();
    }

    void CreateCarFileR() //Dyna_Result.csv 생성
    {
        if (!System.IO.File.Exists(Dyna_Result.getPath()) || new FileInfo(Dyna_Result.getPath()).Length == 0)
        {
            string filePath = Dyna_Result.getPath();
            StreamWriter outStream = System.IO.File.CreateText(filePath);

            outStream.WriteLine("Response_Time,O_X");
            outStream.Close();
        }
        else
        {
            List<Dictionary<string, object>> data = CSVReader.Read(@Dyna_Result.getPath());

            for (var i = 0; i < data.Count; i++)
            {
                DynaR mt = new DynaR();
                mt.Response_Time = data[i]["Response_Time"].ToString();
                mt.O_X = data[i]["O_X"].ToString();

                Dyna_Result.DynaR.Add(mt);
            }
        }
    }

    void UpdateCarFileR() //Dyna_Result.csv에 값 덮어쓰기
    {
        string filePath = Dyna_Result.getPath();
        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine("Stage,Response_Time,O_X,StayTime,BallNum");
        for (int i = 0; i < Dyna_Result.DynaR2.Count; i++)
        {
            string str = Dyna_Result.DynaR[i].Stage + "," + Dyna_Result.DynaR[i].Response_Time + "," + Dyna_Result.DynaR[i].O_X + "," + Dyna_Result.DynaR2[i].StayTime + "," + Dyna_Result.DynaR[i].BallNum;

            outStream.WriteLine(str);
        }

        outStream.Close();
    }

}
