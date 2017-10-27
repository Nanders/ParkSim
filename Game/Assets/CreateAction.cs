using System;
using UnityEngine;

public class CreateAction : Action, IAction
{
    private GameObject go;
    public GameObject createdGo { get; private set; }
    private Vector3 createPosition;
    private Quaternion createRotation;

    public CreateAction(GameObject gameObject, Vector3 position, Quaternion rotation)
    {
        this.go = gameObject;
        this.createPosition = position;
        this.createRotation = rotation;

        Do();
        ActionManager.obj.PushAction(this);
    }

    public void Do()
    {
        createdGo = GameObject.Instantiate(go, createPosition, createRotation);
    }

    public void Undo()
    {
        createPosition = createdGo.transform.position;
        createRotation = createdGo.transform.rotation;
        GameObject.Destroy(this.createdGo);
    }
}
