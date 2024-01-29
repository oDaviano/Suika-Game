using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    public static BGM instance;
    // Start is called before the first frame update
    void Awake()
    {
        //BGM ����� �ҽ��� �̱���
        if (instance != null) {
            Destroy(this.gameObject);
            return;
        }      
        instance = this;
        DontDestroyOnLoad(this);    
    }
}
