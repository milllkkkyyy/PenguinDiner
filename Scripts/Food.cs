using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Interactable
{
    public static event System.Action<GameObject> onFoodConsumed;

    FoodData food;

    public override void InteractWith(Interactable other)
    {
        if (other is Customers customer)
        {
            bool correct = customer.GiveCustomerFood(food);
            if (correct)
            {
                onFoodConsumed?.Invoke(this.gameObject);
            }
        }
        else if (other is Trashcan trash)
        {
            onFoodConsumed?.Invoke(this.gameObject);
        }
    }

    public void SetFood(FoodData order)
    {
        food = order;
    }
}
