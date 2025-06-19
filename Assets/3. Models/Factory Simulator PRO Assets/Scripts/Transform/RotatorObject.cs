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

    // to rotate runtime transform objects
    public class RotatorObject : TransformerObject
    {


        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            x_function += () =>
            {
                object_to_transform?.transform.Rotate(Input.GetAxis("Mouse Y") * transform_speed, 0, 0);

            };

            y_function += () =>
            {

                object_to_transform?.transform.Rotate(0, Input.GetAxis("Mouse X") * transform_speed, 0);
            };

            z_function += () =>
            {

                object_to_transform?.transform.Rotate(0, 0, Input.GetAxis("Mouse X") * transform_speed);
            };


        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }


}