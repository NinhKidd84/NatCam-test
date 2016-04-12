using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using NatCamU;
using UnityEngine.SceneManagement;

public class TestNat : MonoBehaviour {
	public GameObject _Menu, _TestUI;
	bool completeSetUp = false;
	//CAMERA STATS
	public int camType = 0;// 0=Front, 1 = Rear
	public int resolType = 0;//0 = Default, 1 = Medium, 2 = Lowest
	public int focusType = 0; //0=Auto, 1= On, 2 = Off
	public int displaySizeType = 0; //0=full, 1=medium, 2=small
	//FOR TESTING
	public Text _FPSDisplay;
	int[] fpsCount = new int[50];
	int fpsIndex = 0;

	void Start () {
		_Menu.gameObject.SetActive (true);
		NatCam.NatCamPreviewStartEvent += OnFirstFrame;
		NatCam.Initialize(PreviewType.Readable);
	}

	public void _PickACamera(int id) {
		camType = id;
	}
	public void _PickResolution(int id) {
		resolType = id;
	}
	public void _PickFocus(int id) {
		focusType = id;
	}
	public void _PickDisplay(int id) {
		displaySizeType = id;
	}

	public void clickStart() {
		if (camType == 0) {
			NatCam.SetActiveCamera (DeviceCamera.FrontCamera);
		}
		else if (camType == 1) {
			NatCam.SetActiveCamera (DeviceCamera.RearCamera);
		}

		if (resolType == 0) {
			NatCam.SetResolution (ResolutionPreset.Default);
		}
		else if (resolType == 1) {
			NatCam.SetResolution (ResolutionPreset.MediumResolution);
		}
		else if (resolType == 2) {
			NatCam.SetResolution (ResolutionPreset.LowestResolution);
		}

		if (focusType == 0) {
			NatCam.FocusMode = FocusMode.AutoFocus;
		}
		else if (focusType == 1) {
			NatCam.FocusMode = FocusMode.HybridFocus;
		}
		else if (focusType == 2) {
			NatCam.FocusMode = FocusMode.Off;
		}

		if (displaySizeType == 0) {
			transform.localScale = new Vector3 (5f, 10f, 1f);
		}
		else if (displaySizeType == 1) {
			transform.localScale = new Vector3 (5f, 6f, 1f);
		}
		else if (displaySizeType == 1) {
			transform.localScale = new Vector3 (5f, 3f, 1f);
		}
		float theHei = transform.localScale.y;
		float theWidth = theHei * Screen.width / Screen.height;
		transform.localScale = new Vector3 (theWidth, theHei, 1f);

		NatCam.Play ();

		GetComponent<Renderer> ().material.mainTexture = NatCam.PreviewTexture;

		completeSetUp = true;
		_Menu.gameObject.SetActive (false);
		_TestUI.gameObject.SetActive (true);
	}

	private void OnFirstFrame() {
		Debug.Log ("On FIRST FRAME");
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			clickReset ();
		}

		if (completeSetUp) {
			if (NatCam.IsPlaying) {
				int currentFPS = Mathf.FloorToInt (1f / Time.unscaledDeltaTime);
				calcuTheFPS (currentFPS);
			}
		}
	}

	private void calcuTheFPS(int lastFPS) {
		if (fpsIndex >= fpsCount.Length) {
			fpsIndex = 0;
		}
		fpsCount [fpsIndex] = lastFPS;
		int total = 0, amount = 0;
		for (int i = 0; i < fpsCount.Length; i++) {
			if (fpsCount [i] > 0) {
				total += fpsCount [i];
				amount++;
			} else {
				break;
			}
		}

		if (amount > 0 && total > 0) {
			int theAverage = Mathf.FloorToInt (total / amount);
			_FPSDisplay.text = "average FPS = " + theAverage.ToString ();
		}
	}

	public void clickReset() {
		NatCam.Stop ();
		Scene csc = SceneManager.GetActiveScene();
		SceneManager.LoadScene( csc.name );
	}
}
