using UnityEngine;
using UnityEngine.UIElements;

public class AutonomousAgent : AIAgent
{
    [Header("Wander")]
  public AutonomousAgentData AgentData;

    [Header("Perception")]
    public Perception seekPerception;
    public Perception fleePerception;
    public Perception flockPerception;

    float angle;

    private void Update()
    {
        //movement.ApplyForce(Vector3.forward * 10);
        //transform.position = Utilities.Wrap(transform.position, new Vector3(-5, -5, -5), new Vector3(5, 5, 5));


        //Debug.DrawRay(transform.position, transform.forward * perception.maxDistance, Color.yellow);
        //Seek
        if(seekPerception != null )
        {

            var gameObjects = seekPerception.GetGameObjects();
            if(gameObjects.Length > 0)
            {
            Vector3 force = Seek(seekPerception.GetGameObjects()[0]);
            movement.ApplyForce(force);

            }
        }
        

        //Flee
        if( fleePerception != null )
        {
        var gameObjects = fleePerception.GetGameObjects();
        if (gameObjects.Length > 0)
        {
            Vector3 force = Flee(gameObjects[0]);
            movement.ApplyForce(force);
        }

        }

        //Flock
        if (flockPerception != null)
        {
            var gameObjects = flockPerception.GetGameObjects();
            if (gameObjects.Length > 0)
            {
               // movement.ApplyForce(Cohesion(gameObjects));
                //movement.ApplyForce(Separation(gameObjects, AgentData.radius));
                movement.ApplyForce(Alignmentn(gameObjects));
                
            }

        }

        if (movement.Accleration.sqrMagnitude ==0)
        {
            Vector3 force = Wander();
            movement.ApplyForce(force);
        }


        if (movement.Direction.sqrMagnitude != 0)
        {
            Vector3 acceleraion = movement.Accleration;
        acceleraion.y = 0;
        movement.Accleration = acceleraion;

        transform.rotation = Quaternion.LookRotation(movement.Direction);
        float size = 25;
        transform.position = Utilities.Wrap(transform.position,new Vector3(-size,-size,-size), new Vector3(size,size,size));

        }
    }

    private Vector3 Cohesion(GameObject[] neighbors)
    {
        Vector3 position = Vector3.zero;
        foreach (var neighbor in neighbors)
        {
            position += neighbor.transform.position;
        }

        Vector3 center = position / neighbors.Length;
        Vector3 direction = center - transform.position;
        Vector3 force = GetSteeringForce(direction);

        return force;
    }

    private Vector3 Separation(GameObject[] neighbors, float radius)
    {
        Vector3 spearation = Vector3.zero;
        foreach (var neighbor in neighbors)
        {
            Vector3 direction = transform.position - neighbor.transform.position;
            float distance = direction.sqrMagnitude;

            if (distance < radius && distance > 0)
            {
                spearation += direction / (distance * distance);
            }

        }
        Vector3 force = GetSteeringForce(spearation);
        return force;
    }
    private Vector3 Alignmentn(GameObject[] neighbors)
    {
        Vector3 velocities = Vector3.zero;
        foreach(var neighbor in neighbors)
        {
            Rigidbody rb = neighbor.GetComponent<Rigidbody>();
            if(rb != null)
            {
                velocities += rb.angularVelocity;
            }


        }
        Vector3 averageVelocity = velocities / neighbors.Length;
        Vector3 force = GetSteeringForce(averageVelocity);
        return force;
    }
    

    private Vector3 Wander()
    {
        angle += Random.Range(-AgentData.displacement, AgentData.displacement);
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
        Vector3 point = rotation * (Vector3.forward * AgentData.radius);
        Vector3 forward = movement.Direction * AgentData.distance;
        Vector3 force = GetSteeringForce(forward + point);
        return force;
    }

    private Vector3 Seek(GameObject go)
    {
        Vector3 direction = go.transform.position - transform.position;
        Vector3 force = GetSteeringForce(direction);
        return force;
    }

    private Vector3 Flee(GameObject go)
    {
        Vector3 direction =  transform.position - go.transform.position;
        Vector3 force = GetSteeringForce(direction);
        return force;
    }

    private Vector3 GetSteeringForce(Vector3 direction)
    {
        Vector3 desired = direction.normalized * movement.data.maxSpeed;
        Vector3 steer = desired- movement.Velocity;
        Vector3 force = Vector3.ClampMagnitude(steer, movement.data.maxForce);

        return force;
    }
}
