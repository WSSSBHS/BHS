using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharcterView : MonoBehaviour
{
    public GameObject[] Character;

    private int currentCharacter = 0;

    public int CurrectCharacter
    {
        get
        {
            return currentCharacter;
        }
        set
        {
            currentCharacter = value;
            this.UpdateCharacter();
        }
    }
        void UpdateCharacter()
    {
        for (int i = 0; i < 3; i++)
        {
            Character[i].SetActive(i == this.currentCharacter);
        }
    }

    private int bianHao;
    public int BIANHAO
    {
        get
        {

            return bianHao;
        }

        set
        {
            bianHao = value;
           this.JueSeSelect();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void JueSeSelect()
    {
        for (int i = 0; i < 3; i++)
        {
            this.Character[i].SetActive(i == this.bianHao);
        }
    
    }
    public void OnClicekCreate()
    { 
    

    }
}
