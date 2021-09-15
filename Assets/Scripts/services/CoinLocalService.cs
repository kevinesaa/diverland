using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinLocalService : ICoinsService
{
    public event Action<int> OnCoinsLoadedSuccessful;
    public event Action<string, string> OnCoinsLoadedError;

    public void AddCoinsToUser(string user,int coins)
    {
        int coinsSaved = GetCoins();
        coinsSaved += coins;
        PlayerPrefs.SetInt(Constants.COIN_SAVED, coinsSaved);
        PlayerPrefs.Save();
        if (OnCoinsLoadedSuccessful != null)
            OnCoinsLoadedSuccessful(coinsSaved);
    }

    public void GetUserCoins(string user)
    {
        int coinsSaved = GetCoins();
        if (OnCoinsLoadedSuccessful != null)
            OnCoinsLoadedSuccessful(coinsSaved);
    }

    private int GetCoins()
    {
        return PlayerPrefs.GetInt(Constants.COIN_SAVED, 0); 
    }

    
}
