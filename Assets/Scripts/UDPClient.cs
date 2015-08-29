using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Linq;


public class UDPClient : MonoBehaviour {
	public Rigidbody obj;
	public int port;
	UdpClient client;
	Thread receiveThread;
	string[] texts;
	void Start () 
	{
		obj = GetComponent<Rigidbody> ();
		ThreadInit ();
	}
	void Update()
	{
		obj.AddForce(new Vector3(float.Parse(texts[6]),float.Parse(texts[7]),float.Parse(texts[8])) * obj.mass * 10f);
	}
	void ThreadInit(){
		receiveThread = new Thread (new ThreadStart(ReceiveData));
		receiveThread.IsBackground = true;
		receiveThread.Start();
	}
	void Abort(){
		client.Close ();
		receiveThread.Abort ();
	}
	// Update is called once per frame

	void ReceiveData(){
		client = new UdpClient (port);
		while (true) 
		{
			try
			{
				IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
				byte[] data = client.Receive(ref anyIP);
				string text = Encoding.UTF8.GetString(data);
				string[] delimiters = {", ", ","};
				texts = text.Split(delimiters,StringSplitOptions.None);
			}
			catch(Exception e)
			{
				Debug.LogError (e.ToString());
			}
		}
	}
}
