

using System;

/// <summary>
/// Exception to use when something is wrong with the system dynamics.
/// </summary>
public class SystemDynamicsException : Exception
{
    // If a terminal state in the dynamics has actions, it's an issue.
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