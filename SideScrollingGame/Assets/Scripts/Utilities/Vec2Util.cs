using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vec2Util
{
    public static Vector2 Rotate(Vector2 vec, float radAngle) {
        float cos = Mathf.Cos(radAngle);
        float sin = Mathf.Sin(radAngle);
        return new Vector2(vec.x * cos - vec.y * sin, vec.x * sin + vec.y * cos);
    }

    public static Vector2 ToVec2(Vector3 vec3) {
        return new Vector2(vec3.x, vec3.y);
    }
}
