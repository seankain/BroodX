using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    Menu,Playing,Win
}

public class GameControl : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    private MenuPan menuPan;
    private vThirdPersonCamera thp;
    private GameState gameState;
    [SerializeField]
    private Text MenuText;
    [SerializeField]
    private Text HintText;
    private BroodPlayer player;
    private int level = 0;
    private Goal goal;
    private CicadaSpawner cicadaSpawner;

    // Start is called before the first frame update
    void Start()
    {
        menuPan = cam.GetComponent<MenuPan>();
        thp = cam.GetComponent<vThirdPersonCamera>();
        player = FindObjectOfType<BroodPlayer>();
        cicadaSpawner = FindObjectOfType<CicadaSpawner>();
        goal = FindObjectOfType<Goal>();
        goal.PlayerScored += HandlePlayerScored;
        SetGameState(GameState.Menu);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.Menu && Input.GetMouseButtonDown(0))
        {
            HideMenu();
            StartPlay();
        }
    }

    public void SetGameState(GameState nextGameState)
    {
        gameState = nextGameState;
        switch (gameState)
        {
            case GameState.Menu:
                ShowMenu();
                break;
            case GameState.Playing:
                StartPlay();
                break;
            case GameState.Win:
                ShowWin();
                break;
            default:
                ShowMenu();
                break;
        }
    }

    private void ShowMenu() 
    {
        menuPan.enabled = true;
        thp.enabled = false;
        MenuText.text = "Cicada Avoider";
        HintText.text = "Click To Start";
    }

    private void HideMenu()
    {
        menuPan.enabled = false;
        thp.enabled = true;
        MenuText.text = string.Empty;
        HintText.text = string.Empty;
    }

    private IEnumerator ShowObjectiveHint(string hint)
    {
        HintText.text = hint;
        yield return new WaitForSeconds(3);
        HintText.text = string.Empty;
    }
    private void StartPlay()
    {
        gameState = GameState.Playing;
        var message = $"Level {level}";
        if(level == 0)
        {
            message = "Get Inside!";
        }
        StartCoroutine(ShowObjectiveHint(message));
    }
    private void ShowWin() {
        StartCoroutine(ShowObjectiveHint("You did it!"));
    }

    private void HandlePlayerScored(object sender) {
        level++;
        //ShowWin();
        player.ReturnToOrigin();
        UpdateDifficulty();
        StartPlay();
    }

    private void UpdateDifficulty()
    {
        cicadaSpawner.KamikazeProbability = level / 10.0f;
        if (cicadaSpawner.CicadaPoolSize < 100)
        {
            cicadaSpawner.CicadaPoolSize++;
        }
        Debug.Log($"Kamikazie Probability: {cicadaSpawner.KamikazeProbability}. Spawner Pool Size: {cicadaSpawner.CicadaPoolSize}");
    }

}
