using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWorldManger : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform owner;
    public float height = 1.5f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (owner != null)
        {
            this.transform.position = owner.position + Vector3.up * height;
        }
    }
}
