using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerCamera : MonoSingleton<MainPlayerCamera>
{
    public Camera cmra;
    public Transform viewPoint;
    public GameObject X;
    public GameObject player;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
       //Debug.Log($"------------------------------{X.transform.position.x}---------------------------------{X.transform.position.y}----------------------------------------{X.transform.position.z}");

    }

    private void LateUpdate()
    {
        if (player == null)
            return;

        this.transform.position = player.transform.position;
        this.transform.rotation = player.transform.rotation;
    }
}
