using System;
public class EnvironmentAction
{

    public System.Action _actionToDoNoParameters;


    public EnvironmentAction(System.Action actionToDo)
    {
        _actionToDoNoParameters = actionToDo;
    }

    public void Execute()
    {
        _actionToDoNoParameters();
    }

    public override string ToString()
    {
        return _actionToDoNoParameters.Method.Name;
    }

    public override int GetHashCode()
    {
        return ToString().GetHashCode();
    }

    public override bool Equals(object obj)
    {
        var other = obj as EnvironmentAction;
        return other != null && other.ToString() == this.ToString();
    }
}
