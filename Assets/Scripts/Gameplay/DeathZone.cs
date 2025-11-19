using UnityEngine;
using Core;

namespace Gameplay
{
    /// <summary>
    /// Detects when the ball enters the bottom zone and triggers a game loss.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class DeathZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(Tags.Ball))
            {
                return;
            }

            Debug.Log("[DeathZone] Ball reached death zone");
            GameManager.Instance.SetLose();
        }
    }
}