using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kitchen : MonoBehaviour
{
    [SerializeField] GameObject food;
    [SerializeField] Transform foodSpawnPosition;

    public List<GameObject> foods = new List<GameObject>();

    int maxFood = 4;
    float foodOffset = 4f;

    private void OnEnable()
    {
        Customers.onFoodOrder += NewOrder;
        Food.onFoodConsumed += RemoveOrder;
    }

    private void OnDisable()
    {
        Customers.onFoodOrder -= NewOrder;
        Food.onFoodConsumed -= RemoveOrder;
    }

    private void NewOrder(FoodData order)
    {
        // create a new food and push it to the back of the food list
        StartCoroutine(GenerateFood(order));
    }

    private void RemoveOrder(GameObject food)
    {
        // reduce the number of current food we have and remove the food
        // object from the food list
        foods.Remove(food);
        Destroy(food);
        UpdateFoodDisplay();
    }

    private void UpdateFoodDisplay()
    {
        // Update the visuals of the food.
        // For a food in food and a start x and y
        // add the food with an offset in the x direction
        for (int i = 0; i < foods.Count; i++)
        {
            if (i > maxFood)
                foods[i].SetActive(false);
            else
                foods[i].SetActive(true);
            foods[i].transform.position = new Vector3(foodSpawnPosition.position.x + (i * foodOffset), foodSpawnPosition.position.y);
        }
    }

    private IEnumerator GenerateFood(FoodData order)
    {
        yield return new WaitForSeconds(Random.Range(5, 7));
        GameObject child = Instantiate(food, this.transform);
        child.GetComponent<SpriteRenderer>().color = order.GetColor(); 
        child.GetComponent<Food>().SetFood(order);
        foods.Add(child);
        UpdateFoodDisplay();
    }

}
