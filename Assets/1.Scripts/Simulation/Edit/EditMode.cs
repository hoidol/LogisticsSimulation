using UnityEngine;
using UnityEngine.EventSystems;
public class EditMode: SimulationMode
{
    public static EditMode Instanace;
    public EditModePanel editModePanel;

    public EditModeType editModeType;
    public GroundGrid groundGrid;
    //기기 신규 추가
    //기기 배치 수정
    private void Awake()
    {
        Instanace = this;   
    }

    //public string newMachineKey;
    public Machine targetMachine;

    public override void StartMode()
    {
        editModePanel.StartEditMode();
        LinkIndicatorCanvas.Instance.gameObject.SetActive(false);

        groundGrid.gameObject.SetActive(true);
        targetMachine = null;
        SetEditMode(EditModeType.Add);
    }

    public override void EndMode()
    {
        groundGrid.gameObject.SetActive(false);
        LinkIndicatorCanvas.Instance.gameObject.SetActive(false);
        editModePanel.EndEditMode();   
    }

    //머신 추가하려고 버튼 누름
    public void TryToAddMachine(MachineName machineName)
    {
        if(targetMachine != null && targetMachine.machineName == machineName)
        {
            targetMachine = null;
            return;
        }
        targetMachine = MachineManager.Instance.Instantiate(machineName);
        SetEditMode(EditModeType.Adding);
    }

    void SetEditMode(EditModeType type)
    {
        editModeType = type;
        editModePanel.UpdatePanel();

        
    }

    Vector3 dragOffset;
    bool isDragging = false;

    public void SetTarget(Machine machine)
    {
        targetMachine?.Focus(false);
        targetMachine = machine;
        targetMachine?.Focus(true);
    }
    private void Update()
    {
        if (SimulationManager.Instance.simulationModeType != SimulationModeType.Edit)
        {
            return;
        }

        if (Input.GetMouseButton(1))
            return;

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (editModeType == EditModeType.Add)
        {
            //추가 모드에서 기존에 추가된 기기 클릭 시 - 클릭한 기기를 수정할 수 있게
            if (targetMachine != null)
                return;
            
            // 마우스 클릭 → 레이캐스트로 Machine 선택
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 200, LayerMask.GetMask("Machine"))
                    && Physics.Raycast(ray, out RaycastHit groundHit, 200, LayerMask.GetMask("Ground"))
                    )
                {
                    Machine clickedMachine = hit.collider.GetComponent<Machine>();
                    if (clickedMachine != null)
                    {
                        SetTarget(clickedMachine);
                        LinkIndicatorCanvas.Instance.gameObject.SetActive(false);
                        dragOffset = targetMachine.transform.position - groundHit.point;
                        isDragging = true;
                        targetMachine.Drag(true);
                        SetEditMode(EditModeType.Adjust);
                    }
                }
            }
        }
        else if(editModeType == EditModeType.Adding)
        {
            if (Input.GetKeyDown(KeyCode.Escape) )
            {
                MachineManager.Instance.Remove(targetMachine);
                SetTarget(null);
                SetEditMode(EditModeType.Add);
            }

            if (targetMachine != null)
            {
                Adding();
            }

            
        }
        else if (editModeType == EditModeType.Adjust)
        {
            

            Adjust();
        }
    }


    void Adding()
    {
        if (targetMachine == null)
            return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 200, LayerMask.GetMask("Ground")))
        {
            targetMachine.transform.position = new Vector3(hit.point.x, 0, hit.point.z);
            
            if (Input.GetMouseButtonDown(0))
            {
                MachineManager.Instance.AddMachine(targetMachine);
                targetMachine.Drag(false);
                Edited();
                SetTarget(null);
                SetEditMode(EditModeType.Add);
            }
        }


        RotationControl();
    }

    
    void Adjust()
    {
        if (targetMachine == null)
            return;
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            targetMachine = null;
            SetEditMode(EditModeType.Add);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            MachineManager.Instance.Remove(targetMachine);
            SetTarget(null);
            SetEditMode(EditModeType.Add);
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit, 200, LayerMask.GetMask("Machine")))
            {
                targetMachine.Drag(false);
                SetTarget(null);
                LinkIndicatorCanvas.Instance.gameObject.SetActive(false);
                SetEditMode(EditModeType.Add);
                return;
            }
            else
            {
                Machine machine = hit.collider.GetComponent<Machine>();
                if(machine != null)
                {
                    
                    LinkIndicatorCanvas.Instance.gameObject.SetActive(false);
                    if (targetMachine != machine)
                    {
                        targetMachine.Drag(false);
                        SetTarget(machine);
                        editModePanel.UpdatePanel();
                    }


                    targetMachine.Drag(true);

                    if (Physics.Raycast(ray, out RaycastHit groundHit, 200, LayerMask.GetMask("Ground")))
                    {
                        dragOffset = targetMachine.transform.position - groundHit.point;
                        isDragging = true;
                        return;
                    }
                }
                
            }
        }

        //꾹 누르고 있어야지 이동가능
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 200, LayerMask.GetMask("Ground")))
            {
                Vector3 newPos = hit.point + dragOffset;
                newPos.y = 0;
                targetMachine.transform.position = newPos;
                
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            targetMachine.Drag(false);
            Edited();
            //targetMachine = null;
            //SetEditMode(EditModeType.Add);
        }

        RotationControl();
    }

    void RotationControl()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Input.GetKey(KeyCode.LeftShift))
                targetMachine.transform.Rotate(0f, -90f, 0f);
            else
                targetMachine.transform.Rotate(0f, -45f, 0f);
            Edited();
        }
        // E 키: Y축 시계 방향 회전
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (Input.GetKey(KeyCode.LeftShift))
                targetMachine.transform.Rotate(0f, 90f, 0f);
            else
                targetMachine.transform.Rotate(0f, 45f, 0f);
            Edited();
        }
    }

    public void DeleteMachine()
    {
        if (targetMachine == null)
            return;

        MachineManager.Instance.Remove(targetMachine);
        LinkIndicatorCanvas.Instance.gameObject.SetActive(false);
        SetEditMode(EditModeType.Add);
    }

    //무언가가 수정됐을때 호출되는 함수
    public void Edited()
    {
        if (targetMachine != null)
        {
            BoxCollider box = targetMachine.GetComponent<BoxCollider>();

            Collider[] colliders = Physics.OverlapBox(
                box.bounds.center,
                box.bounds.extents*1.1f,
                transform.rotation,
                LayerMask.GetMask("Machine")
            );
            for(int i =0;i< colliders.Length; i++)
            {
                colliders[i].GetComponent<Machine>().EditMachine();
            }

            LinkIndicatorCanvas.Instance.ShowLink(targetMachine);
        }
            
        MachineManager.Instance.Save();
    }

    public void ClosedDetail()
    {
        if (SimulationManager.Instance.simulationModeType != SimulationModeType.Edit)
            return;
    }
}

public enum EditModeType
{
    Add, 
    Adjust,
    Adding //추가중
}
