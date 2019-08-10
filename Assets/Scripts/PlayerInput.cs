using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public event Action<Vector2> OnDirectionPressed = (direction) => { };


    public Vector2? directionPressed = null;
    
//    [SerializeField] private SpawnerManual spawner = null;
    
    private enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            directionPressed = Vector2.down;
            OnDirectionPressed(Vector2.down);
//            spawner.Spawn();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            directionPressed = Vector2.up;

            OnDirectionPressed(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            directionPressed = Vector2.right;

            OnDirectionPressed(Vector2.right);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            directionPressed = Vector2.left;

            OnDirectionPressed(Vector2.left);
        }
    }
}
