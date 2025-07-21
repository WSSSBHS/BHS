using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;

public class UIWorldElementManger : MonoSingleton<UIWorldElementManger>
{
    public GameObject nameBarProfab;
    private Dictionary<Transform,GameObject> elements = new Dictionary<Transform,GameObject>(); //×Öµä

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
    public void AddCharcterNameBar(Transform owner, Character character)
    {
        GameObject goNameBar = Instantiate(nameBarProfab, this.transform);
        goNameBar.name = "ÑªÌõ"+character.entityId;
        goNameBar.GetComponent<UIWorldManger>().owner = owner;
        goNameBar.GetComponent<UINameBar>().character = character;
        goNameBar.SetActive(true);
        this.elements[owner] = goNameBar;

    }
    public void RemovCharcterNameBar(Transform owner)
    {
        if (this.elements.ContainsKey(owner))
        { 
        Destroy(this.elements[owner]);
            this.elements.Remove(owner);
        }

    }
}
