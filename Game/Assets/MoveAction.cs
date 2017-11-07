using System;
using UnityEngine;

public class MoveAction : PlayerAction, IAction
{
    private GameObject go;
    private Vector3 from;
    private Vector3 to;

    public MoveAction(GameObject gameObject, Vector3 fromPosition, Vector3 toPosition)
    {
        go = gameObject;
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