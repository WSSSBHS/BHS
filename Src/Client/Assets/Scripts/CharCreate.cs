using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharCreate : MonoBehaviour
{
   
    public GameObject Select1;
    public GameObject Select2;
   
  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void back()
    {
        Select1.SetActive(true);
        Select2.SetActive(false);
    }
}
