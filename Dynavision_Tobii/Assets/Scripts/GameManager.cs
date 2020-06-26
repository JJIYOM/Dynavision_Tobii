using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region 변수
    public GameObject Spheres;
    public Material Red;
    public Material White;
    public string RandomName;
    public float sum;
    public List<int> Overlap;
    public int sphereNum;
    #endregion

    #region Singleton
    private static GameManager gm;
    public static GameManager GM
    {
        get { return gm; }
    }

    private void Awake()
    {
        gm = GetComponent<GameManager>();
    }
    #endregion

    //컬러변환 함수
    IEnumerator RandomColor()
    {
        float time = 5f - GazeEvent.GE.seeTime;
        yield return new WaitForSeconds(time);

        if (Overlap.Count < 12) //총 12번, 60초
        {            
            if (Overlap.Count > 0) //단계 끝날 때 마다 데이터 저장
            {
                DynaR2 temp2 = new DynaR2();
                temp2.StayTime = GazeEvent.GE.stayTime.ToString();
                Dyna_Result.DynaR2.Add(temp2);
                Debug.Log(GazeEvent.GE.stayTime);
            }

            GazeEvent.GE.seeTime = 0f;

            sphereNum = Random.Range(0, 64);
            Overlap.Add(sphereNum);//랜덤 나온거 추가하고
            RandomName = sphereNum.ToString(); //첫번째 원

            //랜덤 빨간색
            Spheres.transform.GetChild(sphereNum).GetComponent<MeshRenderer>().material = Red;

            GazeEvent.GE.timerOn = true;
            GazeEvent.GE.IslightOn = true;
            GazeEvent.GE.FirstLightOn = true;

        }
        else //12번 반복이 끝나면
        {
            DynaR2 temp2 = new DynaR2();
            temp2.StayTime = GazeEvent.GE.stayTime.ToString();
            Dyna_Result.DynaR2.Add(temp2);
            Debug.Log(GazeEvent.GE.stayTime);

            //평균 구하기
            for (int i = 0; i < GazeEvent.GE.FirstTimelist.Count; i++)
            {
                sum += GazeEvent.GE.FirstTimelist[i];
            }
            UIManager.UI._result = sum / GazeEvent.GE.FirstTimelist.Count;

            UIManager.UI.ViewResult();

            Debug.Log("Finish");
        }
    }
    
    //컬러변환 함수
    public void LightOff()
    {
        Spheres.transform.GetChild(sphereNum).GetComponent<MeshRenderer>().material = White; //흰색으로 lightOff

        UIManager.UI._stage++;
        if (UIManager.UI._stage != 13) UIManager.UI.ViewStage(); //stage text
        GazeEvent.GE.timerOn = false;
        GameManager.GM.StartCoroutine("RandomColor");
    }

    #region 주석
    //3초 이상 안닿으면 깜박깜박 -> 깜빡 2번(2초)해도 안보면 다음으로 넘어가
    /*public IEnumerator LightOn()
    {
        for (int i = 0; i < 2; i++)
        {
            Spheres.transform.GetChild(sphereNum).GetComponent<MeshRenderer>().material = White;
            yield return new WaitForSeconds(0.5f);
            Spheres.transform.GetChild(sphereNum).GetComponent<MeshRenderer>().material = Red;
            yield return new WaitForSeconds(0.5f);
        }

        Spheres.transform.GetChild(sphereNum).GetComponent<MeshRenderer>().material = White;

        AddSaveXResult();

        UIManager.UI._stage++;
        if (UIManager.UI._stage != 31) UIManager.UI.ViewStage();
        GazeEvent.GE.timerOn = false;
        GameManager.GM.StartCoroutine("RandomColor");
    }*/
    #endregion

}



/* 
 * 현재 setting
 랜덤 1~4초 후 빨간 공 점등   -> 수정 어떻게 할지?
 눈으로 보는거 5초 안에 안보면 깜빡깜빡 -> 3초 안보면 2초동안 깜빡이
*/
