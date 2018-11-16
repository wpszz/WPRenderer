﻿using System.Collections.Generic;

namespace WPRenderer
{
    public class Light
    {
        public Vector3 direction;
        public Color color = Color.white;
        public float intensity = 1f;

        public Light(Vector3 direction, Color color, float intensity)
        {
            this.direction = direction.normalized;
            this.color = color;
            this.intensity = intensity;
        }
    }
}
