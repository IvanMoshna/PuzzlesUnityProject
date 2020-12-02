using UnityEngine;
using UnityEngine.SceneManagement;

public class MakeScreenshotByMouseClick : MonoBehaviour
{
	public Camera mainCamera;

	int counter = 1;

	private void Update()
	{
		if (Input.GetMouseButtonDown(1))
		{
			
			ScreenCapture.CaptureScreenshot("Assets/Screenshots/Screenshot" + counter.ToString("00") + "_" + mainCamera.pixelWidth + "x" + mainCamera.pixelHeight + "_" + "_SceneID"+ SceneManager.GetActiveScene().name + ".png");
			counter++;
			Debug.Log("Screenshot " + counter);			
		}
	}
}
