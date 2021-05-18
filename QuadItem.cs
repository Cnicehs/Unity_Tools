using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadItem : MonoBehaviour,IQuad
{
    private Rect _rect;
    Rect rect
    {
        get
        {
            if(_rect==default(Rect))
            {
                _rect = new Rect(this.transform.position.x, this.transform.position.y, this.transform.lossyScale.x, this.transform.lossyScale.y)
                {
                    center = this.transform.position
                };
            }
            return _rect;
        }
    }
    public Vector2 GetPosition()
    {
        return this.transform.position;
    }

    public Rect GetRect()
    {
        return rect;
    }
}
