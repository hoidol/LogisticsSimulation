/*
 * 
 * Developed by Olusola Olaoye, 2023
 * 
 * To only be used by those who purchased from the Unity asset store
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



namespace FactorySimulatorPro
{
    
    [RequireComponent(typeof(BoxCollider))]

    // objects we can transform in runtime
    public class RunTimeTransformObject : MonoBehaviour
    {

        [SerializeField]
        private GameObject highlight_object; // a mesh we will show when this object is highlighted and we will hide when this object is not highlighted


        private void Start()
        {
            unHighlightObject();

            // only add this item to the list of all runtime objects in the app controller class if this object is not an item/container
            if(gameObject.GetComponent<Item>() == null)
            {
                AppController.Instance.all_run_time_objects.Add(this);
            }
        }

        private void OnDestroy()
        {
            if (!gameObject.GetComponent<Item>())
            {
                AppController.Instance.all_run_time_objects.Remove(this);
            }
        }
     
        private void Update()
        {
            // if we click down, update the highlight visibility of this object. if selected, then this object's highlight will be visible
            if (Input.GetMouseButtonDown(0))
            {
                AppController.Instance.updateHighlightOnSelectedObjects(this);
            }
        }
        // if mouse clicks on this object while notclicking over a UI boject, when select this object
        private void OnMouseDown()
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                AppController.Instance.onRuntimeTransformClickedOn(this);
            }
        }

        public void highlightObject()
        {
            if(highlight_object)
            {
                highlight_object.SetActive(true);
            }
        }

        public void unHighlightObject()
        {
            if (highlight_object)
            {
                highlight_object.SetActive(false);
            }
        }


    }
}