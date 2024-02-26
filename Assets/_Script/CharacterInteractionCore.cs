using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterInteractionCore : MonoBehaviour
{
    public Transform interactionPivot;
    public float interactionRadius;
    public bool DebugVisual;

    public Collider[] detects;
    public List<GameObject> detectedGameobjects = new List<GameObject>();



    public List<Interactable_Comp> detected_interactable = new List<Interactable_Comp>();
    public Interactable_Comp currentInteractable;




    private void Update()
    {
        if (interactionPivot == null) return;
        detects = Physics.OverlapSphere(interactionPivot.position, interactionRadius);
        AddRemoveDetectedGameobject();
        GetInteractableComponent();



        
        if (currentInteractable != null && Input.GetKeyDown(KeyCode.E))
        {
            currentInteractable.OnInteract();
        }
        
    }

    private void GetInteractableComponent()
    {
        detected_interactable.Clear();
        foreach (GameObject obj in detectedGameobjects)
        {
            Interactable_Comp interactableComp = obj.transform.root.GetComponent<Interactable_Comp>();
            if (interactableComp != null)
            {
                detected_interactable.Add(interactableComp);
            }
        }
        //current interractable
        if (detected_interactable.Count > 0)
        {
            currentInteractable = detected_interactable[0];
        }
        else
        {
            currentInteractable = null;
        }
    }

    private void AddRemoveDetectedGameobject()
    {

        //Add
        foreach (Collider collider in detects)
        {
            if (!detectedGameobjects.Contains(collider.transform.gameObject))
            {
                detectedGameobjects.Add(collider.transform.gameObject);
            }
        }
        // Remove 
        for (int i = detectedGameobjects.Count - 1; i >= 0; i--)
        {
            GameObject obj = detectedGameobjects[i];
            if (!detects.Any(collider => collider.transform.gameObject == obj))
            {
                detectedGameobjects.RemoveAt(i);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        if (DebugVisual)
        {
            Gizmos.DrawSphere(interactionPivot.position, interactionRadius);
        }
    }
}
