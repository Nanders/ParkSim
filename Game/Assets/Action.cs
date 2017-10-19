using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//base class for all trackable actions
public class Action : IUndoAction
{
    public virtual void Do(){}
    public virtual void Undo(){}
}

public class MoveAction : Action
{
    private GameObject go;
    private Vector3 from;
    private Vector3 to;

    public MoveAction(GameObject gameObject, Vector3 fromPosition, Vector3 toPosition)
    {
        go   = gameObject;
        from = fromPosition;
        to = toPosition;
    }
     
    public override void Do()
    {
        throw new NotImplementedException();
    }

    public override void Undo()
    {
        throw new NotImplementedException();
    }
}

public class CreateAction : Action
{
    public override void Do()
    {
        throw new NotImplementedException();
    }

    public override void Undo()
    {
        throw new NotImplementedException();
    }
}

public class DestroyAction : Action
{
    public override void Do()
    {
        throw new NotImplementedException();
    }

    public override void Undo()
    {
        throw new NotImplementedException();
    }
}