using System;
using UnityEngine;

[Serializable]
public class Attribute
{
    [NonSerialized]
    Stat parent;  
    public Attributes type;
    public ModifiableInt value;

    public void SetParent(Stat _parent)
    {
        parent = _parent;
        value = new ModifiableInt(AttributeModified);
    }

    public void AttributeModified()
    {}
}
