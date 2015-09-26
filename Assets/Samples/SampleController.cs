using UnityEngine;
using System.Collections;
using Leap;

public class SampleController : MonoBehaviour {

	#region Variables
	public GameObject m_Object;
	
	public float m_ObjectScale = 0.2f;
	
	private GameObject m_ObjectInsntance;

	private Controller m_Controller; 
	
	private long m_PrevFrameId;
	#endregion
	
	
	//////////////////////////////////////////////////
	
	
	// Use this for initialization
	void Start () {
		m_Controller = new Controller();
		
		m_ObjectInsntance = Instantiate(m_Object, Vector3.zero, Quaternion.identity) as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (m_Controller == null) {
			return;
		}
		
		// Get a frame
		Frame frame = GetFrame();
		if (frame == null) {
			return;
		}
		
		if (frame.Id == m_PrevFrameId) {
			return;
		}
		m_PrevFrameId = frame.Id;
		
		// Get a hand
		Hand hand = frame.Hands[0];
		if (hand == null) {
			return;
		}
		
		Vector3 normal   = GetPalmNormal(frame);
		Vector3 position = GetPalmPosition(frame);
		
		position += normal * m_ObjectScale;
		
		m_ObjectInsntance.transform.position = position;
		
		// Scale an object with grab-strength
		float s = m_ObjectScale * hand.GrabStrength;
		Vector3 scale = new Vector3(s, s, s);
		
		m_ObjectInsntance.transform.localScale = scale;
	}
	
	
	/**
	 *  Get a palm normal from the controller
	 */
	Vector3 GetPalmNormal(Frame frame) {
		if (m_Controller == null) {
			return Vector3.down;
		}
		
		Hand hand = frame.Hands[0];
		
		Vector3 normal = transform.TransformDirection(hand.PalmNormal.ToUnity(false));
		
		return normal;
	}
	
	
	/**
	 *  Get a palm position from the controller
	 */
	Vector3 GetPalmPosition(Frame frame) {
		if (m_Controller == null) {
			return Vector3.zero;
		}
		
		Hand hand = frame.Hands[0];
		
		Vector3 position = transform.TransformPoint(hand.PalmPosition.ToUnityScaled(false));
		
		return position;
	}
	
	/**
	 *  Get a frame from the controller
	 */
	Frame GetFrame() {
		return m_Controller.Frame();
	}
}
