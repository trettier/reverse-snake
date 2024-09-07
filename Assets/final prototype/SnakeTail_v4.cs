using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeTail_v4 : MonoBehaviour
{
    private float size;

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        size = renderer.bounds.size[0];
    }

}