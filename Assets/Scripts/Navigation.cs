using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation : MonoBehaviour {

    private Transform currentTransform;
    private Color currentColor;
    
    private bool Choosed = false;

    RaycastHit hit;

    void FixedUpdate()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        
        
        if (Physics.Raycast(transform.position, fwd, out hit, 3))
        {            
            if (Choosed)
            {
                Vector3 TempPos = currentTransform.position;
                Quaternion TempRot = currentTransform.rotation;

                currentTransform.position = hit.transform.position;
                currentTransform.rotation = hit.transform.rotation;
                

                hit.transform.position = TempPos;
                hit.transform.rotation = TempRot;
                
            }
            else
            {
                if (currentTransform != null)
                {
                    //currentTransform.position = new Vector3(currentTransform.position.x, currentTransform.position.y, currentTransform.position.z + 0.1f);
                    currentTransform.GetComponent<Renderer>().material.color = currentColor;
                }

                currentTransform = hit.transform;
                currentColor = hit.transform.GetComponent<Renderer>().material.color;
                //currentTransform.position = new Vector3(currentTransform.position.x, currentTransform.position.y, currentTransform.position.z - 0.1f);

                currentTransform.GetComponent<Renderer>().material.color = new Color(0.5f, 0.8f, 0.1f);
            }            
        }            
    }




    public void ChooseOrDropIt()
    {
        Choosed = !Choosed;

        if (Choosed)
        {
            currentTransform.GetComponent<Renderer>().material.color = new Color(0.8f, 0.5f, 0.1f);            
        }

        currentTransform.GetComponent<Collider>().enabled = !Choosed;
        
    }    
}
