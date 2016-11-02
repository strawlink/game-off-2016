using System;
using System.Collections;
using UnityEngine;

public class TwitchIRCController : MonoBehaviour
{
	public static event Action<string, string> OnMessageReceived; // username, message


	private TwitchIRC _twitch;

	private void Awake()
	{
		_twitch = new TwitchIRC();
		_twitch.oauth = TwitchSensitiveInformation.GetOauth();
		_twitch.messageReceivedEvent += RawMessageReceived;
		_twitch.Initialize();
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
		_twitch.OnDisable();

	}

	private void OnDestroy()
	{
		_twitch.OnDestroy();
	}

	private void Update()
	{
		_twitch.Update();
	}

	private void RawMessageReceived(string msg)
	{
		Debug.Log("RawMessage: " + msg);
		FilterMessage(msg);
	}

	private void FilterMessage(string rawMessage)
	{
		int msgIndex = rawMessage.IndexOf("PRIVMSG #");
		string msgString = rawMessage.Substring(msgIndex + _twitch.channelName.Length + 11);
		string user = rawMessage.Substring(1, rawMessage.IndexOf('!') - 1);

		var handler = OnMessageReceived;
		if(handler != null)
		{
			handler(user, msgString);
		}
	}

}