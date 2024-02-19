using System.Linq;


public abstract class Composite : Node
{
    protected int CurrentChildIndex = 0;
    //constructor
    protected Composite(string displayName, params Node[] childNodes)
    {
        Name = displayName;
        ChildNodes.AddRange(childNodes.ToList());
    }
}
