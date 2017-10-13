using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Identificator : MonoBehaviour {

    public Address3 address{ get; set; }

    Transform[] sectors;

    void Awake()
    {
        sectors = GetComponentsInChildren<Transform>();
    }

    public void ApplyColor(Color color)
    {
        foreach(Transform sector in sectors)
        {
            sector.GetComponent<Renderer>().material.color = color;
        }
    }

    public void SwitchSectors(float value)
    {
        if(value != 0)
            for(int i = (int)(value * 4)+1; i < sectors.Length; i++)
            {
                sectors[i].gameObject.SetActive(false);
            }
    }
}
