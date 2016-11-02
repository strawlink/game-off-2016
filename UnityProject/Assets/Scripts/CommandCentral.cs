using System.Collections.Generic;
using UnityEngine;

public class CommandCentral : MonoBehaviour
{
	public static CommandCentral Instance { get; private set; }
	private void Awake()
	{
		Instance = this;

		TwitchIRCController.OnMessageReceived += ParseCommandType;
	}

	private enum CommandType { TestCommandOne, TestCommandTwo, TestCommandSpawnObject }

	private Dictionary<string, CommandType> _commandDict = new Dictionary<string, CommandType>
	{
		{ "test1", CommandType.TestCommandOne },
		{ "test2", CommandType.TestCommandTwo },
		{ "testspawn", CommandType.TestCommandSpawnObject },
		
	};

	private void ParseCommandType(string user, string message)
	{
		CommandType cmdType;
		if(_commandDict.TryGetValue(message, out cmdType))
		{
			HandleCommand(cmdType);
		}
		else
		{
			Debug.Log("Unknown command: " + message);
		}
	}

	private void HandleCommand(CommandType command)
	{
		GameObject go;
		switch (command)
		{
			case CommandType.TestCommandOne:
				go = GameObject.CreatePrimitive(PrimitiveType.Cube);

				go.transform.position = Random.onUnitSphere - Vector3.up * 5;
				break;
			case CommandType.TestCommandTwo:
				go = GameObject.CreatePrimitive(PrimitiveType.Sphere);

				go.transform.position = Random.onUnitSphere + Vector3.up * 5;

				break;
			case CommandType.TestCommandSpawnObject:
				go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
				go.transform.position = Random.onUnitSphere;
				break;
			default:
				break;
		}
	}
}