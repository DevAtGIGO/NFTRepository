using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyLandHandler : MonoBehaviour
{
    // set chain: ethereum, moonbeam, polygon etc
    string chain = "ethereum";
    // set network mainnet, testnet
    string network = "Smart Chain — Testnet";
    
    string rpc = "https://data-seed-prebsc-1-s1.binance.org:8545/";
    //Rinkeby rpc
    //string rpc = "https://eth-rinkeby.alchemyapi.io/v2/" + "8uSdDEWHacrK5cMEJiGjITvaPTRSHIJG";
    // abi in json format
    

    string abi = "[ { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"previousOwner\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"newOwner\", \"type\": \"address\" } ], \"name\": \"OwnershipTransferred\", \"type\": \"event\" }, { \"inputs\": [], \"name\": \"owner\", \"outputs\": [ { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"renounceOwnership\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"newOwner\", \"type\": \"address\" } ], \"name\": \"transferOwnership\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" } ]";

    string contract = "0x6B207135345ADf5528e27A69e9D231aC774E4DD8";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void BuyLand(int x, int y)
    {
        // account to send to
        string to = "0xbE192D7ef970d13C504390d793A50269b1264dC5";
        // amount in wei to send
        string value = "10000000000000000";
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
                PurchaseSuccess(x, y);
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }
    }

    async void PurchaseSuccess(int x, int y)
    {
        Debug.Log("Purchase Success " + x + ":" + y);

        string account = PlayerPrefs.GetString("Account");

        // smart contract method to call
        string method = "createNewLand";
        // array of arguments for contract
        string args = "[" + "\"Land1\"" + "," + "\"" + account + "\"" + "," + x + "," + y + "]";
        // connects to user's browser wallet to call a transaction
        string response = await EVM.Call(chain, network, contract, abi, method, args, rpc);
        // display response in game

        Debug.Log("BuyLandHandler " + " Response " + response);
    }

    public async void OnCheck()
    {
        // smart contract method to call
        string method = "lands";
        // array of arguments for contract
        string args = "[]";
        // connects to user's browser wallet to call a transaction
        string response = await EVM.Call(chain, network, contract, abi, method, args, rpc);
        // display response in game

        Debug.Log("BuyLandHandler " + " Response " + response);
    }
}
