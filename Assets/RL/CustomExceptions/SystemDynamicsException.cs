

using System;

public class SystemDynamicsException : Exception
{
    private bool _terminalStateHasActions;
    public bool TerminalStateHasActions
    {
        get { return _terminalStateHasActions; }
    }

    public SystemDynamicsException()
    {
        _terminalStateHasActions = false;
    }

    public SystemDynamicsException(string message)
        : base(message)
    {
        _terminalStateHasActions = false;
    }

    public SystemDynamicsException(string message, Exception inner)
        : base(message, inner)
    {
        _terminalStateHasActions = false;
    }

    public SystemDynamicsException(string message, Exception inner, bool terminalStateHasActions)
      : base(message, inner)
    {
        _terminalStateHasActions = terminalStateHasActions;
    }

    public SystemDynamicsException(string message, bool terminalStateHasActions)
  : base(message)
    {
        _terminalStateHasActions = terminalStateHasActions;
    }

    public SystemDynamicsException(bool terminalStateHasActions)
    {
        _terminalStateHasActions = terminalStateHasActions;
    }
}