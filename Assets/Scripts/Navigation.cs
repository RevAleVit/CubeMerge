using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation : MonoBehaviour {

    private Transform currentTransform;
    private GameObject Halo;
    private RaycastHit hit;    

    private bool Choosed = false;

    [SerializeField] UnityEngine.UI.Text text;

    void Start()
    {
        Halo = Instantiate(Resources.Load("Halo")) as GameObject;        
    }

    void FixedUpdate()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        
        
        if (Physics.Raycast(transform.position, fwd, out hit, 3))
        {            
            if (Choosed)
            {
                //Swap position of fragments
                Vector3 TempPos = currentTransform.position;
                Quaternion TempRot = currentTransform.rotation;
                currentTransform.position = hit.transform.position;
                currentTransform.rotation = hit.transform.rotation;                
                hit.transform.position = TempPos;
                hit.transform.rotation = TempRot;


                //Swap fragments in massive
                Gaming.SwapFragments(currentTransform.GetComponent<Identificator>().address, hit.transform.GetComponent<Identificator>().address);


                //Swap address in Identificator
                Address3 Temp = currentTransform.GetComponent<Identificator>().address;
                currentTransform.GetComponent<Identificator>().address = hit.transform.GetComponent<Identificator>().address;
                hit.transform.GetComponent<Identificator>().address = Temp;

            }
            else
            {
                currentTransform = hit.transform;
                Halo.transform.GetComponent<Renderer>().material.color = Color.green;
            }
            Halo.transform.position = currentTransform.position;
            Halo.transform.rotation = currentTransform.rotation;

            text.text = currentTransform.GetComponent<Identificator>().address.Side + "|" + currentTransform.GetComponent<Identificator>().address.Row + "|" + currentTransform.GetComponent<Identificator>().address.Col;

        }


    }

    


    public void ChooseOrDropIt()
    {
        if (!Gaming.IsEmpty(currentTransform.GetComponent<Identificator>().address))
        {
            Choosed = !Choosed;

            if (Choosed)
                Halo.transform.GetComponent<Renderer>().material.color = Color.red;

            currentTransform.GetComponent<Collider>().enabled = !Choosed;
        }
        else
            FindObjectOfType<Gaming>().Draw();
        
    }
}
