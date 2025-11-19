using Core;
using UnityEngine;

namespace Gameplay
{
    /// <summary>
    /// Player-controlled paddle that follows pointer movement.
    /// </summary>
    public class Paddle : MonoBehaviour
    {
        [SerializeField] private float _z = -7f;
        [SerializeField] private float _limitX = 7.5f;

        private PaddleInput _input = null;
        private Camera _camera = null;

        private void Awake()
        {
            _camera = Camera.main;
            _input = ServiceLocator.Resolve<PaddleInput>();
        }

        private void Update()
        {
            if (GameManager.Instance.State != GameManager.GameState.Playing)
            {
                return;
            }

            Vector2 screenPos = _input.PointerPosition;

            Ray ray = _camera.ScreenPointToRay(screenPos);
            Plane plane = new Plane(Vector3.up, new Vector3(0f, 0f, _z));

            float distance = 0f;
            bool hasHit = plane.Raycast(ray, out distance);

            if (!hasHit)
            {
                return;
            }

            Vector3 worldPoint = ray.GetPoint(distance);
            float clampedX = Mathf.Clamp(worldPoint.x, -_limitX, _limitX);

            transform.position = new Vector3(
                clampedX,
                transform.position.y,
                _z
            );
        }
    }
}