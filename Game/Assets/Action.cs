using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//base class for all trackable actions
public class Action
{
}

public class MoveAction : Action, IAction
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
     
    public void Do()
    {
        throw new NotImplementedException();
    }

    public void Undo()
    {
        throw new NotImplementedException();
    }
}

public class CreateAction : Action, IAction
{
    private GameObject go;
    private GameObject createdGo;
    private Vector3 createPosition;
    private Quaternion createRotation;

    public CreateAction(GameObject gameObject, Vector3 position, Quaternion rotation)
    {
        this.go = gameObject;
        this.createPosition = position;
        this.createRotation = rotation;
    }

    public void Do()
    {
        createdGo = GameObject.Instantiate(go, createPosition, createRotation);
    }

    public void Undo()
    {
        GameObject.Destroy(this.createdGo);
    }
}

public class DestroyAction : Action, IAction
{
    public void Do()
    {
        throw new NotImplementedException();
    }

    public void Undo()
    {
        throw new NotImplementedException();
    }
}