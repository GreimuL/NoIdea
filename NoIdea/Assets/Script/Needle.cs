using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Needle : NeedleProperty
{
    public int moveDir = 1;
    public int moveType = 0;
    float speed;
    private Vector2 movement;
    private Vector2 currentPosition;

    public override void Start()
    {
        base.Start();
    }
    public void Awake()
    {
    }
    public void randomizeState()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        speed = Random.Range(3f, 5f);
        movement = setMovement(moveDir, moveType);
    }

    public override void Update()
    {
        base.Update();
        currentPosition = gameObject.transform.position;
        gameObject.transform.Translate(movement);
    }



    public void setVarialbe(int dir,int type)
    {
        moveDir = dir;
        moveType = type;
    }

    private Vector2 setMovement(int dir, int type)
    {
        switch (type)
        {
            case 0:
                return new Vector2(dir * Time.deltaTime * speed, 0);
            default:
                return new Vector2(dir * Time.deltaTime * speed, 0);

        }
    }
}
