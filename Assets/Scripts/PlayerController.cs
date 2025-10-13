using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private void Start()
    {
#if UNITY_ANDROID
        Debug.Log("Android platform detected");
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
#endif
    }

}
