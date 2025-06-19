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




namespace FactorySimulatorPro
{

    // this is a base abstract class for the translation, rotation and scale classes which provide basic transform features for runtime
    public abstract class TransformerObject : MonoBehaviour
    {

        public GameObject object_to_transform // the game object we are transforming
        {
            get;
            set;
        }

        [SerializeField]
        protected GameObject x_controller; // translate on x, rotate on x or scale on x

        [SerializeField]
        protected GameObject y_controller;// translate on y, rotate on y or scale on y

        [SerializeField]
        protected GameObject z_controller; // translate on z, rotate on z or scale on z


        [SerializeField]
        protected float transform_speed = 1; // speed of translation, rotation or scale

        private float initial_transform_speed;

        // depending on if the mouse cursor's raycasts hits the x, y or z controller
        private bool did_click_x_controller;
        private bool did_click_y_controller;
        private bool did_click_z_controller;


        protected System.Action x_function, y_function, z_function;

        private int layer_index = 6; // layer 6 is the xray layer for the transform objects

        private int layer_mask;


        private Vector3 default_transform_object_scale;

        protected virtual void Start()
        {
            layer_mask = 1 << layer_index;

            default_transform_object_scale = transform.localScale;

            initial_transform_speed = transform_speed;
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if (Input.GetMouseButton(0) && (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0))
            {
                updateFunctions();
            }
          

            updateFirstHit();

            updateTransform();

            updateUpClickEvent();

            updateScaleOfTransformObjectReletiveToCameraDistance();

            updateTransformSpeedReletiveToCameraDistance();
        }

        private void updateFirstHit()
        {
            // find out which controller was hit when user left clicks 
            // ray casting is done from mouse cursor
            RaycastHit hit;
            if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, layer_mask))
            {
                did_click_x_controller = hit.collider.gameObject == x_controller;

                did_click_y_controller = hit.collider.gameObject == y_controller;

                did_click_z_controller = hit.collider.gameObject == z_controller;

            }
        }

        private void updateUpClickEvent()
        {
            if (Input.GetMouseButtonUp(0))
            {
                did_click_x_controller = false;
                did_click_y_controller = false;
                did_click_z_controller = false;


                x_controller.SetActive(true);
                y_controller.SetActive(true);
                z_controller.SetActive(true);
            }
        }

        private void updateTransform()
        {
            if (object_to_transform)
            {
                transform.position = object_to_transform.transform.position;
                transform.rotation = object_to_transform.transform.rotation;
                transform.SetAsLastSibling();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        
        private void updateFunctions()
        {
            if (did_click_x_controller)
            {
                // hide y and z controller when x controller is being used
                y_controller.SetActive(false);
                z_controller.SetActive(false);
                // if it is the translation tool, we will translate along the x axis, if rotation tool, we will rotate along the x axis
                // if it is scale tool, we will scale along the x axis
                x_function?.Invoke();  
            }

            if (did_click_y_controller)
            {
                // hide x and z controller when y controller is being used
                x_controller.SetActive(false);
                z_controller.SetActive(false);
                // if it is the translation tool, we will translate along the y axis, if rotation tool, we will rotate along the y axis
                // if it is scale tool, we will scale along the y axis
                y_function?.Invoke();
            }

            if (did_click_z_controller)
            {
                // hide x and y controller when z controller is being used
                x_controller.SetActive(false);
                y_controller.SetActive(false);
                // if it is the translation tool, we will translate along the z axis, if rotation tool, we will rotate along the z axis
                // if it is scale tool, we will scale along the z axis
                z_function?.Invoke();
            }
        }


        private void updateScaleOfTransformObjectReletiveToCameraDistance()
        {
            if(Camera.main)
            {
                float distance_to_camera = Vector3.Distance(Camera.main.transform.position, transform.position) / 10;
                distance_to_camera = Mathf.Clamp(distance_to_camera, 0.5f, 2.5f);

                // scale the transform object based on the distance of the transform object from the main camera
                transform.localScale = distance_to_camera * default_transform_object_scale;
            }
        }


        private void updateTransformSpeedReletiveToCameraDistance()
        {
            if (Camera.main)
            {
                // scale the transform object based on the distance of the transform object from the main camera
                transform_speed = (Vector3.Distance(Camera.main.transform.position, transform.position) / 10) * initial_transform_speed;
            }
        }

    }


}