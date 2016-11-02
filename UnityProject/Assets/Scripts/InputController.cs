using UnityEngine;

public class InputController : MonoBehaviour
{
	public static InputController Instance { get; private set; }

	private void Awake()
	{
		Instance = this;
	}
}