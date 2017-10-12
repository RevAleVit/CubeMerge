using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaming : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ItCube.Fill();
        CubeDrawing();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private void CubeDrawing()
    {
        foreach (ItSide side in ItCube.sides)
        {
            foreach (ItFragment fragment in side.fragments)
            {
                Transform Temp = fragment.gameobject.transform;

                Destroy(fragment.gameobject);

                fragment.gameobject = Instantiate(Resources.Load("Fragment"), Temp) as GameObject;
                fragment.gameobject.GetComponent<Renderer>().material.color = fragment.color;
                fragment.gameobject.transform.parent = this.transform;
                fragment.gameobject.GetComponent<Identificator>().address = fragment.address;
            }
        }
    }
}
