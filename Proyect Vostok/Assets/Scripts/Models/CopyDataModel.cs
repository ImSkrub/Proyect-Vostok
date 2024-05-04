using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyDataModel : MonoBehaviour
{
    private Vector3 pos;
    private string action;

    public CopyDataModel(Vector3 pos, string action) 
    {
        this.pos = pos;
        this.action = action;
    }
    public Vector3 Pos { get { return pos; } }
    public string Action { get { return action; } }
}
