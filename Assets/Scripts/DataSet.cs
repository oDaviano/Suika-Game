using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace DataSets
{
    public static class DataSet
    {

        static string dataFileName = "PlayerData.cfg";
        static string optionFileName = "PlayerSetting.cfg";
        public static bool soundOn;
        public static List<int> scores = new List<int>();

        public static bool SavePlayerDatas(List<int> score)
        {
            if (!File.Exists(Application.persistentDataPath + dataFileName))
            {
              var file =   File.Create(Application.persistentDataPath + dataFileName);
                file.Close();
                
            }
            StreamWriter sw = new StreamWriter(Application.persistentDataPath + dataFileName);
            if (score.Count > 0)
            {
                for (int i = 0; i < score.Count; i++)
                {
                    sw.WriteLine((i)+"="+ score[i]);
                }
            }
            sw.Close();
            return true;
        }

        public static bool SavePlayerOption(bool sound)
        {
            if (!File.Exists(Application.persistentDataPath + optionFileName))
            {
                File.Create(Application.persistentDataPath + optionFileName);
            }
            StreamWriter sw = new StreamWriter(Application.persistentDataPath + optionFileName);
            sw.WriteLine("Sound=" + sound);
            sw.Close();
            return true;
        }

        public static bool LoadPlayeDatas(List<int> score)
        {
            if (File.Exists(Application.persistentDataPath + dataFileName))
            {
                StreamReader sr = new StreamReader(Application.persistentDataPath + dataFileName);
                int count = 0;
                while (!sr.EndOfStream)
                {
                    
                    var line = sr.ReadLine();
                    if(line.Contains(count+"=")){
                        string[] s = line.Split("=");
                        int scorePoint;
                        int.TryParse(s[1], out scorePoint);
                        scores.Add(scorePoint);
                        score.Add(scorePoint);
                    }

                    count++;
                }
                sr.Close();
                    return true;
            }
            else
            {
                var file = File.Create(Application.persistentDataPath + dataFileName);
                file.Close();
                return false;
            }
        }

        public static bool LoadPlayeOption(bool sound)
        {
            if (File.Exists(Application.persistentDataPath + optionFileName))
            {
                StreamReader sr = new StreamReader(Application.persistentDataPath + optionFileName);
                while (!sr.EndOfStream)
                {

                    var line = sr.ReadLine();
                    if (line.Contains("Sound"))
                    {
                        string[] s = line.Split("=");
                        bool.TryParse(s[1], out soundOn);
                    }                  
                }

                sr.Close();
                return true;
            }
            else
            {
                var file = File.Create(Application.persistentDataPath + optionFileName);
                file.Close();
                StreamWriter sw = new StreamWriter(Application.persistentDataPath + optionFileName);
                sw.Write("Sound=" + true);
                sw.Close();
                StreamReader sr = new StreamReader(Application.persistentDataPath + optionFileName);
                while (!sr.EndOfStream)
                {

                    var line = sr.ReadLine();
                    if (line.Contains("Sound"))
                    {
                        string[] s = line.Split("=");
                        bool.TryParse(s[1], out soundOn);
                    }
                }

                sr.Close();
                return false;
            }
        }



        public static void SetPopupSize(GameObject popupUI) {
            popupUI.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        }
        public static void PopupAnimation(GameObject popUpUI, float speed)
        {//공용 팝업애니메이션
            float scale = popUpUI.transform.localScale.x;
            if (scale < 1.0f) {
                scale += speed * 0.2f;
                popUpUI.transform.localScale = new Vector3(scale, scale, scale);
            }
            else if (scale > 1.0f) {
                popUpUI.transform.localScale = new Vector3(1f, 1f, 1f);
            }

        }


        public static void SetResolution() {//해상도 고정 함수 - 차후 옵션 여러 개 만들 것

            int setWidth = 900;
            int setHeight = 1600; 

            int deviceWidth = Screen.width; 
            int deviceHeight = Screen.height;

            Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); 

            if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) 
            {
                float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight);
                Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f);
            }
            else 
            {
                float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); 
                Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight);
            }

        }
        public static void SetScoreData(int[] sd)
        {
            if (sd.Length == 11)
            {
                sd[0] =  (int) Scores.Cherry;
                sd[1] =  (int) Scores.Strawberry;
                sd[2] = (int)Scores.Grapes;
                sd[3] = (int)Scores.Dekopon;
                sd[4] = (int)Scores.Orange;
                sd[5] = (int)Scores.Apple;
                sd[6] = (int)Scores.Pear;
                sd[7] = (int)Scores.Peach;
                sd[8] = (int)Scores.Pineaplle;
                sd[9] = (int)Scores.Melon;
                sd[10] = (int)Scores.WaterMelon;
            }
        }

         enum Scores//점수 구조체 - 다른 저장방식 고려해볼 것
        {
            Cherry = 2,
            Strawberry = 4,
            Grapes = 6,
            Dekopon = 8,
            Orange = 10,
            Apple = 12,
            Pear = 14,
            Peach = 16,
            Pineaplle = 18,
            Melon = 20,
            WaterMelon = 22
        }

    }

}