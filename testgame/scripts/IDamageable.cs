using System;
using System.Collections.Generic;
using System.Text;

namespace Rockhoppers.scripts
{
    public interface IDamageable
    {
        public float Health { get; set; }
        public bool IsDestroyed { get; set; }



        public void TakeDamage(float damage)
        {
            Health -= damage;

            if(Health <= 0)
                Destroy();          
        }



        public void Destroy()
        {
            IsDestroyed = true;
            Entity e = this as Entity;

            e.SpritePath = "explosion";
            e.deleteQueued = true;
            e.deleteDelay = 1.5f;
            e.Delete();
        }
    }
}
