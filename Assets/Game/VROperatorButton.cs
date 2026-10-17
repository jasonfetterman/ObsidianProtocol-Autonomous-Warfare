using UnityEngine;
using UnityEngine.SceneManagement;

public class VROperatorButton : MonoBehaviour
{
    public void OpenVR()
    {
        SceneManager.LoadScene("VR_Command");
    }
}
