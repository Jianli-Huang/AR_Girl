using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPerformance : MonoBehaviour {

    private void OnEnable()
    {
        SceneManager.LoadScene(0);
    }
}
