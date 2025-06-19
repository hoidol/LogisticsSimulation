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
using UnityEngine.UI;
using UnityEngine.EventSystems;



namespace FactorySimulatorPro
{
    // button to help spawn object into the scene
    public class ObjectSpawnerButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {

        [SerializeField]
        private GameObject object_to_spawn_prefab;


        private GameObject spawned_object;


        private bool follow_mouse; // if spawned object should follow mouse movement

        private bool is_mouse_in;

        private int layer_index = 7; // layer for the ground. place object on the ground when ray cast hit to the ground is detected

        private int layer_mask;

        private void Start()
        {
            layer_mask = 1 << layer_index;
        }


        // Update is called once per frame
        void Update()
        {

            if (is_mouse_in && Input.GetMouseButtonDown(0)) // if mouse cursor is over button and user clicks down
            {
                spawned_object = Instantiate(object_to_spawn_prefab);

                follow_mouse = true;
            }


            if (follow_mouse)
            {
                RaycastHit hit;

                // place spawned object at the point the mouse' raycase hits a ground colider
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, layer_mask))
                {
                    spawned_object.transform.position = hit.point;
                }
                else
                {
                    // place spawned object at a distance of 10 away from mouse cursor if no ground collider is found
                    spawned_object.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + (Camera.main.ScreenPointToRay(Input.mousePosition).direction * 10);

                }

            }


            if (Input.GetMouseButtonUp(0))
            {
                follow_mouse = false;
            }
        }




        public void OnPointerEnter(PointerEventData eventData)
        {
            is_mouse_in = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            is_mouse_in = false;
        }


    }

}