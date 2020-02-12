using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Text progressText;
    public Image progressBar;
    public Text currencyText;

    public Text statusText;

    private float lifetime;

    public int currency;

    private void Start()
    {
        progressText.text = "0%";
        progressBar.rectTransform.localScale = new Vector3(0, 1, 1);
        currency = 20;
    }

    private void Update()
    {
        lifetime += Time.deltaTime;
        currencyText.text = currency.ToString();

        if (lifetime >= 10)
        {
            currency += 5;
            lifetime = 0;
        }
    }

    public bool plantPlanted(int cost)
    {
        if (currency >= cost)
        {
            currency -= cost;
            return true;
        }
        else
            return false;
    }

    public void plantFinishes()
    {
        currency += 25;
    }

    public void updateProgress(float progress)
    {
        progressText.text = (int)(progress * 100) + "%";
        progressBar.rectTransform.localScale = new Vector3(progress, 1, 1);
    }

    public void exit()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
