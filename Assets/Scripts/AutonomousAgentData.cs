using UnityEngine;

[CreateAssetMenu(fileName = "AutonomousAgentData", menuName = "Data/AutonomousAgentData")]
public class AutonomousAgentData : ScriptableObject
{
    [Range(1,20)]   public  float distance;
    [Range(1,20)] public float displacement;
    [Range(1,20)] public float radius;
}
