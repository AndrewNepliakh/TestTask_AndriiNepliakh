using UnityEngine;
using UnityEngine.SceneManagement;

public class AppEnterPoint : MonoBehaviour
{
   private void Awake()
   {
      SceneManager.LoadScene(Constants.MainScene);
   }
}
