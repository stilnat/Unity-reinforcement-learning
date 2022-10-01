using System;
public class EnvironmentAction
{

    private System.Action<Action> _actionToDo;
    private System.Action _actionToDoNoParameters;

    public EnvironmentAction(System.Action<Action> actionToDo)
    {
        _actionToDo = actionToDo;
    }

    public EnvironmentAction(System.Action actionToDo)
    {
        _actionToDoNoParameters = actionToDo;
    }

    public void Execute(Action actionParameters)
    {
        _actionToDo(actionParameters);
    }

    public void Execute()
    {
        _actionToDoNoParameters();
    }
}
