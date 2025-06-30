using UnityEngine;

public class LinkPoint : MonoBehaviour
{
    public Machine machine; //연결점의 현재 장비
    public LinkPoint linkedPoint; //현재 연결된 포인트

    bool init;
    public void Init()
    {
        if (init)
            return;
        init = true;
        gameObject.tag = "LinkPoint";
        machine = GetComponentInParent<Machine>();
    }

    //이거를 Edit 모드에서 마우스를 땠을때 호출해도 괜찮네
    public LinkPoint Link() 
    {
        Init();
        Debug.Log($"LinkPoint Link() 1 machine id {machine.id}");
        Collider[] cols = Physics.OverlapBox(transform.position, new Vector3(0.5f, 0.5f, 0.5f),Quaternion.identity, LayerMask.GetMask("LinkPoint"));

        Debug.Log($"LinkPoint Link() 2 cols.Length {cols.Length}");
        if (cols.Length <= 0)
            return null;

         for(int i =0;i< cols.Length; i++)
        {
            LinkPoint linkPoint = cols[i].GetComponent<LinkPoint>();
            if (linkPoint.machine == machine)
            {
                Debug.Log($"LinkPoint Link() 3 if (linkPoint.machine == machine)");
                continue;
                
            }
                
            //방향이 다르면 못하게 처리 
            if( machine is Conveyor && linkPoint.machine is Conveyor)
            {
                if((linkPoint.machine as Conveyor).direction != (machine as Conveyor).direction)
                {
                    Debug.Log($"LinkPoint Link() 3 direction != direction");
                    continue;
                }
            }

            Debug.Log($"LinkPoint Link() 4 링크 완료 ");
            linkedPoint = linkPoint;
            return linkPoint;
        }
        Debug.Log($"LinkPoint Link() 5 링크 불가 null ");
        return null;
        
    }


    //public bool canLink; 
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (linkedPoint != null)
    //        return;

    //    if (collision.CompareTag("LinkPoint"))
    //    {
    //         LinkPoint linkPoint = GetComponent<LinkPoint>();
    //        if (linkPoint.machine == machine)//같은 기기임
    //            return;
    //        canLink = true; 
    //        //machine의 위치를 
    //    }
    //}

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
