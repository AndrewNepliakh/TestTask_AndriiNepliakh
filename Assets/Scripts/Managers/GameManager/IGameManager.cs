using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Managers
{
    public interface IGameManager
    {
        public event Action<GameState> OnStateChange;
        void LoadScene(string sceneKey, LoadSceneMode mode);
        public void OnWin();
        public void OnPlay();
        public void OnLose();
    }
}