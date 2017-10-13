using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation : MonoBehaviour {

    private Transform currentTransform;
    private GameObject Halo;
    private RaycastHit hit;

    [SerializeField] private GameManager gameManager;

    private bool Selected = false;

    [SerializeField] UnityEngine.UI.Text text;

    void Start()
    {
        Halo = Instantiate(Resources.Load("Halo")) as GameObject;        
    }

    void FixedUpdate()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        
        //Check for ray hits
        if (Physics.Raycast(transform.position, fwd, out hit, 3))
        {
            if (Selected) //Check for selected fragment
            {
                //Swap position of fragments
                Vector3 TempPos = currentTransform.position;
                Quaternion TempRot = currentTransform.rotation;
                currentTransform.position = hit.transform.position;
                currentTransform.rotation = hit.transform.rotation;
                hit.transform.position = TempPos;
                hit.transform.rotation = TempRot;
                
                //Swap address in Identificator
                Address3 Temp = currentTransform.GetComponent<Identificator>().address;
                currentTransform.GetComponent<Identificator>().address = hit.transform.GetComponent<Identificator>().address;
                hit.transform.GetComponent<Identificator>().address = Temp;

                //Swap fragments in massive
                if (GameManager.SwapFragments(currentTransform.GetComponent<Identificator>().address, hit.transform.GetComponent<Identificator>().address))
                {
                    //Some fragments was merged, redraw cube
                    Selected = false;
                    gameManager.Draw();
                }
            }
            else
            {
                currentTransform = hit.transform;
                Halo.transform.GetComponent<Renderer>().material.color = Color.cyan;
            }
            Halo.transform.position = currentTransform.position;
            Halo.transform.rotation = currentTransform.rotation;

            text.text = currentTransform.GetComponent<Identificator>().address.Side + "|" + currentTransform.GetComponent<Identificator>().address.Row + "|" + currentTransform.GetComponent<Identificator>().address.Col;
        }


    }

    public void ChooseOrDropIt()
    {
        if (currentTransform != null && !GameManager.IsEmpty(currentTransform.GetComponent<Identificator>().address))
        {
            Selected = !Selected;

            if (Selected)
                Halo.transform.GetComponent<Renderer>().material.color = Color.magenta;

            currentTransform.GetComponent<Collider>().enabled = !Selected;
        }
        else
            gameManager.Draw();
    }
}
