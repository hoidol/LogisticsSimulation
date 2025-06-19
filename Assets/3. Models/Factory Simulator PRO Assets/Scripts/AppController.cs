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
using UnityEngine.SceneManagement;



namespace FactorySimulatorPro
{
    [DefaultExecutionOrder(-100)]
    public class AppController : MonoBehaviour
    {
        private static AppController instance;
        public static AppController Instance
        {
            get
            {
                return instance;
            }
        }


        [SerializeField]
        private TransformObjectController transform_object_controller;



        [SerializeField]
        private Button delete_selected_objects_button; // delete selected object

        [SerializeField]
        private Button duplicate_selected_objects_button; // duplicate selected object

        [SerializeField]
        private Button refresh_scene_button; // restart scene

        [SerializeField]
        private Button flip_selected_objects_button; // flip a flipable object. Flipable object includes conveyor belt and in out machines


        // list of all runtime objects on scene
        public List<RunTimeTransformObject> all_run_time_objects = new List<RunTimeTransformObject>();

        // list of selected objects
        private List<RunTimeTransformObject> selected_runtime_transforms = new List<RunTimeTransformObject>();

        // if we are multiple selecting by holding down control or shift key
        public bool multiple_select
        {
            get;
            private set;
        }


        // Start is called before the first frame update
        void Start()
        {
            if (instance == null)
            {
                instance = this;
            }

            initTopButtonFunctionalities();

            refresh_scene_button.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
        }


        private void initTopButtonFunctionalities()
        {
            delete_selected_objects_button.onClick.AddListener(() => deleteObjects());
            duplicate_selected_objects_button.onClick.AddListener(() => duplicateObject());

            flip_selected_objects_button.onClick.AddListener(() => flipObject());

        }

       

        // delete all selected objects
        private void deleteObjects()
        {
            
            for (int i = 0; i < selected_runtime_transforms.Count; i++)
            {
                Destroy(selected_runtime_transforms[i].gameObject);
            }
            selected_runtime_transforms.Clear();
        }

        // duplicate all selected objects
        private void duplicateObject()
        {

            // only duplicate selected runtime transforms that have no parents
            for (int i = 0; i < selected_runtime_transforms.Count; i++)
            {
                if(selected_runtime_transforms[i].transform.parent == null)
                {
                    RunTimeTransformObject duplicate_object = Instantiate(selected_runtime_transforms[i], 
                                                                         selected_runtime_transforms[i].transform.position,
                                                                         selected_runtime_transforms[i].transform.rotation);

                    // unparent runtime transform children of the duplicate object
                    foreach(RunTimeTransformObject obj in duplicate_object.GetComponentsInChildren<RunTimeTransformObject>())
                    {
                        obj.transform.SetParent(null);
                    }
                }
            }
        }
        

        private void flipObject()
        {
            for (int i = 0; i < selected_runtime_transforms.Count; i++)
            {
                selected_runtime_transforms[i].GetComponent<IFlipable>()?.flip();
            }
        }


        private void Update()
        {
            updateMultipleSelectOnPC();

            updateKeyboardShortcut();

        }



        private void updateKeyboardShortcut()
        {
            // ctrl + D to duplicate object
            if((Input.GetKey(KeyCode.LeftControl)  || Input.GetKey( KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.D))
            {
                duplicateObject();
            }



            // CTRL + F to flip object
            if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.F))
            {
                flipObject();
            }



            // CTRL + A to highlight all objects
            if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.A))
            {

                // first of all, clear the selected objects list
                selected_runtime_transforms.Clear();

                // add all objects in the scene to the list of selected objects and highlight them
                for (int i = 0; i < all_run_time_objects.Count; i++)
                {
                    selected_runtime_transforms.Add(all_run_time_objects[i]);
                    all_run_time_objects[i].highlightObject();

                    // attach every other runtime object to the last one on the list
                    if (i != all_run_time_objects.Count - 1)
                    {
                        all_run_time_objects[i].transform.parent = all_run_time_objects[all_run_time_objects.Count - 1].transform;
                    }
                }
                transform_object_controller.setAppropriateTransformToolToObject(all_run_time_objects[all_run_time_objects.Count - 1].transform);
            }



            // CTRL + H to unhighlight all objects
            if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.H))
            {
                selected_runtime_transforms.Clear();

                // add all objects in the scene to the list of selected objects and highlight them
                for (int i = 0; i < all_run_time_objects.Count; i++)
                {
                    all_run_time_objects[i].unHighlightObject();
                    all_run_time_objects[i].transform.SetParent(null);
                }
                
            }



            // delete key to delete object
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                deleteObjects();
            }
        }


        // update the visibility of the highlight object for each runtime object in the scene
        // so only selected objects are highlighted
        public void updateHighlightOnSelectedObjects(RunTimeTransformObject runtime_object)
        {
            
            runtime_object.unHighlightObject();
            
            // highlight selected runtime objects 
            for (int i = 0; i < selected_runtime_transforms.Count; i++)
            {
                selected_runtime_transforms[i].highlightObject();

            }
        }

        // multiple select is on when holding down left or right control or shift key
        private void updateMultipleSelectOnPC()
        {
            multiple_select = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)
                              || Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl);
        }


        // when runtime object is clicked on, add it to the selected object list but first clear the list if it is not multiple select 
        public void onRuntimeTransformClickedOn(RunTimeTransformObject transform_object)
        {
            if (!multiple_select)
            {
                // unparent any object we had parented when transforming multiple objects
                foreach (RunTimeTransformObject runtime_object in selected_runtime_transforms)
                {
                    runtime_object.transform.SetParent(null);
                }
                selected_runtime_transforms.Clear();
            }

            selected_runtime_transforms.Add(transform_object);

            transform_object_controller.setAppropriateTransformToolToObject(transform_object.transform);


            // if we select more than one object, parent the other object(s) to the last selected object.
            // this will give us the ability to transform multiple objects in runtime.
            if (selected_runtime_transforms.Count > 1)
            {
                foreach (RunTimeTransformObject object_ in selected_runtime_transforms)
                {
                    object_.transform.SetParent(transform_object.transform);
                }
            }

        }

        //  returning the last selected object in a multiple selection
        public RunTimeTransformObject getLastSelectedRuntimeTransform()
        {
            if (selected_runtime_transforms.Count > 0)
            {
                return selected_runtime_transforms[selected_runtime_transforms.Count - 1];
            }
            return null;
        }

        // returning the first selected object in a multiple selection
        public RunTimeTransformObject getfirstSelectedRuntimeTransform()
        {
            if (selected_runtime_transforms.Count > 0)
            {
                return selected_runtime_transforms[0];
            }
            return null;
        }

        // returning the list of selected objects
        public List<RunTimeTransformObject> getSelectedRuntimeTransforms()
        {
            return selected_runtime_transforms;
        }


    }


}