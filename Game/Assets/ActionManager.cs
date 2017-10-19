using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : BehaviourSingleton<ActionManager>
{
    public int undoLength;
    private DropoutStack<IAction> undoStack = new DropoutStack<IAction>(20);

    public void PushAction(IAction action)
    {
        undoStack.Push(action);
    }

    public void Redo()
    {
        throw new NotImplementedException();
    }

    public void Undo()
    {
        var action = undoStack.Pop();
        action.Undo();
    }
}

public interface IAction
{
    void Do();
    void Undo();
}
