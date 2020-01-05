using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrowNeedle : NeedleProperty
{
    private float speed;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        Random.InitState((int)System.DateTime.Now.Ticks);
        speed = -Random.Range(3f, 10f);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        gameObject.transform.Translate(new Vector2(0, speed *Time.deltaTime));
    }
}
