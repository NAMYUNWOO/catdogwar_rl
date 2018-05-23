using System.Collections.Concurrent;
using System.Threading;
using NetMQ;
using UnityEngine;
using NetMQ.Sockets;

public class NetMqListener
{
    private readonly Thread _listenerWorker;

    private bool _listenerCancelled;

    public delegate void MessageDelegate(string message);

    private readonly MessageDelegate _messageDelegate;

    private readonly ConcurrentQueue<string> _messageQueue = new ConcurrentQueue<string>();

    private void ListenerWork()
    {
        AsyncIO.ForceDotNet.Force();
        using (var subSocket = new SubscriberSocket())
        {
            subSocket.Options.ReceiveHighWatermark = 1000;
            subSocket.Connect("tcp://localhost:12345");
            subSocket.Subscribe("");
            while (!_listenerCancelled)
            {
                string frameString;
                if (!subSocket.TryReceiveFrameString(out frameString)) continue;
                //Debug.Log(frameString);
                _messageQueue.Enqueue(frameString);
            }
            subSocket.Close();
        }
        NetMQConfig.Cleanup();
    }

    public void Update()
    {
        while (!_messageQueue.IsEmpty)
        {
            string message;
            if (_messageQueue.TryDequeue(out message))
            {
                _messageDelegate(message);
            }
            else
            {
                break;
            }
        }
    }

    public NetMqListener(MessageDelegate messageDelegate)
    {
        _messageDelegate = messageDelegate;
        _listenerWorker = new Thread(ListenerWork);
    }

    public void Start()
    {
        _listenerCancelled = false;
        _listenerWorker.Start();
    }

    public void Stop()
    {
        _listenerCancelled = true;
        _listenerWorker.Join();
    }
}

public class ClientObject : MonoBehaviour
{
    private NetMqListener _netMqListener;

    private void HandleMessage(string message)
    {
		print (message);
        var splittedStrings = message.Split(' ');
        if (splittedStrings.Length != 2) return;
		Time.timeScale = 1.0f;
		var dogAction = int.Parse(splittedStrings[0]);
		var catAction = int.Parse(splittedStrings[1]);
		transform.GetComponent<MyGameManager>().Action(dogAction,catAction);
		/*
		var dog_position = GameObject.FindWithTag ("Player").transform.position;
		var cat_position = GameObject.FindWithTag ("Enemy").transform.position;
		print ("postition1 : "+dog_position.ToString() + " and "+cat_position.ToString());
		*/
		 

    }

    private void Start()
    {
        _netMqListener = new NetMqListener(HandleMessage);
        _netMqListener.Start();
    }

    private void Update()
	{	
        _netMqListener.Update();
    }

    private void OnDestroy()
    {
        _netMqListener.Stop();
    }
}
