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



namespace FactorySimulatorPro
{
    // this class controls the current transforming object (translation, rotation or scale)
    public class TransformObjectController : MonoBehaviour
    {
        public enum TransformType
        {
            none,
            translate,
            rotate,
            scale
        }

        private TransformType transform_type = TransformType.translate; // by default, the transform type should be translation



        [SerializeField]
        private TranslatorObject translator_object_prefab; // prefab for translation object

        [SerializeField]
        private RotatorObject rotator_object_prefab;// prefab for rotation object

        [SerializeField]
        private ScalarObject scale_object_prefab;// prefab for scale object


        private TranslatorObject translator_object;
        private RotatorObject rotator_object;
        private ScalarObject scale_object;


        [SerializeField]
        private Button translate_tool_button;

        [SerializeField]
        private Button rotate_tool_button;

        [SerializeField]
        private Button scale_tool_button;

        [SerializeField]
        private Button none_tool_button;



        void Start()
        {
            // instantiate each transformer prefab and assign them accordingly
            translator_object = Instantiate(translator_object_prefab);
            rotator_object = Instantiate(rotator_object_prefab);
            scale_object = Instantiate(scale_object_prefab);

            translator_object.gameObject.SetActive(false);
            rotator_object.gameObject.SetActive(false);
            scale_object.gameObject.SetActive(false);


            // change transform type to translation when translation button is clicked
            translate_tool_button.onClick.AddListener(() =>
            {
                transform_type = TransformType.translate;

                setTransformToolToActiveObjectPosition(translator_object.gameObject);
            });


            // change transform type to rotation when rotation button is clicked
            rotate_tool_button.onClick.AddListener(() =>
            {
                transform_type = TransformType.rotate;

                setTransformToolToActiveObjectPosition(rotator_object.gameObject);
            });

            // change transform type to scale when scale button is clicked
            scale_tool_button.onClick.AddListener(() =>
            {
                transform_type = TransformType.scale;

                setTransformToolToActiveObjectPosition(scale_object.gameObject);
            });

            // no transform tool to be used
            none_tool_button.onClick.AddListener(() =>
            {
                transform_type = TransformType.none;

                setAppropriateTransformToolToObject(null);
            });

        }


        private void Update()
        {
            updateTransformTypeViaKeyboard();
        }


        private void updateTransformTypeViaKeyboard()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                transform_type = TransformType.translate;
                setAppropriateTransformToolToObject(AppController.Instance.getLastSelectedRuntimeTransform().transform);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                transform_type = TransformType.rotate;
                setAppropriateTransformToolToObject(AppController.Instance.getLastSelectedRuntimeTransform().transform);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                transform_type = TransformType.scale;
                setAppropriateTransformToolToObject(AppController.Instance.getLastSelectedRuntimeTransform().transform);
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                transform_type = TransformType.none;
                setAppropriateTransformToolToObject(AppController.Instance.getLastSelectedRuntimeTransform().transform);
            }

        }

        private void setTransformToolToActiveObjectPosition(GameObject transform_tool)
        {
            if (AppController.Instance.getLastSelectedRuntimeTransform())
            {
                setAppropriateTransformToolToObject(AppController.Instance.getLastSelectedRuntimeTransform().transform);

            }
            else
            {
                transform_tool.SetActive(false);
            }
        }

        // show a particular transform tool depending on the transform type we are currently using. Only one transform tool can be shown at a time
        public void setAppropriateTransformToolToObject(Transform object_transform)
        {
            switch (transform_type)
            {
                case TransformType.translate:

                    translator_object.gameObject.SetActive(true);
                    rotator_object.gameObject.SetActive(false);
                    scale_object.gameObject.SetActive(false);

                    translator_object.object_to_transform = object_transform.gameObject;
                    translator_object.transform.position = object_transform.position;

                    break;




                case TransformType.rotate:

                    translator_object.gameObject.SetActive(false);
                    rotator_object.gameObject.SetActive(true);
                    scale_object.gameObject.SetActive(false);

                    rotator_object.object_to_transform = object_transform.gameObject;
                    rotator_object.transform.position = object_transform.position;

                    break;




                case TransformType.scale:

                    translator_object.gameObject.SetActive(false);
                    rotator_object.gameObject.SetActive(false);
                    scale_object.gameObject.SetActive(true);

                    scale_object.object_to_transform = object_transform.gameObject;
                    scale_object.transform.position = object_transform.position;

                    break;



                case TransformType.none:

                    translator_object.gameObject.SetActive(false);
                    rotator_object.gameObject.SetActive(false);
                    scale_object.gameObject.SetActive(false);

                    break;
            }
        }

    }


}