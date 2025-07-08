using System.Collections.Generic;
using UnityEngine;

public class LinkPoint : MonoBehaviour
{
    //현재 연결 포인트 닿아있는 머신 체크
    [SerializeField] private Machine machine; //연결점의 현재 장비
    public Machine Machine
    {
        get
        {
            if(machine == null)
                machine = GetComponentInParent<Machine>();
            return machine;
        }
    }
    public List<Machine> linkedMachines = new List<Machine>();
    
    public TransferDirection transferDirection;
    bool init;
    public bool isConveyor;
    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (init)
            return;
        
        init = true;
        gameObject.tag = "LinkPoint";
        machine = GetComponentInParent<Machine>();
        isConveyor = machine is Conveyor;
    }

    //이거를 Edit 모드에서 마우스를 땠을때 호출해도 괜찮네
    public void Link() 
    {
        Init();
        Collider[] cols = Physics.OverlapBox(transform.position,new Vector3( 0.3f,3f,0.3f),Quaternion.identity, LayerMask.GetMask("Machine"));

        linkedMachines.Clear();
        //Debug.Log($"LinkPoint Link() machine id {machine.id} cols.Length {cols.Length}");
        if (cols.Length <= 0)
            return;

         for(int i =0;i< cols.Length; i++)
        {
            Machine linkedMachine = cols[i].GetComponent<Machine>();
            if (machine == linkedMachine)
            {
                //Debug.Log($"같은 머신 제외");
                continue;
            }

            linkedMachines.Add(linkedMachine);
        }
        
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (SimulationManager.Instance.simulationModeType == SimulationModeType.Edit)
            return;

        if (!isConveyor)
            return;

        if (other.CompareTag("Box"))
        {
            Box box = other.GetComponent<Box>();

            if (box == null)
                return;

            //나가는거에 부딪혔을때랑 들어오는거에 부딪혔을때랑 다르지
            if (transferDirection == TransferDirection.In)
            {
                if (box.productData.asrs_id == machine.destinationId)
                {
                    //box- 현재 있는 장비를 참조하게
                    box.machine.Unload(box, machine);
                }
            }
            else
            {
                //박스가 있는 장비의 끝에 도착한 경우
                if (box.machine == machine)
                {
                    //Debug.Log("LinkPoint if (box.machine == machine)");
                    machine.Unload(box, machine.nextMachine);
                }

            }
        }
    }
   

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("LinkPoint"))
    //    {
    //        LinkPoint linkPoint = GetComponent<LinkPoint>();
    //        if (linkPoint.machine == machine)//같은 기기임
    //            return;
    //        canLink = true;
    //        //machine의 위치를 
    //    }
    //}
}
public enum TransferDirection
{
    In,   // 입고 방향
    Out   // 출고 방향
}