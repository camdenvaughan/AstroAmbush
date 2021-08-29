using UnityEngine;

public class ShipMovementController : MonoBehaviour {

	public float velocity;
	public float rotationSpeed;
	
	private ShipInputController inputController;
	private Animator anim;

	// Use this for initialization
	void Start () {
		inputController = GetComponent<ShipInputController>();
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	public void Move()
    {
		var inputDirection = new Vector3(inputController.horizontal, inputController.vertical, 0);
		float thrust = Vector3.Dot(inputDirection.normalized, this.transform.up);
		var rotation = Vector3.Dot(inputDirection.normalized, this.transform.right);
		var rotationAmount = rotationSpeed * Time.deltaTime * rotation;
		if (thrust < 0)
		{
			rotationAmount *= 2;
			thrust = .5f;
		}
	    this.transform.position += thrust * inputDirection.magnitude * this.transform.up * velocity * Time.deltaTime;

		this.transform.rotation = Quaternion.Euler(0, 0, this.transform.rotation.eulerAngles.z - rotationAmount);
		anim.SetFloat("rotation", rotation);
    }
}