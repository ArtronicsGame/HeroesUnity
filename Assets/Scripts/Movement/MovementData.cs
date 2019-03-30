using UnityEngine;

[CreateAssetMenu(fileName = "MovementData")]
public class MovementData : ScriptableObject
{
    public Vector2 direction;
    public float speed;
}