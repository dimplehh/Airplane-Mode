using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region Panel
    [Header("Panel")]
    [SerializeField] public GameObject DisconnectPanel;
    [SerializeField] public GameObject RoomPanel;
    [SerializeField] public GameObject winRegamePanel;
    [SerializeField] public GameObject loseRegamePanel;
    [SerializeField] public GameObject GameUI;
    #endregion Panel
    #region panelChild
    [HideInInspector]public TMP_InputField NicknameInput; //2Â÷·Î ±ò²ûÈ÷ Á¤¸®
    [HideInInspector] public TMP_Text countTxt;
    [HideInInspector] public GameObject InitGameBtn;
    [HideInInspector] public GameObject hostWaitingTxt;
    [HideInInspector] public GameObject explainTxt;
    [HideInInspector] public GameObject disconnectTxt;
    [HideInInspector] public GameObject countDownTxt;
    [HideInInspector] public TMP_Text NicknameTxt1;
    [HideInInspector] public TMP_Text NicknameTxt2;
    [HideInInspector] public TMP_Text waveTxt;
    #endregion panelChild

    public static UIManager UI;
    void Awake() => UI = this;
    // Start is called before the first frame update
    void Start()
    {
        NicknameInput = DisconnectPanel.transform.GetChild(0).GetComponent<TMP_InputField>();
        UI.NicknameInput.characterLimit = 6;

        countTxt = RoomPanel.transform.GetChild(0).GetComponent<TMP_Text>();
        InitGameBtn = RoomPanel.transform.GetChild(1).gameObject;
        hostWaitingTxt = RoomPanel.transform.GetChild(2).gameObject;
        explainTxt = RoomPanel.transform.GetChild(3).gameObject;

        disconnectTxt = GameUI.transform.GetChild(0).gameObject;
        countDownTxt = GameUI.transform.GetChild(1).gameObject;
        NicknameTxt1 = GameUI.transform.GetChild(2).GetComponent<TMP_Text>();
        NicknameTxt2 = GameUI.transform.GetChild(3).GetComponent<TMP_Text>();
        waveTxt = GameUI.transform.GetChild(4).GetComponent<TMP_Text>();
    }
}
