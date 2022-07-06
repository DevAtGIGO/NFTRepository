using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using FirebaseWebGL.Scripts.FirebaseBridge;
using TMPro;
using FirebaseWebGL.Examples.Utils;
using FirebaseWebGL.Scripts.Objects;
using System.Collections.Generic;
using System;
using DG.Tweening;

public class WalletDetails
{
    public string walletID;

    public WalletDetails(string walletID)
    {
        this.walletID = walletID;
    }
}

public class LoginController : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void Web3Connect();

    [DllImport("__Internal")]
    private static extern string ConnectAccount();

    [DllImport("__Internal")]
    private static extern void SetConnectAccount(string value);

    public void CreateUserWithEmailAndPassword() =>
        FirebaseAuth.CreateUserWithEmailAndPassword(emailInputField.text, passwordInputField.text, gameObject.name, "DisplayInfo", "DisplayErrorObject");

    public void SignInWithEmailAndPassword() =>
        FirebaseAuth.SignInWithEmailAndPassword(emailInputField.text, passwordInputField.text, gameObject.name, "DisplayInfo", "DisplayErrorObject");

    public void SignInWithGoogle() =>
        FirebaseAuth.SignInWithGoogle(gameObject.name, "DisplayInfo", "DisplayErrorObject");

    public void SignInWithFacebook() =>
        FirebaseAuth.SignInWithFacebook(gameObject.name, "DisplayInfo", "DisplayErrorObject");

    private int expirationTime;
    private string account;

    [SerializeField]
    private Button signUp;
    [SerializeField]
    private Button login;
    [SerializeField]
    private Button loginGoogle;
    [SerializeField]
    private Button loginFB;
    [SerializeField]
    private Button clearWeb3;
    [SerializeField]
    private Button signOut;
    [SerializeField]
    private Button enterGame;

    [SerializeField]
    private CanvasGroup loginCanvas;

    [SerializeField]
    private CanvasGroup continueCanvas;

    [SerializeField]
    private TMP_InputField emailInputField;

    [SerializeField]
    private TMP_InputField passwordInputField;

    [SerializeField]
    private TextMeshProUGUI outputText;

    private string currentUid;

    private void Start()
    {
        ToggleLoginUI(true, true);

        if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
            DisplayError("The code is not running on a WebGL build; as such, the Javascript functions will not be recognized.");
            return;
        }

        clearWeb3.gameObject.SetActive(false);

        FirebaseAuth.OnAuthStateChanged(gameObject.name, "DisplayUserInfo", "DisplayInfo");

        signUp.onClick.RemoveAllListeners();
        login.onClick.RemoveAllListeners();
        loginGoogle.onClick.RemoveAllListeners();
        loginFB.onClick.RemoveAllListeners();
        signOut.onClick.RemoveAllListeners();
        clearWeb3.onClick.RemoveAllListeners();

        signUp.onClick.AddListener(CreateUserWithEmailAndPassword);
        login.onClick.AddListener(SignInWithEmailAndPassword);
        loginGoogle.onClick.AddListener(SignInWithGoogle);
        loginFB.onClick.AddListener(SignInWithFacebook);
        signOut.onClick.AddListener(SignOut);
        clearWeb3.onClick.AddListener(ClearWeb3Account);
        enterGame.onClick.AddListener(EnterGame);
    }

    private void ToggleLoginUI(bool value, bool immediate = false)
    {
        CanvasGroup toShow = value ? loginCanvas : continueCanvas;
        CanvasGroup toHide = value ? continueCanvas : loginCanvas;

        toShow.interactable = true;
        toHide.interactable = false;

        toShow.blocksRaycasts = true;
        toHide.blocksRaycasts = false;

        if (immediate)
        {
            toShow.alpha = 1;
            toHide.alpha = 0;
        } else
        {
            toShow.DOFade(1f, 0.5f);
            toHide.DOFade(0f, 0.25f);
        }
    }

    public void DisplayUserInfo(string user)
    {
        var parsedUser = StringSerializationAPI.Deserialize(typeof(FirebaseUser), user) as FirebaseUser;
        SuccessfulLogin(parsedUser);
    }

    public void SuccessfulLogin(FirebaseUser parsedUser)
    {
        currentUid = parsedUser.uid;
        outputText.color = outputText.color == Color.green ? Color.blue : Color.green;
        string data = $"Email: {parsedUser.email}, UserId: {parsedUser.uid}, EmailVerified: {parsedUser.isEmailVerified}";
        outputText.text = data;
        Debug.Log(data);

        FirebaseFirestore.GetDocument("userWalletDetails", currentUid, this.name, "OnFetchWalletDetails", "DisplayInfo");
    }

    public void OnFetchWalletDetails(string walletDetails)
    {
        outputText.text = "WALLET DETAILS " + walletDetails;

        WalletDetails storedDetails = StringSerializationAPI.Deserialize(typeof(WalletDetails), walletDetails) as WalletDetails;
        outputText.text = "Deserialized WalletDetails " + storedDetails.walletID;

        OnWeb3Login(storedDetails.walletID);
    }

    public void DisplayInfo(string info)
    {
        outputText.color = Color.white;
        outputText.text = info;
        Debug.Log(info);
    }

    public void DisplayErrorObject(string error)
    {
        var parsedError = StringSerializationAPI.Deserialize(typeof(FirebaseError), error) as FirebaseError;
        DisplayError(parsedError.message);
    }

    public void DisplayError(string error)
    {
        outputText.color = Color.red;
        outputText.text = error;
        Debug.LogError(error);
    }

    public void OnWeb3Login(string storedWalletID)
    {
        Web3Connect();
        OnConnected(storedWalletID);
    }

    async private void OnConnected(string storedWalletID)
    {
        account = ConnectAccount();
        while (account == "")
        {
            await new WaitForSeconds(0.5f);
            account = ConnectAccount();
        };

        // If current user does not have a wallet mapped
        if(string.IsNullOrEmpty(storedWalletID))
        {
            FirebaseFirestore.GetDocumentsInCollection("userWalletDetails", this.name, "FetchAllWalletSuccess", "DisplayErrorObject");
        }
        // If current user does not have same wallet
        else if(storedWalletID != account)
        {
            outputText.color = Color.red;
            outputText.text = "Wallet ID and LoginId differ, please rejoin with account: " + account;
            clearWeb3.gameObject.SetActive(true);
        // If current user does not have stored wallet
        // check if any other user has same wallet
        } else
        {
            // save account for next scene
            FirebaseFirestore.GetDocumentsInCollection("userWalletDetails", this.name, "FetchAllWalletSuccess", "DisplayErrorObject");
        }



        // load next scene
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void FetchAllWalletSuccess(string data)
    {
        Dictionary<string, WalletDetails> storedDetails = StringSerializationAPI.Deserialize(typeof(Dictionary<string, WalletDetails>), data) as Dictionary<string, WalletDetails>;

        bool duplicateWallet = false;
        foreach(KeyValuePair<string, WalletDetails> wallet in storedDetails)
        {
            if(wallet.Value.walletID == account && wallet.Key != currentUid)
            {
                duplicateWallet = true;
            }
        }

        if (duplicateWallet == true)
        {
            DisplayError("A duplicate wallet login exists with different sign in details\nPlease sign in with the correct wallet");
            account = "";
            ClearWeb3Account();
            SignOut();
        }
        else
        {
            // Connection successful
            PlayerPrefs.SetString("Account", account.ToUpper());
            SetConnectAccount(account);

            WalletDetails newWallet = new WalletDetails(account);
            string updatedWallet = StringSerializationAPI.Serialize(typeof(WalletDetails), newWallet);

            FirebaseFirestore.SetDocument("userWalletDetails", currentUid, updatedWallet, this.name, "", "DisplayErrorObject");

            ToggleLoginUI(false);
        }
    }

    void EnterGame()
    {
        // Go to main menu
        SceneManager.LoadScene(1);
    }

    public void SignOut()
    {
        FirebaseAuth.OnSignOut(this.name, "DisplayInfo", "DisplayErrorObject");
        ToggleLoginUI(true);
    }

    public void ClearWeb3Account()
    {
        PlayerPrefs.SetString("Account", "");

        // Remove wallet details from our backend so that user can connect metamask wallet with another user
        WalletDetails newWallet = new WalletDetails("");
        string updatedWallet = StringSerializationAPI.Serialize(typeof(WalletDetails), newWallet);
        FirebaseFirestore.SetDocument("userWalletDetails", currentUid, "", this.name, "UpdateWalletSuccess", "UpdateWalletFailed");
    }
}
