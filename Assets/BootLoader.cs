using UnityEngine;
using UnityEngine.SceneManagement;

public class BootLoader : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("SYS‑04_SimulationCore", LoadSceneMode.Additive);
    }
}
