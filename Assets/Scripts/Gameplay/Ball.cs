using System;
using Core;
using Core.Events;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay
{
    /// <summary>
    /// Controls ball movement, launching and collision behavior.
    /// </summary>
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class Ball : MonoBehaviour
    {
        [SerializeField] 
        private float _speed = 8f;

        private Rigidbody _rb = null;
        private bool _launched = false;
        private bool _followPaddle = false;
        private Paddle _paddle = null;
        private Vector3 _lastVelocity = Vector3.zero;
        private PaddleInput _input = null;
        private EventBus _eventBus = null;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.useGravity = false;
            _rb.interpolation = RigidbodyInterpolation.Interpolate;

            _input = ServiceLocator.Resolve<PaddleInput>();
            _eventBus = ServiceLocator.Resolve<EventBus>();
            _paddle = FindAnyObjectByType<Paddle>();

            if (_paddle == null)
            {
                Debug.LogError("Ball: Paddle not found in scene!");
            }
        }

        private void Start()
        {
            ResetBall();
        }

        private void OnEnable()
        {
            _eventBus.Subscribe<GameRestartEvent>(OnRestart);
            _eventBus.Subscribe<GameStartedEvent>(OnGameStarted);
        }

        private void OnDisable()
        {
            _eventBus.Unsubscribe<GameRestartEvent>(OnRestart);
            _eventBus.Unsubscribe<GameStartedEvent>(OnGameStarted);
        }

        private void Update()
        {
            if (GameManager.Instance.State != GameManager.GameState.Playing)
            {
                Stop();
                return;
            }

            if (!_launched && _followPaddle)
            {
                FollowPaddlePosition();
            }

            if (!_launched && _input.Clicked)
            {
                Launch();
            }
        }

        private void FixedUpdate()
        {
            if (!_launched)
            {
                return;
            }

            _lastVelocity = _rb.linearVelocity;

            float currentSpeed = _rb.linearVelocity.magnitude;
            
            if (Mathf.Abs(currentSpeed - _speed) > 0.1f)
            {
                _rb.linearVelocity = _rb.linearVelocity.normalized * _speed;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (GameManager.Instance.State != GameManager.GameState.Playing)
            {
                return;
            }
            
            if (collision.collider.gameObject == _paddle.gameObject)
            {
                if (transform.position.z < _paddle.transform.position.z)
                {
                    Stop();
                    GameManager.Instance.SetLose();
                    return;
                }
                
                float hitOffset = transform.position.x - _paddle.transform.position.x;

                Vector3 direction = new Vector3(
                    hitOffset,
                    0f,
                    1f
                ).normalized;

                _rb.linearVelocity = direction * _speed;
                return;
            }
            
            Vector3 normal = collision.contacts[0].normal;
            Vector3 reflect = Vector3.Reflect(_lastVelocity, normal);

            reflect.x += Random.Range(-0.05f, 0.05f);

            _rb.linearVelocity = reflect.normalized * _speed;
        }

        private void OnRestart(GameRestartEvent _)
        {
            ResetBall();
            _input.ResetClick();
        }

        private void OnGameStarted(GameStartedEvent _)
        {
            ResetBall();
            _input.ResetClick();
        }

        private void FollowPaddlePosition()
        {
            if (_paddle == null)
            {
                return;
            }

            transform.position = new Vector3(
                _paddle.transform.position.x,
                0.5f,
                -6f
            );
        }

        private void Launch()
        {
            _launched = true;
            _followPaddle = false;

            Vector3 direction = new Vector3(
                Random.Range(-0.4f, 0.4f),
                0f,
                1f
            ).normalized;

            _rb.linearVelocity = direction * _speed;
        }
        
        private void ResetBall()
        {
            _launched = false;
            _followPaddle = true;

            FollowPaddlePosition();
            _rb.linearVelocity = Vector3.zero;
        }
        
        private void Stop()
        {
            _launched = false;
            _rb.linearVelocity = Vector3.zero;
        }
    }
}