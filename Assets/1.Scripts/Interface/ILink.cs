using System;
//연결할 수 있는 객체에 상속 
public interface ILink
{
    ILink startLink
    {
        get;
        set;
    }
    ILink endLink
    {
        get;
        set;
    }

    void Link(ILink link);
}
