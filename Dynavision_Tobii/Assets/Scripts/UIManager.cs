using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
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


    private static UIManager _uiManager;
    public static UIManager UI
    {
        get { return _uiManager; }
    }

    void Awake()
    {
        _uiManager = GetComponent<UIManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnApplicationQuit()
    {
        UpdateCarFile();
        UpdateCarFileR();
    }

    public void GameStart()
    {
        Time.timeScale = 1f;
        GazeEvent.GE.TotalTimer = true;
        StartPanel.SetActive(false);
        //CreateCarFile(); //파일 만들기
        //CreateCarFileR();
        GameManager.GM.StartCoroutine("RandomColor");

        //GameManager.GM.RandomC(); //몇초뒤에 뜨는걸로 해야할듯 처음에는
    }

    public void ReStart()
    {
        SceneManager.LoadScene("Dynavision");
    }

    public void ViewStage()
    {
        Stage.text = "Stage : " + _stage + " / 12";
    }

    public void ViewTime()
    {
        time.text = string.Format("Time : {0:N3}", _time);
        //  = "Time : " + _time;
    }

    public void ViewResult()
    {
        ResultPanel.SetActive(true);
        for (int i = 0; i < GazeEvent.GE.FirstTimelist.Count; i++)
        {
            ResponseTime[i].text = string.Format("{0:N3}", GazeEvent.GE.FirstTimelist[i]);
        }
        ResultTime.text = string.Format("평균시간 : {0:N3}", _result);
        Time.timeScale = 0f;
        //= "평균시간 : " + _result;      
    }

    public void Pause()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Continue()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Exit()
    {
        Application.Quit();
    }

    void CreateCarFile()
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

    void UpdateCarFile()
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

    void CreateCarFileR()
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

    void UpdateCarFileR()
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
