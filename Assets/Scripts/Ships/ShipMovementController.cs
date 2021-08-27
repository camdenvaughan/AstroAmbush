
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShipMovementType {
	Slide,
	ManualRotation,
	AdaptiveRotation
}

public class ShipMovementController : MonoBehaviour {

	public float velocity;
	public float rotationSpeed;
	public ShipMovementType movementType;

	private ShipInputController inputController;
	private Animator anim;

	// Use this for initialization
	void Start () {
		inputController = GetComponent<ShipInputController>();
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		switch(movementType) {
			case ShipMovementType.ManualRotation:
				UpdateMoveManualRotation();
				return;
			case ShipMovementType.AdaptiveRotation:
				UpdateMoveAdaptiveRotation();
				return;
			case ShipMovementType.Slide:
			default:
				UpdateMoveSlide();
				return;
		}
	}

    private void UpdateMoveManualRotation()
    {
        this.transform.position += inputController.vertical * this.transform.up * velocity * Time.deltaTime;
		float rotationAmount = rotationSpeed * Time.deltaTime * inputController.horizontal;
		this.transform.rotation = Quaternion.Euler(0, 0, this.transform.rotation.eulerAngles.z - rotationAmount);
    }

    private void UpdateMoveAdaptiveRotation()
    {// Lots of questions
		var inputDirection = new Vector3(inputController.horizontal, inputController.vertical, 0);
		float thrust = Vector3.Dot(inputDirection.normalized, this.transform.up);
		var rotation = Vector3.Dot(inputDirection.normalized, this.transform.right);
		this.transform.position += thrust * inputDirection.magnitude * this.transform.up * velocity * Time.deltaTime;
		var rotationAmount = rotationSpeed * Time.deltaTime * rotation;
		this.transform.rotation = Quaternion.Euler(0, 0, this.transform.rotation.eulerAngles.z - rotationAmount);
		anim.SetFloat("rotation", rotation);
    }

    private void UpdateMoveSlide()
    {
		this.transform.position += inputController.horizontal * Vector3.right * velocity * Time.deltaTime;
		this.transform.position += inputController.vertical * Vector3.up * velocity * Time.deltaTime;
    }
}