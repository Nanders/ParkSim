using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : BehaviourSingleton<ActionManager>
{
    void Update()
    {
        //Ctrl+Z, Ctrl+Shift+Z
#if !UNITY_EDITOR
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Z))
        {
            if (Input.GetKey(KeyCode.LeftShift))
                Redo();
            else
                Undo();
        }
#endif
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.N))
        {
            Undo();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            Redo();
        }
#endif
    }

    private DropoutStack<IAction> undoStack = new DropoutStack<IAction>(100);
    private DropoutStack<IAction> redoStack = new DropoutStack<IAction>(100);

    public void PushAction(IAction action)
    {
        undoStack.Push(action);
        redoStack.Clear();
    }

    public void Redo()
    {
        var action = redoStack.Pop();
        if (action == null)
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


