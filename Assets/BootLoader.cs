using UnityEngine;
using UnityEngine.SceneManagement;

public class BootLoader : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("SYS04_SimulationCore", LoadSceneMode.Additive);
    }
}
