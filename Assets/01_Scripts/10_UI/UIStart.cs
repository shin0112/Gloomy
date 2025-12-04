using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LibraryButton : MonoBehaviour
{
    public void GoToLibraryScene()
    {
        SceneManager.LoadScene("LibraryScene");
    }
}
