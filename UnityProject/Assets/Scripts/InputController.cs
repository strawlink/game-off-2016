using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
	public static InputController Instance { get; private set; }

	private void Awake()
	{
		Instance = this;
		
	}

	void Update()
	{

	}
}