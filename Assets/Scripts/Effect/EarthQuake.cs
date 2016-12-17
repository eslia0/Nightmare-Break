using UnityEngine;
using System.Collections;

public class EarthQuake : MonoBehaviour
{
	[UnityEngine.SerializeField, Range(1f, 50f)]
	public float magnitude = 1.9f;//지진의 진도
	[UnityEngine.SerializeField, Range(0f, 100f)]
	public float shakingSpeed = 68;//흔들리는 스피드
	[UnityEngine.SerializeField, Range(0f, 1f)]
	public float randomAmount = 0f;//랜덤 정도
	public float duration = 1;//지속 시간
	public Vector3 forceByAxis =new Vector3(1,0,1);//흔들리는 축
	public AnimationCurve forceOverTime;//진도의 변화 그래프
	public bool forceRecenter = true; //위치 복귀
	public bool loop = false; //루프 사용 유무

	public string currentState; //현재 상태 스트링

	bool running = false;
	public bool Running
	{
		get { return running; }
		set { startStop(value); }
	}

	Vector3 startPosition; //처음 위치
	Vector3 delta;
	Quaternion startRotation; //처음의 회전 값
	[SerializeField]
	float currentMagnitude; //현재 진도
	float timeSinceStarted;

	// Use this for initialization
	void Start ()
	{
		startPosition = transform.position;//처음 위치 설정
		startRotation = transform.rotation;//처음 회전값 설정
		try { gameObject.AddComponent<Rigidbody>(); } catch {};

		GetComponent<Rigidbody>().freezeRotation = true;//회전 값 고정
		GetComponent<Rigidbody>().mass = float.MaxValue; //mass 설정
		GetComponent<Rigidbody>().useGravity = false; //중력 사용 안함
		GetComponent<Rigidbody>().isKinematic = true;
		Running = false;

	}

	public void startStop(bool value)
	{
		if (value)
			currentState = "quake started";
		else{
			currentState = "quake stopped";
		}
		currentMagnitude = 0;
		timeSinceStarted = 0;
		running = value;
	//	GetComponent<Rigidbody>().velocity = Vector3.zero;
		delta = Vector3.zero;
		transform.position = startPosition;
		transform.rotation = startRotation;

	}

	public void OnRunning()
	{
		
		forceByAxis = new Vector3(Mathf.Clamp(forceByAxis.x, 0f, 1f),//흔들리는 축의 값 범위 제한
			                      Mathf.Clamp(forceByAxis.y, 0f, 1f),//흔들리는 축의 값 범위 제한
			                      Mathf.Clamp(forceByAxis.z, 0f, 1f));//흔들리는 축의 값 범위 제한
		timeSinceStarted += Time.deltaTime;
		currentMagnitude = forceOverTime.Evaluate(timeSinceStarted / duration) * magnitude * 15;//그래프에서 현재 진도의 값을 얻어온다.
		if (timeSinceStarted > duration && !loop)
			Running = false;
		if (timeSinceStarted > duration && loop)
			Running = true;
		delta += new Vector3(Time.deltaTime * shakingSpeed * my_rand(),//각 축 마다 흔들리는 값 랜덤으로 가져온다.
			                 Time.deltaTime * shakingSpeed * my_rand(),//각 축 마다 흔들리는 값 랜덤으로 가져온다.
		                     Time.deltaTime * shakingSpeed * my_rand());//각 축 마다 흔들리는 값 랜덤으로 가져온다.
		GetComponent<Rigidbody>().position = new Vector3(Mathf.Cos(delta.x) * Time.deltaTime * currentMagnitude * forceByAxis.x, //진도와 각축의 값을 가져와서 속도를 준다.
			                                             Mathf.Cos(delta.y) * Time.deltaTime * currentMagnitude * forceByAxis.y,//진도와 각축의 값을 가져와서 속도를 준다.
			                                             Mathf.Cos(delta.z) * Time.deltaTime * currentMagnitude * forceByAxis.z);//진도와 각축의 값을 가져와서 속도를 준다.
		currentMagnitude /= 15;

	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		if (running)
			OnRunning();
		if (forceRecenter) {
		}
		GetComponent<Rigidbody>().velocity += (startPosition - transform.position) * Time.deltaTime * 40;
	
	}

	float my_rand()
	{
		return (1f - Random.Range(0, randomAmount));
	}
}
