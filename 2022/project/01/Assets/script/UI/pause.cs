using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pause : MonoBehaviour
{
    Canvas pause_canvas;
    void pause_Game()
    {
            Time.timeScale = 0;
        pause_canvas.gameObject.SetActive(true);
    }
    void resume_game()
    {
        Time.timeScale = 1;
        pause_canvas.gameObject.SetActive(false);
    }
}
