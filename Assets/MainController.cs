using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MainController : MonoBehaviour
{
	public enum ControlState
	{
		CameraMove,
		HorizontalPlace,
		VerticalPlace,
		NumStates
	}
	[SerializeField] ControlState state = ControlState.CameraMove;
	Vector2 mouseDelta;
	bool LMouseDown;

	[SerializeField] GameObject HeldPlatform;
	MeshRenderer platformmesh;

	[SerializeField] float LookSensitivity;
	[SerializeField] Vector2 MoveConstraint;
	[SerializeField] Vector2 RandRange;

	List<GameObject> clonePlatforms;

	private void Start()
	{
		platformmesh = HeldPlatform.GetComponent<MeshRenderer>();
		clonePlatforms = new List<GameObject>();
	}

	private void Update()
	{
		if (LMouseDown)
		{
			Vector3 LD = Vector3.back * mouseDelta.x * LookSensitivity * Time.deltaTime;
			transform.localPosition += LD;
			transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Mathf.Clamp(transform.localPosition.z, MoveConstraint.x, MoveConstraint.y));
		}
		/*
		if (state == ControlState.Rotating && LMouseDown)
		{
			Vector2 LD = mouseDelta * LookSensitivity * Time.deltaTime;
			transform.Rotate(Vector3.up * LD.x);
		}
		*/
		else if (state == ControlState.HorizontalPlace)
		{
			Plane plane = new Plane(Vector3.up, new Vector3(0, HeldPlatform.transform.position.y, 0));
			Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
			float distance;

			if (plane.Raycast(ray, out distance))
			{
				HeldPlatform.transform.position = ray.GetPoint(distance);
			}

		}
		else if (state == ControlState.VerticalPlace)
		{
			Plane plane = new Plane(Vector3.right, new Vector3(HeldPlatform.transform.position.x, 0, 0));
			Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
			float distance;

			if (plane.Raycast(ray, out distance))
			{
				HeldPlatform.transform.position = ray.GetPoint(distance);
			}
		}
	}
	public void OnPlace(InputAction.CallbackContext context)
	{
		if (context.performed && state != ControlState.CameraMove && !EventSystem.current.IsPointerOverGameObject())
		{
			GameObject placed = Instantiate(HeldPlatform);
			MeshRenderer mesh = placed.GetComponent<MeshRenderer>();
			mesh.material.color = new Color(mesh.material.color.r, mesh.material.color.g, mesh.material.color.b, 1);
			placed.GetComponent<BoxCollider>().isTrigger = false;
			clonePlatforms.Add(placed);

			HeldPlatform.transform.eulerAngles = new Vector3(Random.Range(RandRange.x, RandRange.y), Random.Range(RandRange.x, RandRange.y), Random.Range(RandRange.x, RandRange.y));
		}
	}
	public void OnChange(InputAction.CallbackContext context)
	{
		LMouseDown = context.performed;
		/*if (context.performed)
		{

			state = (ControlState)((int)(state + 1) % (int)ControlState.NumStates);
			if (state == ControlState.CameraMove)
				platformmesh.enabled = false;
			else
				platformmesh.enabled = true;
		}*/
	}
	public void OnRotate(InputAction.CallbackContext context)
	{
		if (context.ReadValue<float>() == 0)
			HeldPlatform.transform.eulerAngles = new Vector3(Random.Range(RandRange.x, RandRange.y), 
				Random.Range(RandRange.x, RandRange.y), 
				Random.Range(RandRange.x, RandRange.y));
	}
	public void OnMove(InputAction.CallbackContext context)
	{
		mouseDelta = context.ReadValue<Vector2>();
	}


	public void ClearClones()
	{
		foreach (GameObject clone in clonePlatforms)
		{ 
			Destroy(clone);
		}
		clonePlatforms.Clear();
	}

}
