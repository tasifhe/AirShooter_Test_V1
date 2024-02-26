using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Comp : MonoBehaviour
{



    public bool isInteracting;


    private void Update()
    {
        
    }

    public void OnInteract()
    {
        Debug.Log(this.gameObject.name);
        isInteracting = true;
        OnInteractReset();
    }

    void OnInteractReset()
    {
        if (isInteracting)
        {
            Invoke("SetIsInteractingStateFalse", 0.5f);
        }
    }
   
    void SetIsInteractingStateFalse()
    {
        isInteracting = false;
    }

}
