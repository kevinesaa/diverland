using System;
using System.Collections.Generic;
using UnityEngine;

public interface ICoinsService
{
    event Action<int> OnCoinsLoadedSuccessful;
    event Action<string, string> OnCoinsLoadedError;
    void GetUserCoins(string user);
    void AddCoinsToUser(string user,int coins);
}
