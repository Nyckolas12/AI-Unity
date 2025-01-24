using UnityEngine;

public class KinematicMovement : Movement
{
    public override void ApplyForce(Vector3 force)
    {
        Accleration += force;

    }

    private void LateUpdate()
    {
        Velocity += Accleration * Time.deltaTime;
        Velocity = Vector3.ClampMagnitude(Velocity, data.maxSpeed);
        transform.position += Velocity * Time.deltaTime;
        Accleration = Vector3.zero;
    }
}
