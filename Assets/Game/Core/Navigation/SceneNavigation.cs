using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace ObsidianProtocol.Game.Core
{
    public sealed class SceneNavigation
    {
        public bool Initialized { get; private set; }

        public bool Initialize()
        {
            if (Initialized)
                return false;

            Initialized = true;
            return true;
        }

        public void LoadScene(string sceneName)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(sceneName))
            {
                return;
            }

            SceneManager.LoadScene(sceneName.Trim());
        }

        public void LoadSceneAsync(
            MonoBehaviour runner,
            string sceneName)
        {
            if (!Initialized ||
                runner == null ||
                string.IsNullOrWhiteSpace(sceneName))
            {
                return;
            }

            runner.StartCoroutine(
                LoadSceneRoutine(sceneName.Trim()));
        }

        private IEnumerator LoadSceneRoutine(
            string sceneName)
        {
            AsyncOperation operation =
                SceneManager.LoadSceneAsync(sceneName);

            while (operation != null &&
                   !operation.isDone)
            {
                yield return null;
            }
        }
    }
}
