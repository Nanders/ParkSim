using System;
using UnityEngine;

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
