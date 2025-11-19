using UnityEngine;

namespace Gameplay.Bricks
{
    /// <summary>
    /// Brick with two hit points. Changes color on first hit, destroyed on second hit.
    /// </summary>
    public class DoubleBrick : BrickBase
    {
        private readonly Color _hitColor = new Color(0.972f, 0.702f, 0.702f);
        private int _health = 2;
        private Renderer _renderer = null;
        private Material _material = null;

        protected override void Awake()
        {
            base.Awake();
            _renderer = GetComponentInChildren<Renderer>();
            _material = _renderer.material;
        }

        protected override void Hit()
        {
            _health--;

            if (_health == 1)
            {
                _material.color = _hitColor;
            }
            else if (_health <= 0)
            {
                base.Hit();
            }
        }
    }
}