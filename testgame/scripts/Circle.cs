using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Rockhoppers.scripts
{
    public struct Circle
    {
        public float Radius { get; set; }

        public float Area { get => 2 * MathF.PI * Radius; }

        public Vector2 Position { get; set; }

        public Circle(float _radius, Vector2 _position)
        {
            Radius = _radius;
            Position = _position;
        }

        public Circle(float _radius)
        {
            Radius = _radius;
            Position = Vector2.Zero;
        }

        public bool Contains(Vector2 target)
        {

            if(Vector2.Distance(Position,target) < Radius)
            {
                return true;
            }

            return false;
        }
    }
}
