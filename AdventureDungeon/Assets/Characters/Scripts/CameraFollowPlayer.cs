using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public PlayerCharacterController plr;
    public float followDelay=0.3f;

    private struct PointInSpace
    {
        public Vector3 Position ;
        public float Time ;
    }
    
    [SerializeField]
    [Tooltip("The transform to follow")]
    private Transform target;
    
    [SerializeField]
    [Tooltip("The offset between the target and the camera")]
    private Vector3 offset;
    
    [Tooltip("The delay before the camera starts to follow the target")]
    [SerializeField]
    private float delay = 0.3f;
    
    [SerializeField]
    [Tooltip("The speed used in the lerp function when the camera follows the target")]
    private float speed = 0;

    private Vector3 velocity = Vector3.zero;

    ///<summary>
    /// Contains the positions of the target for the last X seconds
    ///</summary>
    private Queue<PointInSpace> pointsInSpace = new Queue<PointInSpace>();

    void LateUpdate ()
    {
        // Add the current target position to the list of positions
        pointsInSpace.Enqueue( new PointInSpace() { Position = target.position+new Vector3(0,0,-10), Time = Time.time } ) ;
        
        // Move the camera to the position of the target X seconds ago 
        while( pointsInSpace.Count > 0 && pointsInSpace.Peek().Time <= Time.time - delay + Mathf.Epsilon )
        {
            transform.position = Vector3.SmoothDamp( transform.position, pointsInSpace.Dequeue().Position + offset, ref velocity, speed /*Time.deltaTime * speed*/);
            // transform.position = Vector3.SmoothDamp( transform.position, pointsInSpace.Dequeue().Position + offset, ref velocity,1/*Time.deltaTime * speed*/);
        }
    }
}
