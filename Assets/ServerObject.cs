using System.Diagnostics;
using System.Threading;
using NetMQ;
using NetMQ.Sockets;
using UnityEngine;
using System.Collections;
using System;

public class NetMqPublisher
{
    private readonly Thread _listenerWorker;

    private bool _listenerCancelled;

    public delegate string MessageDelegate(string message);

    private readonly MessageDelegate _messageDelegate;

    private readonly Stopwatch _contactWatch;

    private const long ContactThreshold = 1000;

    public bool Connected;

    private void ListenerWork()
    {
        AsyncIO.ForceDotNet.Force();
        using (var server = new ResponseSocket())
        {
            server.Bind("tcp://*:12346");

            while (!_listenerCancelled)
            {
				Connected = _contactWatch.ElapsedMilliseconds < ContactThreshold;
                string message;
                if (!server.TryReceiveFrameString(out message)) continue;
                _contactWatch.Restart();
                var response = _messageDelegate(message);
				server.SendFrame (response);
            }
        }
        NetMQConfig.Cleanup();
    }

    public NetMqPublisher(MessageDelegate messageDelegate)
    {
        _messageDelegate = messageDelegate;
        _contactWatch = new Stopwatch();
        _contactWatch.Start();
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

public class ServerObject : MonoBehaviour
{
    public bool Connected;
    private NetMqPublisher _netMqPublisher;
    private string _response;
	public string msg = "";
	public bool newReq = false;
	public float TIMESCALE;
    public int frameNum = 0;
    private void Start()
    {
		TIMESCALE = transform.GetComponent<MyGameManager> ().TIMESCALE;
        _netMqPublisher = new NetMqPublisher(HandleMessage);
        _netMqPublisher.Start();
    }

    private void Update()
    {	

		if (msg == "req_skip") {
			Time.timeScale = TIMESCALE;
			_response = "res_skip";
			Stopwatch s = new Stopwatch ();
			s.Start ();
			while (s.Elapsed < TimeSpan.FromSeconds(0.0001f)) {
			}
			s.Stop ();
			Connected = _netMqPublisher.Connected;
			return;
		}


		if (! newReq) {  // After taking Action stop animation
			Time.timeScale = 0.0f;
		}
		if (msg != "" && newReq) { // if there is new request(Action) go process 
			string[] splittedStrings = msg.Split (' ');
			if (splittedStrings.Length == 2) {
				var catAction = int.Parse (splittedStrings [0]);
				var dogAction = int.Parse (splittedStrings [1]);
				transform.GetComponent<MyGameManager> ().Action (catAction, dogAction);
				Time.timeScale = TIMESCALE;
				newReq = false; // after take action waiting new message (action)

			}
		}


		GameObject[] coinObjs = GameObject.FindGameObjectsWithTag ("TargetCoin");
		GameObject[] skullObjs = GameObject.FindGameObjectsWithTag ("TargetSkull");
		if (coinObjs.Length < 1 || skullObjs.Length < 1) {
			return;
		}
        var dog_position = GameObject.FindWithTag("Player").transform.position;
        var cat_position = GameObject.FindWithTag("Enemy").transform.position;
        MyGameManager mgm = GameObject.Find("GameManager").GetComponent<MyGameManager>();


        float catR = mgm.catReward;
        float dogR = mgm.dogReward;
        //float x0 = cat_position.x;
        float x1 = cat_position.y;
        float x2 = coinObjs[0].transform.position.x;
        float x3 = coinObjs[0].transform.position.y;
        float x4 = coinObjs[0].GetComponent<Rigidbody2D>().velocity.x;
        float x5 = coinObjs[0].GetComponent<Rigidbody2D>().velocity.y;
        float x6 = skullObjs[0].transform.position.x;
        float x7 = skullObjs[0].transform.position.y;
        float x8 = skullObjs[0].GetComponent<Rigidbody2D>().velocity.x;
        float x9 = skullObjs[0].GetComponent<Rigidbody2D>().velocity.y;
        float x11 = dog_position.y;
        int x12 = (int)mgm.GAMETIME;
        if (mgm.scoreAddVec_coin != new Vector2(0, 0))
        {
            x2 = mgm.scoreAddVec_coin.x;
            x3 = mgm.scoreAddVec_coin.y;
            x4 = 0.0f;
            x5 = 0.0f;
            catR = mgm.catReward_p;
            dogR = mgm.dogReward_p;

        }
        if (mgm.scoreAddVec_skul != new Vector2(0, 0))
        {
            x6 = mgm.scoreAddVec_skul.x;
            x7 = mgm.scoreAddVec_skul.y;
            x8 = 0.0f;
            x9 = 0.0f;
            catR = mgm.catReward_p;
            dogR = mgm.dogReward_p;
        }
        //float x10 = dog_position.x;

        _response = $"{frameNum} {catR} {dogR} {x1} {x2} {x3} {x4} {x5} {x6} {x7} {x8} {x9} {x11} {x12}";
        /*
        float catR = mgm.catReward;
        float dogR = mgm.dogReward;
        float x0 = cat_position.y;
		float x1 = coinObjs [0].transform.position.x;
		float x2 = coinObjs [0].transform.position.y;
		float x3 = coinObjs [0].GetComponent<Rigidbody2D> ().velocity.x;
		float x4 = coinObjs [0].GetComponent<Rigidbody2D> ().velocity.y;
		float x5 = skullObjs [0].transform.position.x;
		float x6 = skullObjs [0].transform.position.y;
		float x7 = skullObjs [0].GetComponent<Rigidbody2D> ().velocity.x;
		float x8 = skullObjs [0].GetComponent<Rigidbody2D> ().velocity.y;
		float x9 = dog_position.y;
		
		_response = $"{frameNum} {catR} {dogR} {x0} {x1} {x2} {x3} {x4} {x5} {x6} {x7} {x8} {x9}";
        */
        frameNum++;
        mgm.catReward = 0.0f;
        mgm.dogReward = 0.0f;
        mgm.catReward_p = 0.0f;
        mgm.dogReward_p = 0.0f;
        mgm.scoreAddVec_coin = new Vector2(0, 0);
        mgm.scoreAddVec_skul = new Vector2(0, 0);
        Connected = _netMqPublisher.Connected;


	
    }
		

    private string HandleMessage(string message)
    {
		/* type of messages 
		 * 1. skip
		 * 2. action ex) "2 4"  
		 * 
		*/
		msg = message;   // 1. getMessage from client, let process go.
		print (message);
		newReq = true;

        return _response;

    }

    private void OnDestroy()
    {
        _netMqPublisher.Stop();
    }
}
