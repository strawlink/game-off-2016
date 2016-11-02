using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TwitchIRC
{
	public string oauth;
	public string nickName = "wmmu";
	public string channelName = "wmmu";
	private string server = "irc.twitch.tv";
	private int port = 6667;
	
	public Action<string> messageReceivedEvent;

	private string buffer = string.Empty;
	private bool stopThreads = false;
	private Queue<string> commandQueue = new Queue<string>();
	private List<string> receivedMsgs = new List<string>();
	private System.Threading.Thread inProc, outProc;
	private void StartIRC()
	{
		System.Net.Sockets.TcpClient sock = new System.Net.Sockets.TcpClient();
		sock.Connect(server, port);
		if (!sock.Connected)
		{
			Debug.Log("Failed to connect!");
			return;
		}
		var networkStream = sock.GetStream();
		var input = new System.IO.StreamReader(networkStream);
		var output = new System.IO.StreamWriter(networkStream);

		//Send PASS & NICK.
		output.WriteLine("PASS " + oauth);
		output.WriteLine("NICK " + nickName.ToLower());
		output.Flush();

		//output proc
		outProc = new System.Threading.Thread(() => IRCOutputProcedure(output));
		outProc.Start();
		//input proc
		inProc = new System.Threading.Thread(() => IRCInputProcedure(input, networkStream));
		inProc.Start();
		Debug.Log("Started Twitch listening");
	}
	private void IRCInputProcedure(System.IO.TextReader input, System.Net.Sockets.NetworkStream networkStream)
	{
		while (!stopThreads)
		{
			if (!networkStream.DataAvailable)
				continue;

			buffer = input.ReadLine();

			//was message?
			if (buffer.Contains("PRIVMSG #"))
			{
				lock (receivedMsgs)
				{
					receivedMsgs.Add(buffer);
				}
			}

			//Send pong reply to any ping messages
			if (buffer.StartsWith("PING "))
			{
				SendCommand(buffer.Replace("PING", "PONG"));
			}

			//After server sends 001 command, we can join a channel
			if (buffer.Split(' ')[1] == "001")
			{
				SendCommand("JOIN #" + channelName);
			}
		}
	}
	private void IRCOutputProcedure(System.IO.TextWriter output)
	{
		System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
		stopWatch.Start();
		while (!stopThreads)
		{
			lock (commandQueue)
			{
				if (commandQueue.Count > 0) //do we have any commands to send?
				{
					// https://github.com/justintv/Twitch-API/blob/master/IRC.md#command--message-limit 
					//have enough time passed since we last sent a message/command?
					if (stopWatch.ElapsedMilliseconds > 1750)
					{
						//send msg.
						output.WriteLine(commandQueue.Peek());
						output.Flush();
						//remove msg from queue.
						commandQueue.Dequeue();
						//restart stopwatch.
						stopWatch.Reset();
						stopWatch.Start();
					}
				}
			}
		}
	}

	public void SendCommand(string cmd)
	{
		lock (commandQueue)
		{
			commandQueue.Enqueue(cmd);
		}
	}
	public void SendMsg(string msg)
	{
		lock (commandQueue)
		{
			commandQueue.Enqueue("PRIVMSG #" + channelName + " :" + msg);
		}
	}
	
	public void Initialize()
	{
		stopThreads = false;
		StartIRC();
	}
	public void OnDisable()
	{
		stopThreads = true;
	}
	public void OnDestroy()
	{
		stopThreads = true;
	}
	public void Update()
	{
		lock (receivedMsgs)
		{
			if (receivedMsgs.Count > 0)
			{
				for (int i = 0; i < receivedMsgs.Count; i++)
				{
					var handler = messageReceivedEvent;
					if(handler != null)
					{
						handler(receivedMsgs[i]);
					}
				}
				receivedMsgs.Clear();
			}
		}
	}
}