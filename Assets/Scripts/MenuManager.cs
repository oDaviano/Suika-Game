using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DataSets;
using TMPro;

public class MenuManager : MonoBehaviour
{

    List<int> scoreBoardScore;
    [SerializeField] Button BGMButton;
    [SerializeField] GameObject[] scoreAreas;
    [SerializeField] GameObject scoreBoard;
    [SerializeField] GameObject noRecordMessage;
    [SerializeField] AudioSource UIAudioSource;
    AudioSource BGMAudioSource;
    bool sound;

    public void EnterGame()
    {
        SceneManager.LoadScene("InGame");
    }

    public void QuitGame() {    
        Application.Quit();
    }

    public void SoundControl() {//BGM on/off
        sound = !sound;
        UIAudioSource.volume=BGMAudioSource.volume = sound ? 1 : 0;
        DataSet.SavePlayerOption(sound);

        BGMButton.transform.GetChild(0).gameObject.SetActive(!sound);
        BGMButton.transform.GetChild(1).gameObject.SetActive(sound);


    }

    public void SetScoreBoard()//스코어 보드 초기화
    {

        if (scoreBoardScore.Count >0)
        {
            ScoreQuickSort(scoreBoardScore, 0, scoreBoardScore.Count-1);
            scoreBoard.SetActive(true);
            noRecordMessage.SetActive(false);
            for(int i = 0;i< 5; i++)
            {
                if (i < scoreBoardScore.Count)
                {
                    scoreAreas[i].SetActive(true);
                    scoreAreas[i].transform.Find("Score").GetComponent<TextMeshProUGUI>().text = scoreBoardScore[i].ToString();
                }
                else
                {
                    scoreAreas[i].SetActive(false);
                }

            }
        }
        else
        {
            noRecordMessage.SetActive(true);
            scoreBoard.SetActive(false);
        }
    }

    static void ScoreQuickSort(List<int>array, int p, int r)//스코어보드 퀵소트
    {
        if (p < r)
        {
            int q = Partition(array, p, r);
           ScoreQuickSort(array, p, q - 1);
           ScoreQuickSort(array, q + 1, r);
        }
    }


    static int Partition(List<int> arr, int p, int r)
    {
        int q = p;
        for (int j = p; j < r; j++)
        {
            if (arr[j] > arr[r])
            {
                Swap(arr, q, j);
                q++;
            }
        }
        Swap(arr, q, r);
        return q;
    }
   private static void Swap(List<int>arr, int a, int b)
    {
        int temp = arr[a];
        arr[a] = arr[b];
        arr[b] = temp;
    }

    public void SetPopupSize(GameObject popupUI) {//팝업창 애니메이션
        DataSet.SetPopupSize(popupUI);
    }


    private void Awake() {
        DataSet.SetResolution();
    }
    void Start()
    {
    
        scoreBoardScore = new List<int>();
        Time.timeScale = 1f;//Ingame에서 Quit할 경우 TimeScale초기화
        DataSet.LoadPlayeDatas(scoreBoardScore);
        DataSet.LoadPlayeOption(sound);
        sound = DataSet.soundOn;
        if (BGMAudioSource == null) {
            BGMAudioSource = GameObject.Find("BGM").GetComponent<AudioSource>(); ;
        }
        UIAudioSource.volume=BGMAudioSource.volume = sound ? 1 : 0;
        BGMButton.transform.GetChild(0).gameObject.SetActive(!sound);
        BGMButton.transform.GetChild(1).gameObject.SetActive(sound);

    }
    void Update()
    {

        DataSet.PopupAnimation(scoreBoard.transform.parent.parent.gameObject,1f);
    }
}
