using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : BehaviourSingleton<MoneyManager>
{
    private double balance;

    double addValue(double value)
    {
        return balance + value;
    }
}
