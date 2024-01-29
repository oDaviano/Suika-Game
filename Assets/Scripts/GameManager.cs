using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.IO;
using TMPro;
using DataSets;

public class GameManager : MonoBehaviour {

    [SerializeField] List<int> scores;
    [SerializeField] GameObject cursor;
    [SerializeField] GameObject gameOverLine;
    [SerializeField] GameObject gameOverPopUp;
    [SerializeField] GraphicRaycaster m_Raycaster;
    [SerializeField] EventSystem m_EventSystem;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI prevRecord;
    [SerializeField] TextMeshProUGUI pScoreText;
    [SerializeField] TextMeshProUGUI gameOverScoreText;
    [SerializeField] AudioSource UIAuidoSource;

    [SerializeField] Button LButton;
    [SerializeField] Button RButton;

    PointerEventData eventData;
    bool isPause = false;
    bool sound;
    int currentBall = 0;
    int nextBall = 1;
    int spawnCount = 1;
    int score = 0;
    int[] scoreDatas;

    float spawnDelay = 1.2f;
    float spawnDelayTime = 1.2f;

    [SerializeField] GameObject nextBallSprite;


    string prefabPath = "Sprites/Ball/Ball_";

    public void Spawn() {
        if (spawnDelay >= spawnDelayTime && !isPause) {

            GameObject newBall = Instantiate(Resources.Load<GameObject>(prefabPath + currentBall), cursor.transform.position, Quaternion.Euler(0, 0, 0));
            spawnCount++;
            spawnDelay = 0;
            currentBall = nextBall;
            if (spawnCount < 6) {

                nextBall = spawnCount;
            }
            else {
                nextBall = Random.Range(1, 6);
            }

           SetCursorAndBallSprite();
        }
    }


    public void Pause() {
        isPause = true;
        Time.timeScale = 0f;


    }
    public void Resume() {
        Time.timeScale = 1f;
        isPause = false;
    }

    public void Retry() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public void ReturnMain() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main");
    }


    public void GetScore(int i) {
        score += scoreDatas[i];

        pScoreText.text = gameOverScoreText.text = score.ToString();
        scoreText.text = "SCORE: " + score.ToString();
    }

    public void GameOver() {
        Pause();
        scores.Add(score);
        ScoreQuickSort(scores, 0, scores.Count - 1);

        while (scores.Count > 5) {
            scores.RemoveAt(scores.Count - 1);
        }

        DataSet.SavePlayerDatas(scores);
        gameOverPopUp.SetActive(true);
        gameOverPopUp.transform.Find("GameOverUI").Find("Score").GetComponent<TextMeshProUGUI>().text = score.ToString();

    }
    static void ScoreQuickSort(List<int> array, int p, int r) {//·©Å· Äü¼ÒÆ®
        if (p < r) {
            int q = Partition(array, p, r);
            ScoreQuickSort(array, p, q - 1);
            ScoreQuickSort(array, q + 1, r);
        }
    }


    static int Partition(List<int> array, int p, int r) {
        int q = p;
        for (int j = p; j < r; j++) {
            if (array[j] > array[r]) {
                Swap(array, q, j);
                q++;
            }
        }
        Swap(array, q, r);
        return q;
    }

    private static void Swap(List<int> arr, int a, int b) {
        int temp = arr[a];
        arr[a] = arr[b];
        arr[b] = temp;
    }

    public void SetPopupSize(GameObject popupUI) {
        DataSet.SetPopupSize(popupUI);
    }

    public void SetCursorAndBallSprite() {
       cursor.GetComponent<SpriteRenderer>().sprite = Resources.Load<GameObject>(prefabPath + currentBall).GetComponent<SpriteRenderer>().sprite;
       cursor.transform.localScale = Resources.Load<GameObject>(prefabPath + currentBall).transform.localScale;
       nextBallSprite.GetComponent<Image>().sprite = Resources.Load<GameObject>(prefabPath + nextBall).GetComponent<SpriteRenderer>().sprite;
       Vector2 scale = new Vector2(Resources.Load<GameObject>(prefabPath + nextBall).transform.localScale.x , Resources.Load<GameObject>(prefabPath + nextBall).transform.localScale.y);

        nextBallSprite.transform.localScale = scale;
    }

    void Start() {
        Time.timeScale = 1f;
        scoreDatas = new int[11];
        DataSet.SetScoreData(scoreDatas);
        cursor = GameObject.Find("Cursor");
        currentBall = 0;
        nextBall = 1;

      SetCursorAndBallSprite();
        DataSet.LoadPlayeDatas(scores);
        DataSet.LoadPlayeOption(sound);
        sound = DataSet.soundOn;
        UIAuidoSource.volume = sound ? 1 : 0;
        if (scores.Count > 0)
        {
            prevRecord.text = "RECORD: " + scores[0].ToString();
        }
        else
        {
            prevRecord.text = "RECORD: 0";
        }
        eventData = new PointerEventData(m_EventSystem);


    }

    public void MoveCursor(int dir) {
        if (!isPause) {
            Vector3 vec = cursor.transform.position;

            if ((vec.x >= -2.6f && vec.x <= 2.6f) || (vec.x > 2.6f && dir < 0) || (vec.x < -2.6f && dir > 0)) {
                cursor.transform.Translate(dir * Time.deltaTime, 0, 0);
            }
        }
    }

    private void Awake() {
        DataSet.SetResolution();
    }
    void Update() {

        DataSet.PopupAnimation(pScoreText.transform.parent.gameObject, 1f);
        DataSet.PopupAnimation(gameOverPopUp, 1f);

        spawnDelay += Time.deltaTime;
    }
}
