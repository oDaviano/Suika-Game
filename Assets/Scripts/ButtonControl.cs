using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/*
 버튼 pressed 구현용
 */

public class ButtonControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    bool pressed = false;
    [SerializeField] int direction;
    GameManager gameManager;
    public void OnPointerDown(PointerEventData eventData) { 
        pressed = true; 
    }
    public void OnPointerUp(PointerEventData eventData) { 
     pressed = false; 
    }     
    void Start() {
       gameManager =  GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update() {
        if (pressed) {
            gameManager.MoveCursor(direction);
        }
    }

}
