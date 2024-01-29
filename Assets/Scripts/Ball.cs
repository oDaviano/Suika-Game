using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    [SerializeField] int kind;
    bool merged = false;
    string prefabPath = "Sprites/Ball/Ball_";
    Transform myTransform;
    [SerializeField] bool onLine = false;

    GameManager gameManager;
    void Start() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        myTransform = transform;

    }

    public void SetMerged(bool isMerged) {
        merged = isMerged;
    }
    public int GetKind() {
        return kind;
    }
    public bool GetLine() {
        return onLine;
    }

    public bool GetMerged() {
        return merged;
    }
    public void Merge(int kind, Vector2 position) {
        GameObject mergedBall = Instantiate(Resources.Load<GameObject>(prefabPath + (kind + 1)), position, Quaternion.Euler(0, 0, 0));
    }
    /*
     * 콜라이더 충돌 감지하여
     *같은 과일끼리 충돌시 상위단계 과일로 합성
     * 수박은 합성대신 소멸
     */


    private void OnCollisionEnter2D(Collision2D collision) {
        GameObject collisionObj = collision.gameObject;
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Ball"))) {

            if (collision.gameObject.GetComponent<Ball>().GetKind() == kind && !merged && !collisionObj.GetComponent<Ball>().GetMerged()) {

                collisionObj.GetComponent<Ball>().SetMerged(true);//합성 중복실행 방지
                merged = true;
                Vector2 center = new Vector2((transform.position.x + collisionObj.transform.position.x) / 2, (transform.position.y + collisionObj.transform.position.y) / 2);
                if (kind < 10) {
                    Merge(kind, center);

                }
                gameManager.GetScore(kind);
                Destroy(collision.gameObject);
                Destroy(gameObject);

            }
            else if (collision.gameObject.GetComponent<Ball>().GetKind() != kind) {
                if (collisionObj.GetComponent<Ball>().GetLine() && transform.position.y > 2.1f) {
                    gameManager.GameOver();
                }
            }

        }

    }

    /*
     게임오버 로직 관련
    게임 오버 라인 위로 넘어가거나 현재 게임오버 라인에 접촉한 과일에 새 과일이 낙하하 경우 게임오버
     */

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.name == "GameOverLine") {
            onLine = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.name == "GameOverLine") {
            onLine = false;
            if (gameObject.transform.position.y > collision.gameObject.transform.position.y) {
                gameManager.GameOver();
            }
        }
    }

}
