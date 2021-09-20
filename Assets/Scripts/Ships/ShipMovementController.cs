using UnityEngine;

public class ShipMovementController : MonoBehaviour {

	public float velocity;
	public float rotationSpeed;
	
	private Animator anim;
	public float debugFloat;

	// Use this for initialization
	void Start () 
	{
		anim = GetComponent<Animator>();
	}

	public void Move(float horizontal, float vertical)
    {
		var inputDirection = new Vector3(horizontal, vertical, 0);

		var rotation = Vector3.Dot(inputDirection.normalized, this.transform.right);
		var rotationAmount = rotationSpeed * Time.deltaTime * rotation;

		transform.position += transform.up * velocity * Time.deltaTime;

		debugFloat = inputDirection.magnitude;
	    transform.rotation = Quaternion.Euler(0, 0, this.transform.rotation.eulerAngles.z - rotationAmount);
	    anim.SetFloat("rotation", rotation);

    }
}