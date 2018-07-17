using UnityEngine;

public class RikishiPlayerInputProvider : MonoBehaviour {
    public float DebugPlayerAimRayHeight = 0.75f;
    public float DebugPlayerAimRayLength = 1f;
    Transform mainCameraTransform;
    Transform playerTransform;

    void Start () {
        mainCameraTransform = Camera.main.transform;
        playerTransform = GameObject.FindWithTag("Player").transform;
    }
	
	void Update () {
        //var fromCameraToPlayer = mainCameraTransform.transform.position - playerTransform.position; // this drew a line from the camera to the player
        var fromCameraToPlayerOpposite = playerTransform.position - mainCameraTransform.transform.position; // this drew a line from the player in the opposite direction of the camera :D which is what I want!
        Debug.DrawRay(playerTransform.position, fromCameraToPlayerOpposite, Color.blue);

        var playerDesiredRotation = Vector3.ProjectOnPlane(fromCameraToPlayerOpposite, Vector3.up); // line parallel to ground instead of at an angle
        var angleOfDesiredRotation = Vector3.SignedAngle(playerDesiredRotation, playerTransform.forward, Vector3.up) * -1;
        var debugPlayerAimRay = new Vector3(playerTransform.forward.x, playerTransform.forward.y, playerTransform.forward.z) * DebugPlayerAimRayLength;
            debugPlayerAimRay = Quaternion.AngleAxis(angleOfDesiredRotation, Vector3.up) * debugPlayerAimRay;
        Debug.DrawRay(playerTransform.position, debugPlayerAimRay, Color.green);
	}


    /*
     * 
     *         if (Input.GetKey(KeyCode.RightArrow))
        {
            //Rotate the sprite about the Y axis in the positive direction
            transform.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * m_Speed, Space.World);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //Rotate the sprite about the Y axis in the negative direction
            transform.Rotate(new Vector3(0, -1, 0) * Time.deltaTime * m_Speed, Space.World);
        }
        */
}
