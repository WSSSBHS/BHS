using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;
using UnityEngine.UI;

public class UINameBar : MonoBehaviour
{
    public Text avaverName;
    public Camera camera;
    public Character character;
    // Start is called before the first frame update
    void Start()
    {
        if (this.character != null)
        {
            UpdateInfo();
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInfo();

        //this.transform.LookAt(Camera.main.transform, Vector3.up);   //����������İ취 ��������
        this.transform.forward = camera.transform.forward;        //����������İ취2  ��������
    }

    void UpdateInfo()
    {
        if (this.character != null)
        {
            string  name = this.character .Name + "LV"+this.character.Info.Level;
            if (name != this.avaverName.text)
            { 
            this.avaverName.text = name;
            }
        }
    }
}
