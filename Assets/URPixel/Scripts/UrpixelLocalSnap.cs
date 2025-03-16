using UnityEngine;
using Urpixel;

public class UrpixelLocalSnap : UrpixelSnap
{
    private Vector3 localPosition;

    protected override void OnEnable()
    {
        base.OnEnable();
        localPosition = transform.localPosition;
    }

    protected override void Update()
    {
        transform.localPosition = localPosition;
        base.Update();
    }
}
