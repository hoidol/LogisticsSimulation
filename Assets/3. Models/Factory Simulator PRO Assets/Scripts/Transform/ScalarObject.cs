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
    // to scale selected runtime objects
    public class ScalarObject : TransformerObject
    {
        // scaling for the x and z axis will depend on the foward direction of the camera relative to the forward and sideward direction of this transform object
        // scaling for the y axis will be more straightforward. 

        protected override void Start()
        {
            base.Start();

            x_function += () =>
            {

                float facing_dot_product = Vector3.Dot(transform.forward, Camera.main.transform.forward);

                if (facing_dot_product > -1 && facing_dot_product <= -0.5f)
                {
                    object_to_transform.transform.localScale += new Vector3(1, 0, 0) * -Input.GetAxis("Mouse X") * transform_speed;

                }
                else if (facing_dot_product > -0.5f && facing_dot_product <= 0)
                {
                    object_to_transform.transform.localScale += new Vector3(1, 0, 0) * Mathf.Sign(Vector3.Dot(transform.right, Camera.main.transform.forward)) * Input.GetAxis("Mouse Y") * transform_speed;

                }
                else if (facing_dot_product > 0 && facing_dot_product <= 0.5f)
                {
                    object_to_transform.transform.localScale += new Vector3(1, 0, 0) * Mathf.Sign(Vector3.Dot(transform.right, Camera.main.transform.forward)) * Input.GetAxis("Mouse Y") * transform_speed;

                }
                else if (facing_dot_product > 0.5f && facing_dot_product <= 1)
                {
                    object_to_transform.transform.localScale += new Vector3(1, 0, 0) * Input.GetAxis("Mouse X") * transform_speed;

                }
            };

            y_function += () =>
            {
                object_to_transform.transform.localScale += new Vector3(0, 1, 0) * Input.GetAxis("Mouse Y") * transform_speed;
            };

            z_function += () =>
            {
                float facing_dot_product = Vector3.Dot(transform.forward, Camera.main.transform.forward);

                if (facing_dot_product > -1 && facing_dot_product <= -0.5f)
                {
                    object_to_transform.transform.localScale += new Vector3(0, 0, 1) * -Input.GetAxis("Mouse Y") * transform_speed;

                }
                else if (facing_dot_product > -0.5f && facing_dot_product <= 0)
                {
                    object_to_transform.transform.localScale += new Vector3(0, 0, 1) * -Mathf.Sign(Vector3.Dot(transform.right, Camera.main.transform.forward)) * Input.GetAxis("Mouse X") * transform_speed;

                }
                else if (facing_dot_product > 0 && facing_dot_product <= 0.5f)
                {
                    object_to_transform.transform.localScale += new Vector3(0, 0, 1) * -Mathf.Sign(Vector3.Dot(transform.right, Camera.main.transform.forward)) * Input.GetAxis("Mouse X") * transform_speed;

                }
                else if (facing_dot_product > 0.5f && facing_dot_product <= 1)
                {
                    object_to_transform.transform.localScale += new Vector3(0, 0, 1) * Input.GetAxis("Mouse Y") * transform_speed;

                }
            };

        }


        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }

}