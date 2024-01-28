using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneySystem : StaticInstance<MoneySystem>
{
    public static int money = 50;
    public Coroutine moneyCoroutine;

    public static int currentMoney()
    {
        return money;
    }

    public void AddMoney(int add)
    {
        money += add;
    }

    public void ReduceMoney(int reduce)
    {
        money -= reduce;
    }

}
