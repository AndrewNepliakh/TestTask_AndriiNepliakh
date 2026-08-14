using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Managers
{
    public interface IGameManager
    {
        GameState GameState { get; }
        public event Action<GameState> OnStateChange;
        void LoadScene(string sceneKey, LoadSceneMode mode);
        public void OnWin();
        public void OnPlay();
        public void OnLose();
    }
}