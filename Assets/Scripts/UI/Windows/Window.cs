using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    [SerializeField] private WindowType windowType;

    public WindowType WindowType => windowType;

    public virtual void Open() => gameObject.SetActive(true);
    public virtual void Close() => gameObject.SetActive(false);
}