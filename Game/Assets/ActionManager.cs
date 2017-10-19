using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : BehaviourSingleton<ActionManager>
{
    public int undoLength;
    public int undoStackLegnth;
    public int redoStackLength;

    void Update()
    {
        undoStackLegnth = undoStack.Count();
        redoStackLength = redoStack.Count();
    }

    private DropoutStack<IAction> undoStack = new DropoutStack<IAction>(20);
    private DropoutStack<IAction> redoStack = new DropoutStack<IAction>(20);

    private IAction lastAction;

    public void PushAction(IAction action)
    {
        undoStack.Push(action);
        redoStack.Clear();
    }

    public void Redo()
    {
        var action = redoStack.Pop();
        if (action == null || action == lastAction)
            return;
        undoStack.Push(action);
        action.Do();
    }

    public void Undo()
    {
        var action = undoStack.Pop();
        if (action == null)
            return;
        redoStack.Push(action);
        action.Undo();
    }
}

public interface IAction
{
    void Do();
    void Undo();
}
