using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Dynavision 데이터 (CSV 항목)
 */

public class Dyna_Data //첫 번째 CSV
{
    public static List<Dyna> Dyna = new List<Dyna>();
    public static List<Dyna2> Dyna2 = new List<Dyna2>();

    public static string getPath() //저장 경로
    {
#if UNITY_EDITOR 
        return Application.dataPath + "/CSV/" + "Saved_data_Dyna.csv";
#elif UNITY_ANDROID
        return Application.persistentDataPath+"/Saved_data_Dyna.csv";
#elif UNITY_STANDALONE_WIN
        return Application.dataPath + "/CSV/" + "Saved_data_Dyna" + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv";
#endif
    }
}

public class Dyna //첫 번째 항목 1 
{
    public string t_Time { get; set; } //플레이 시간(실시간)
    public string EyeLocation_x { get; set; } //시선 좌표 x
    public string EyeLocation_y { get; set; } //시선 좌표 y
    public string CheckPoint { get; set; } //공을 보았을 때 시점 (공 번호)
}

public class Dyna2 //첫 번째 항목 2
{
    public string LightOn { get; set; } //불이 켜진 시점 (공 번호)
}

public class Dyna_Result //두 번째 CSV
{
    public static List<DynaR> DynaR = new List<DynaR>();
    public static List<DynaR2> DynaR2 = new List<DynaR2>();

    public static string getPath() //저장경로
    {
#if UNITY_EDITOR 
        return Application.dataPath + "/CSV/" + "Dyna_Result.csv";
#elif UNITY_ANDROID
        return Application.persistentDataPath+"/Dyna_Result.csv";
#elif UNITY_STANDALONE_WIN
       return Application.dataPath + "/CSV/" + "Dyna_Result" + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv";
#endif
    }
}

public class DynaR //두 번째 항목 1
{
    public string Stage { get; set; } //단계
    public string Response_Time { get; set; } //시선 반응 시간
    public string O_X { get; set; } //O_X 여부
    public string BallNum { get; set; } //공 번호
}

public class DynaR2 //두 번째 항목 2
{
    public string StayTime { get; set; } //시선이 공에 머무른 시간
}