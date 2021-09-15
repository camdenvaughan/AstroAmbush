using UnityEngine;

public class ShipMovementController : MonoBehaviour {

	public float velocity;
	public float rotationSpeed;
	
	private Animator anim;

	// Use this for initialization
	void Start () 
	{
		anim = GetComponent<Animator>();
	}

	public void Move(float horizontal, float vertical, bool rotate)
    {
		var inputDirection = new Vector3(horizontal, vertical, 0);
		float thrust = Vector3.Dot(inputDirection.normalized, this.transform.up);
		var rotation = Vector3.Dot(inputDirection.normalized, this.transform.right);
		var rotationAmount = rotationSpeed * Time.deltaTime * rotation;
		if (thrust < 0)
		{
			rotationAmount *= 2;
			thrust = .5f;
		}
		transform.position += thrust * inputDirection.magnitude * this.transform.up * velocity * Time.deltaTime;

		if (rotate)
			transform.rotation = Quaternion.Euler(0, 0, this.transform.rotation.eulerAngles.z - rotationAmount);

	    anim.SetFloat("rotation", rotation);
    }
}