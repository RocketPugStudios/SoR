using WUG.BehaviorTreeVisualizer;

public abstract class Node: NodeBase
{
    //Keeps track of the number of times the node has been evaluated in a single 'run'.
    public int EvaluationCount;
    // Runs the logic for the node
    public virtual NodeStatus Run()
    {
        //Runs the 'custom' logic
        NodeStatus nodeStatus = OnRun();
        //Increments the tracker for how many times the node has been evaluated this 'run'
        EvaluationCount++;
        // If the nodeStatus is not Running, then it is Success or Failure and can be Reset
        if (nodeStatus != NodeStatus.Running)
        {
            Reset();
        }
        //Return the StatusResult.
        return nodeStatus;
    }
    public void Reset()
    {
        EvaluationCount = 0;
        OnReset();
    }
    protected abstract NodeStatus OnRun();
    protected abstract void OnReset();
}
