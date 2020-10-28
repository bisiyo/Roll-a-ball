using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mobile : MonoBehaviour
{
    private Material mymat;

    void Start()
    {
        mymat = GetComponent<Renderer>().material;


    }
    // store a reference to that collider in a variable named 'other'..
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Player")
        {
            mymat.SetColor("_EmissionColor", Color.white);
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.name == "Player")
        {
            mymat.SetColor("_EmissionColor", Color.black);
        }
    }
}
