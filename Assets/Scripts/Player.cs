using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Text progressText;

    private void Start()
    {
        progressText.text = "0%";
    }

    public void updateProgress(float progress)
    {
        progressText.text = (int)(progress * 100) + "%";
    }
}
