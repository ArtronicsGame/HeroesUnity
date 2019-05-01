using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchCamera : MonoBehaviour
{
    public Transform target;
    public float smoothness = 0.05f;
    public float xError = 8f;
    public float yError = 4.5f;

    public float width;
    public float height;
    

    void Update()
    {
        if (target != null)
        {
            Vector3 delta = target.position - transform.position;

            if (Mathf.Abs(delta.x) > xError ||
                Mathf.Abs(delta.y) > yError)
            {
                var position = target.position;
                float xTarget = transform.position.x, yTarget = transform.position.y;
                
                if(Mathf.Abs(delta.x) > xError)
                    xTarget = delta.x > 0 ? position.x - xError : position.x + xError;
                if(Mathf.Abs(delta.y) > yError)
                    yTarget = delta.y > 0 ? position.y - yError : position.y + yError;
                Vector2 targetPosition = new Vector2(xTarget, yTarget);
                
                Camera cam = Camera.main;
                float camHeight = 2f * cam.orthographicSize;
                float camWidth = camHeight * cam.aspect;

                if (targetPosition.x < camWidth / 2)
                    targetPosition.x = camWidth / 2;
                if (targetPosition.x > width - (camWidth / 2))
                    targetPosition.x = width - (camWidth / 2);
                
                if (targetPosition.y < camHeight / 2)
                    targetPosition.y = camHeight / 2;
                if (targetPosition.y > height - (camHeight / 2))
                    targetPosition.y = height - (camHeight / 2);
                
                Vector3 smoothTarget = Vector2.Lerp(transform.position, targetPosition, smoothness);
                transform.position = smoothTarget + Vector3.back;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        var position = Camera.main.transform.position;
        Gizmos.DrawLine(position + new Vector3(-xError, -yError), position + new Vector3(xError, -yError));
        Gizmos.DrawLine(position + new Vector3(xError, -yError), position + new Vector3(xError, yError));
        Gizmos.DrawLine(position + new Vector3(xError, yError), position + new Vector3(-xError, yError));
        Gizmos.DrawLine(position + new Vector3(-xError, yError), position + new Vector3(-xError, -yError));
    }
}