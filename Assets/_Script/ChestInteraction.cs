using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInteraction : MonoBehaviour
{
   



    [SerializeField]private bool canInteract;

    public Interactable_Comp Interactable_Comp;
    public Animator animator;

    private void Start()
    {
        Interactable_Comp = GetComponent<Interactable_Comp>();       
    }


    private void Update()
    {
        
        

        if(Interactable_Comp.isInteracting)
        {
            if (!canInteract)
            {
                InteractLocked();
                Invoke("ResetInteract", 0.5f);
            }

            if (canInteract)
            {
                OnInteract();
            }
            
        }

    }


    public void OnInteract()
    {
        animator.SetBool("state", true);
        Invoke("ResetInteract", 0.5f);
    }

    public void InteractLocked()
    {
        animator.SetBool("shake",true);
    }

    public void ChangeInteractState(bool state)
    {
        canInteract = state;
    }

    public void ResetInteract()
    {
        canInteract = false;
        animator.SetBool("shake", false);
    }

    public bool GetInteractState()
    {
        return canInteract;
    }
    
}
