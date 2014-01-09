using UnityEngine;
using System.Collections;

public class ConversationManager : MonoBehaviour 
{
	//reference to the player
	private GameObject Player;
	//reference to the players mouth vector
	private Transform PlayerMouthVec;
	//NPC's trust level
	private int Trust = 100;
	//NPC's head position
	public GameObject HeadPos;
	//Can the player chat with this character?
	private bool CanChat = false;
	//Is the NPC currently chatting?
	private bool IsChatting = false;
	//reference to word bubble texture
	public Texture WordBubble;
	//The default size of a word bubble
	private Vector2 WordBubbleSize = new Vector2(200, 150);
	//the offset for the word bubble to allign it round the text
	private Vector3 WordBubbleOffset = new Vector3(-0.03f, 0.03f, 0);
	//The default starting position of text
	private Vector3 NPCTextPosition = new Vector3(1, 2.5f, 0);
	//pause timer between a player response and the NPC text
	private float NPCResponseTime = 0.25f;
	private float NPCResponseTimer = 0;
	
	//The various chat texts of the NPC's
	public string[] NpcChatText;
	//a script that is to be enabled on a particular NPC chat event
	public MonoBehaviour[] ScriptToEnable;
	//The current chat text being spoken by the NPC
	[HideInInspector]public int CurrentNPCChatText = 0;
	
	//the default positioning of the players chat text
	private Vector3 PlayerChatTextPosition = new Vector3(1, 4, 0);
	private Vector3 PlayerChatCircleSize = new Vector3(0, 0.15f, 0);
	private Vector3 PlayerChatCircleAngle = new Vector3(0, 1, 0.4f);
	//pause timer between a NPC response and the Player text
	private float PlayerResponseTime = 0.75f;
	private float PlayerResponseTimer = 0;
	//used to interpoloate player word bubbles, between 0 and 1
	private float WordBubbleInterpolator = 1;
	//the rate at which the word bubbles rotate
	private float WordBubbleInterpolationRate = 1.5f;
	//the direction it should be interpolating
	private int WordBubbleInterpoleDirection;
	//The various things the player can say in response
	public string[] PlayerChatText;
	//What those responses corrispond to
	public int[] PlayerChatResponceTo;
	//what NPC chat those responses lead to
	public int[] PlayerChatLeadsTo;
	//the array values of ALL currently available responses
	private int[] CurrentResponsesAvailable;
	//the players currently selected chat option, in reference to "CurrentlyAvailableChatText"
	private int CurrentPlayerChatText = 0;
	//the array variable of the currently selected chat option, in reference to "PlayerChatText"
	private int CurrentPlayerChatArray = 0;
	//does this response exit the chat
	public bool[] ExitFromChat;
	
	//A reference to the current NPC GUIText object
	private GameObject CurrentNPCGuiText;
	//A reference to the current NPC word bubble texture object
	private GameObject CurrentNPCWordBubble;
	//A reference to all Player GUIText objects
	private GameObject[] CurrentPlayerGUITexts;
	//A reference to all player chat bubble texture objects
	private GameObject[] CurrentPlayerWordBubbles;
	
	// Use this for initialization
	void Start () 
	{
		//find the player
		Player = GameObject.FindGameObjectWithTag("Player");
		//find his neck vector, its near the mouth
		foreach (Transform child in Player.transform)
		{
      		if (child.name == "NeckVector")
			{
				PlayerMouthVec = child.transform;
				break;
			}
		}
		
		CurrentPlayerGUITexts = new GameObject[0];
		CurrentPlayerWordBubbles = new GameObject[0];
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(PlayerResponseTimer > 0)
		{
			PlayerResponseTimer -= Time.deltaTime;	
		}
		
		if(NPCResponseTimer > 0)
		{
			NPCResponseTimer -= Time.deltaTime;	
		}
		
		if(WordBubbleInterpolator < 1f)
		{
			WordBubbleInterpolator += Time.deltaTime * WordBubbleInterpolationRate;
			if(WordBubbleInterpolator > 1f)
				WordBubbleInterpolator = 1;
		}
		
		//start a chat
		if(CanChat && Input.GetKeyDown(KeyCode.E) && !IsChatting && Player.GetComponent<PlayerStateMachine>().MyPlayerState == PlayerStateMachine.PlayerState.None)
		{
			IsChatting = true;
			Player.GetComponent<PlayerStateMachine>().MyPlayerState = PlayerStateMachine.PlayerState.Chatting;
			Player.GetComponent<PlayerStateMachine>().InStateTill = float.PositiveInfinity;
			
			PlayerResponseTimer = PlayerResponseTime;
			NPCResponseTimer = 0;
		}
		//end a chat
		else if(IsChatting && Input.GetKeyDown(KeyCode.G) && Player.GetComponent<PlayerStateMachine>().MyPlayerState == PlayerStateMachine.PlayerState.Chatting)
		{
			IsChatting = false;
			Player.GetComponent<PlayerStateMachine>().MyPlayerState = PlayerStateMachine.PlayerState.None;
		}
		//Select chat player option
		else if(IsChatting && Input.GetKeyDown(KeyCode.E) && Player.GetComponent<PlayerStateMachine>().MyPlayerState == PlayerStateMachine.PlayerState.Chatting && PlayerResponseTimer <= 0 && NPCResponseTimer <= 0)
		{
			//Set the NPC's chat text to whatever the players selected chat text leads to
			CurrentNPCChatText = PlayerChatLeadsTo[CurrentPlayerChatArray];
			
			//clear our the objects so the new chat bubbles re-initialize
			foreach(GameObject loopObj in CurrentPlayerGUITexts)
			{
				Destroy(loopObj);	
			}
			foreach(GameObject loopObj in CurrentPlayerWordBubbles)
			{
				Destroy(loopObj);	
			}
			
			CurrentPlayerGUITexts = new GameObject[0];
			CurrentPlayerWordBubbles = new GameObject[0];
			
			if(ExitFromChat[CurrentPlayerChatArray])
			{
				IsChatting = false;
				Player.GetComponent<PlayerStateMachine>().MyPlayerState = PlayerStateMachine.PlayerState.None;
			}
			
			PlayerResponseTimer = PlayerResponseTime;
			NPCResponseTimer = NPCResponseTime;
		}
		//cycle left through player chat options
		else if(IsChatting && Input.GetKeyDown(KeyCode.A) && Player.GetComponent<PlayerStateMachine>().MyPlayerState == PlayerStateMachine.PlayerState.Chatting && WordBubbleInterpolator == 1 && CurrentResponsesAvailable.Length > 1 && PlayerResponseTimer <= 0 && NPCResponseTimer <= 0)
		{
			CurrentPlayerChatText++;
			WordBubbleInterpolator = 0;
			WordBubbleInterpoleDirection = 1;
		}
		//cycle right through player chat options
		else if(IsChatting && Input.GetKeyDown(KeyCode.D) && Player.GetComponent<PlayerStateMachine>().MyPlayerState == PlayerStateMachine.PlayerState.Chatting && WordBubbleInterpolator == 1 && CurrentResponsesAvailable.Length > 1 && PlayerResponseTimer <= 0 && NPCResponseTimer <= 0)
		{
			CurrentPlayerChatText--;
			WordBubbleInterpolator = 0;
			WordBubbleInterpoleDirection = -1;
		}
		
		if(ScriptToEnable[CurrentNPCChatText] != null && ScriptToEnable[CurrentNPCChatText].enabled == false)
		{
			ScriptToEnable[CurrentNPCChatText].enabled = true;
		}		
	}
	
	void OnGUI()
	{
		if(IsChatting)
		{
			GUIStyle MyGuiStyle = new GUIStyle();
			
			if(NPCResponseTimer <= 0)
			{
				//create a gui text object and word bubble for the NPC
				if(CurrentNPCGuiText == null)
				{
					//spawn text object over NPC
					CurrentNPCGuiText = new GameObject(gameObject.name + " Speach Text");
					CurrentNPCGuiText.AddComponent("GUIText");
					CurrentNPCGuiText.guiText.color = Color.black;
					//spawn speach bubble texture object over NPC
					CurrentNPCWordBubble = new GameObject(gameObject.name + " Word Bubble");
					CurrentNPCWordBubble.AddComponent("GUITexture");
					CurrentNPCWordBubble.guiTexture.texture = WordBubble;
					CurrentNPCWordBubble.transform.position = Vector3.zero;
					CurrentNPCWordBubble.transform.localScale = Vector3.zero;
				}
				
				//draw text inside the word bubble, positioned over the NPC
				CurrentNPCGuiText.transform.position = Player.GetComponentInChildren<Camera>().WorldToViewportPoint(HeadPos.transform.position + NPCTextPosition);
				//CurrentNPCWordBubble.guiTexture.pixelInset = new Rect(Player.GetComponentInChildren<Camera>().WorldToScreenPoint(HeadPos.transform.position).x - (WordBubbleSize.x / 2), Player.GetComponentInChildren<Camera>().WorldToScreenPoint(HeadPos.transform.position).y, WordBubbleSize.x, WordBubbleSize.y);
				Vector3 NPCWordBubblePosition;
				NPCWordBubblePosition = new Vector3((CurrentNPCGuiText.transform.position.x * Screen.width) + (WordBubbleOffset.x * Screen.width), (CurrentNPCGuiText.transform.position.y * Screen.height) + (WordBubbleOffset.y * Screen.height) - WordBubbleSize.y, CurrentNPCGuiText.transform.position.z);
				CurrentNPCWordBubble.guiTexture.pixelInset = new Rect(NPCWordBubblePosition.x, NPCWordBubblePosition.y, WordBubbleSize.x, WordBubbleSize.y);
				
				//say the correct text
				CurrentNPCGuiText.GetComponent<GUIText>().text = NpcChatText[CurrentNPCChatText];
			}
			else
			{
				Destroy(CurrentNPCGuiText);
				Destroy(CurrentNPCWordBubble);
			}
			
			if(PlayerResponseTimer <= 0)
			{
				//search to find all the chat texts that respond to the current NPC chat text
				int NumberOfResponses = 0;//the number of responses relevent to this stage of the conversation
				CurrentResponsesAvailable = new int[0];//a list of the array values deemed relevent for this stage of the conversation
				for(int i = 0; i < PlayerChatText.Length; i++)
				{
					if(PlayerChatResponceTo[i] == CurrentNPCChatText)
					{
						NumberOfResponses++;
						AddIntToArray(ref CurrentResponsesAvailable, i);
					}
				}
				
				//do a check, if the CurrentPlayerChatText is out of bounds, loop it around
				if(CurrentPlayerChatText < 0)
					CurrentPlayerChatText = NumberOfResponses - 1;
				else if(CurrentPlayerChatText > NumberOfResponses - 1)
					CurrentPlayerChatText = 0;
				
				CurrentPlayerChatArray = CurrentResponsesAvailable[CurrentPlayerChatText];
			
				//Make sure the correct number of word bubble texture and GUIText objects are spawned
				if(CurrentPlayerGUITexts.Length != NumberOfResponses)
				{
					//destroy the old objects
					foreach(GameObject loopObj in CurrentPlayerGUITexts)
					{
						Destroy(loopObj);	
					}
					//reset the string size
					CurrentPlayerGUITexts = new GameObject[NumberOfResponses];
					
					//create new objects
					for(int i = 0; i < CurrentPlayerGUITexts.Length; i++)
					{
						CurrentPlayerGUITexts[i] = new GameObject(Player.gameObject.name + " Speach Text");
						CurrentPlayerGUITexts[i].AddComponent("GUIText");
						CurrentPlayerGUITexts[i].guiText.color = Color.black;
					}
				}
				if(CurrentPlayerWordBubbles.Length != NumberOfResponses)
				{
					//destroy the old objects
					foreach(GameObject loopObj in CurrentPlayerWordBubbles)
					{
						Destroy(loopObj);	
					}
					//reset the string size
					CurrentPlayerWordBubbles = new GameObject[NumberOfResponses];
					
					//create new objects
					for(int i = 0; i < CurrentPlayerWordBubbles.Length; i++)
					{
						CurrentPlayerWordBubbles[i] = new GameObject(Player.gameObject.name + " World Bubbles");
						CurrentPlayerWordBubbles[i].AddComponent("GUITexture");
						CurrentPlayerWordBubbles[i].guiTexture.texture = WordBubble;
						CurrentPlayerWordBubbles[i].transform.position = Vector3.zero;
						CurrentPlayerWordBubbles[i].transform.localScale = Vector3.zero;
					}
					
					//initialize the first default chat response
					CurrentPlayerChatText = CurrentResponsesAvailable[0];
				}
				
				//Position, fill, and size all the word bubbles and texts
				//TODO: When changing word bubbles, lerp the position so the word bubbles more clearly appear to rotate
				for(int i = 0; i < CurrentPlayerWordBubbles.Length; i++)
				{				
					GameObject WordBubblePosition = new GameObject("WordBubblePositioner");
					//position text
					WordBubblePosition.transform.position = Player.GetComponentInChildren<Camera>().WorldToViewportPoint(PlayerMouthVec.position  + PlayerChatTextPosition);
					WordBubblePosition.transform.RotateAround(Player.GetComponentInChildren<Camera>().WorldToViewportPoint(PlayerMouthVec.position + PlayerChatTextPosition + PlayerChatCircleSize), PlayerChatCircleAngle.normalized, (i * (360f / NumberOfResponses)) + ((CurrentPlayerChatText + (WordBubbleInterpoleDirection * -1) + WordBubbleInterpolator * WordBubbleInterpoleDirection) * (360f / NumberOfResponses)));
					
					CurrentPlayerGUITexts[i].transform.position = WordBubblePosition.transform.position;
					CurrentPlayerGUITexts[i].guiText.color = Color.black;
					CurrentPlayerGUITexts[i].guiText.text = PlayerChatText[CurrentResponsesAvailable[i]];
					
					//position word bubble
					WordBubblePosition.transform.position = new Vector3((WordBubblePosition.transform.position.x * Screen.width) + (WordBubbleOffset.x * Screen.width), (WordBubblePosition.transform.position.y * Screen.height) + (WordBubbleOffset.y * Screen.height) - WordBubbleSize.y, WordBubblePosition.transform.position.z);
					float ScaleValue = Mathf.Abs(0 - WordBubblePosition.transform.position.z) / 180f; //a value between 0 and 1 that lets me know how what position something is at % wise.  0 = front, 1 = back
					
					CurrentPlayerWordBubbles[i].transform.position = new Vector3(0, 0, WordBubblePosition.transform.position.z);
					CurrentPlayerWordBubbles[i].guiTexture.pixelInset = new Rect(WordBubblePosition.transform.position.x, WordBubblePosition.transform.position.y, WordBubbleSize.x, WordBubbleSize.y);	
					
					//clean up the transformer so we dont fill the scene with dead transforms
					GameObject.Destroy(WordBubblePosition.gameObject);
				}
			}
		}
		else
		{
			//destroy unused gui text object
			if(CurrentNPCGuiText != null)
			{
				Destroy(CurrentNPCGuiText);
				Destroy(CurrentNPCWordBubble);
				foreach(GameObject loopObj in CurrentPlayerGUITexts)
				{
					Destroy(loopObj);	
				}
				foreach(GameObject loopObj in CurrentPlayerWordBubbles)
				{
					Destroy(loopObj);	
				}
				CurrentPlayerGUITexts = new GameObject[0];
				CurrentPlayerWordBubbles = new GameObject[0];
			}
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			CanChat = true;
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			CanChat = false;
			IsChatting = false;
		}
	}
	
	public virtual void AddIntToArray(ref int[] MyArray, int i)
	{
		int[] TempArray = new int[MyArray.Length + 1];
		
		for(int j = 0; j < MyArray.Length; j++)
		{
			TempArray[j] = MyArray[j];	
		}
		
		TempArray[MyArray.Length] = i;		
		
		MyArray = TempArray;
	}
}
