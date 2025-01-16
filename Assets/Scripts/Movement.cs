using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    [SerializeField] protected float maxSpeed = 5;
    [SerializeField]  float minSpeed = 5;
    [SerializeField]  float maxForce = 5;
    public Vector3 Velocity { get; set; }
    public Vector3 Accleration { get; set; }
    public Vector3 Direction { get { return Velocity.normalized; } }

    public abstract void ApplyForce(Vector3 force);
}
