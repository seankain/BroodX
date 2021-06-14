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
    

    // Start is called before the first frame update
    void Start()
    {
        menuPan = cam.GetComponent<MenuPan>();
        thp = cam.GetComponent<vThirdPersonCamera>();
        SetGameState(GameState.Menu);
    }

    // Update is called once per frame
    void Update()
    {
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
    }
    private void StartPlay() { }
    private void ShowWin() { }

}
