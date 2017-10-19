using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : BehaviourSingleton<ActionManager>
{
    public int undoLength;
    private DropoutStack<Action> undoStack;

	void Start ()
    {
        undoStack = new DropoutStack<Action>(undoLength);
    }
	
}

public interface IUndoAction
{
    void Do();
    void Undo();
}
