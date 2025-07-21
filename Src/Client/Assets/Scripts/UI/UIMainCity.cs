using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;
using UnityEngine.UI;

public class UIMainCity : MonoBehaviour
{
    public Text avatorname;
    public Text avatorLevel;
    // Start is called before the first frame update
    void Start()
    {
        this.UpdateAvator();
    }
    void UpdateAvator()
    {
        this.avatorname.text = User.Instance.CurrentCharacter.Name+User.Instance.CurrentCharacter.Id; 

        this.avatorLevel.text =User.Instance.CurrentCharacter.Level.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
