using UnityEngine;
using UnityEngine.Events;

// namespace Unity.FPS.Game
// {
    public abstract class ProjectileBase : MonoBehaviour
    {
        public GameObject Owner { get; private set; }
        public Vector2 InitialPosition { get; private set; }
        public Vector2 InitialDirection { get; private set; }
        /* Do I need? */public Vector2 InheritedMuzzleVelocity { get; private set; }
        // public float InitialCharge { get; private set; }

        public UnityAction OnShoot;

        public void Shoot(WeaponController controller)
        {
            Owner = controller.Owner;
            InitialPosition = transform.position;
            InitialDirection = transform.up;
            InheritedMuzzleVelocity = controller.MuzzleWorldVelocity;
            // InitialCharge = controller.CurrentCharge;

            OnShoot?.Invoke();
        }
        public void EnemyShoot(EnemyController controller)
        {
            Owner = controller.Owner;
            InitialPosition = transform.position;
            InitialDirection = transform.up;
            // InheritedMuzzleVelocity = controller.MuzzleWorldVelocity;
            // InitialCharge = controller.CurrentCharge;

            OnShoot?.Invoke();
        }
    }
// }