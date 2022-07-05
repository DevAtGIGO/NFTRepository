using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LandTokenData
{
    public string landName;
    public int x;
    public int y;
    public int tokenID;

    public LandTokenData(string name, int posX, int posY, int id)
    {
        landName = name;
        x = posX;
        y = posY;
        tokenID = id; 
    }
}   
public class BuyLandHandler : MonoBehaviour
{
    // set chain: ethereum, moonbeam, polygon etc
    string chain = "ethereum";
    // set network mainnet, testnet
    string network = "Smart Chain — Testnet";
    
    string rpc = "https://data-seed-prebsc-1-s1.binance.org:8545/";
    //Rinkeby rpc
    //string rpc = "https://eth-rinkeby.alchemyapi.io/v2/8uSdDEWHacrK5cMEJiGjITvaPTRSHIJG";

    // abi in json format
    string abi = "[ { \"inputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"constructor\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"owner\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"approved\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"uint256\", \"name\": \"tokenId\", \"type\": \"uint256\" } ], \"name\": \"Approval\", \"type\": \"event\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"owner\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"operator\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"bool\", \"name\": \"approved\", \"type\": \"bool\" } ], \"name\": \"ApprovalForAll\", \"type\": \"event\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"from\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"to\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"uint256\", \"name\": \"tokenId\", \"type\": \"uint256\" } ], \"name\": \"Transfer\", \"type\": \"event\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"to\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"tokenId\", \"type\": \"uint256\" } ], \"name\": \"approve\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"owner\", \"type\": \"address\" } ], \"name\": \"balanceOf\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"string\", \"name\": \"_name\", \"type\": \"string\" }, { \"internalType\": \"address\", \"name\": \"_to\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"_xPos\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"_yPos\", \"type\": \"uint256\" } ], \"name\": \"createNewLand\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"getAllLands\", \"outputs\": [ { \"components\": [ { \"internalType\": \"string\", \"name\": \"name\", \"type\": \"string\" }, { \"internalType\": \"uint256\", \"name\": \"xPos\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"yPos\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"id\", \"type\": \"uint256\" } ], \"internalType\": \"struct PollutionLand.Land[]\", \"name\": \"\", \"type\": \"tuple[]\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"tokenId\", \"type\": \"uint256\" } ], \"name\": \"getApproved\", \"outputs\": [ { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"owner\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"operator\", \"type\": \"address\" } ], \"name\": \"isApprovedForAll\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"name\": \"lands\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"name\", \"type\": \"string\" }, { \"internalType\": \"uint256\", \"name\": \"xPos\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"yPos\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"id\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"name\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"tokenId\", \"type\": \"uint256\" } ], \"name\": \"ownerOf\", \"outputs\": [ { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"from\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"to\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"tokenId\", \"type\": \"uint256\" } ], \"name\": \"safeTransferFrom\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"from\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"to\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"tokenId\", \"type\": \"uint256\" }, { \"internalType\": \"bytes\", \"name\": \"data\", \"type\": \"bytes\" } ], \"name\": \"safeTransferFrom\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"operator\", \"type\": \"address\" }, { \"internalType\": \"bool\", \"name\": \"approved\", \"type\": \"bool\" } ], \"name\": \"setApprovalForAll\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"bytes4\", \"name\": \"interfaceId\", \"type\": \"bytes4\" } ], \"name\": \"supportsInterface\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"symbol\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"tokenId\", \"type\": \"uint256\" } ], \"name\": \"tokenURI\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"from\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"to\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"tokenId\", \"type\": \"uint256\" } ], \"name\": \"transferFrom\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" } ]";

    string contract = "0xC0EF739C10682859Bdec0BA831E929b6D378cfaC";

    public Action<List<LandTokenData>> onLandDataChange;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void BuyLand(int x, int y, string name)
    {
        // account to send to
        string to = "0xbE192D7ef970d13C504390d793A50269b1264dC5";
        // amount in wei to send
        string value = "10000000000000";
        // gas limit OPTIONAL
        string gasLimit = "";
        // gas price OPTIONAL
        string gasPrice = "";
        // connects to user's browser wallet (metamask) to send a transaction
        try
        {
            string response = await Web3GL.SendTransaction(to, value, gasLimit, gasPrice);

            string txConfirmed = await EVM.TxStatus(chain, network, response, rpc);
            while(txConfirmed == "pending")
            {
                await new WaitForSeconds(0.1f);
                txConfirmed = await EVM.TxStatus(chain, network, response, rpc);
            }

            if(txConfirmed == "success")
            {
                // Transaction confirmed
                PurchaseSuccess(x, y, name);
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }
    }

    async void PurchaseSuccess(int x, int y, string name)
    {
        Debug.Log("Purchase Success " + x + ":" + y);

        string account = PlayerPrefs.GetString("Account");

        // smart contract method to call
        string method = "createNewLand";
        // array of arguments for contract
        string args = "[" + "\"" + name + "\"" + "," + "\"" + account + "\"" + "," + "\"" + x + "\"" + "," + "\"" + y + "\"" + "]";
        // connects to user's browser wallet to call a transaction
        // display response in game
        string value = "0";
        string gasLimit = "";
        // gas price OPTIONAL
        string gasPrice = "";

        try
        {
            string response = await Web3GL.SendContract(method, abi, contract, args, value, gasLimit, gasPrice);
            Debug.Log("BuyLandHandler Response " + response);
            string txConfirmed = await EVM.TxStatus(chain, network, response, rpc);
            while (txConfirmed == "pending")
            {
                await new WaitForSeconds(0.1f);
                txConfirmed = await EVM.TxStatus(chain, network, response, rpc);
            }
            OnLandUpdate();
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }
    }

    public async void OnLandUpdate()
    {
        string method = "getAllLands";
        // array of arguments for contract
        string args = "[]";
        // connects to user's browser wallet to call a transaction
        string response = await EVM.Call(chain, network, contract, abi, method, args, rpc);
        // display response in game

        Debug.Log("BuyLandHandler " + " LookUp " + response);

        GetLandData(response);
    }

    void GetLandData(string contractResponse)
    {
        string lands = contractResponse.Substring(1, contractResponse.Length - 2);
        Debug.Log(lands);
        List<LandTokenData> landData = new List<LandTokenData>();

        foreach (string landStr in lands.Split("],["))
        {
            string landString = landStr.Trim('[');
            landString = landString.Trim(']');

            string[] landDetails = landString.Replace("\"", "").Split(',');

            int xPos = 0;
            int.TryParse(landDetails[1], out xPos);

            int yPos = 0;
            int.TryParse(landDetails[2], out yPos);

            int tokenId = 0;
            int.TryParse(landDetails[3], out tokenId);

            LandTokenData landToken = new LandTokenData(landDetails[0], xPos, yPos, tokenId);
            landData.Add(landToken);
        }

        onLandDataChange?.Invoke(landData);
    }
}
