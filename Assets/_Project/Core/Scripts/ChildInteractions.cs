using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildInteractions : MonoBehaviour
{
    [SerializeField]
    private bool _isChildMonster;


    public bool ChildType
    {
        get { return _isChildMonster; }
        set { _isChildMonster = value; }
    }
}
