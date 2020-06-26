using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dyna_Data
{
    public static List<Dyna> Dyna = new List<Dyna>();
    public static List<Dyna2> Dyna2 = new List<Dyna2>();

    public static string getPath()
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

public class Dyna
{
    public string t_Time { get; set; }
    public string EyeLocation_x { get; set; }
    public string EyeLocation_y { get; set; }
    public string CheckPoint { get; set; }
}

public class Dyna2
{
    public string LightOn { get; set; }
}

public class Dyna_Result
{
    public static List<DynaR> DynaR = new List<DynaR>();
    public static List<DynaR2> DynaR2 = new List<DynaR2>();

    public static string getPath()
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

public class DynaR
{
    public string Stage { get; set; }
    public string Response_Time { get; set; }
    public string O_X { get; set; }
    public string BallNum { get; set; }
}

public class DynaR2
{
    public string StayTime { get; set; }
}