using UnityEngine;

public class LinkPoint : MonoBehaviour
{
    public Machine machine; //연결점의 현재 장비
    public LinkPoint linkedPoint; //현재 연결된 포인트

     
    private void Awake()
    {
        gameObject.tag = "LinkPoint";
        machine = GetComponentInParent<Machine>();
    }

    //이거를 Edit 모드에서 마우스를 땠을때 호출해도 괜찮네
    public LinkPoint Link() 
    {
        Collider[] cols = Physics.OverlapBox(transform.position, new Vector3(0.3f, 0.3f, 0.3f),Quaternion.identity, LayerMask.GetMask("LinkPoint"));

        if (cols.Length <= 0)
            return null;

        linkedPoint = cols[0].GetComponent<LinkPoint>();
        return linkedPoint;
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
