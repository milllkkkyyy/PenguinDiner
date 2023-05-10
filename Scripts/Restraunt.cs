using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Restraunt : MonoBehaviour
{
    [SerializeField] UIController ui;
    [SerializeField] GameObject customer;
    [SerializeField] Transform customerSpawnPosition;

    // Variables for keeping track of customers
    public List<GameObject> waitingCustomers = new List<GameObject>();
    public List<GameObject> sittingCustomers = new List<GameObject>();

    float customerOffset = 4f;
    int maxCustomers = 2;

    // Variables for money
    [SerializeField] int goal = 0;
    int money = 0;

    // Variables for time
    [SerializeField] float closingTime = 0f; // (seconds)
    float time = 0.0f; // (seconds)

    // Variables for food menu
    [SerializeField] List<FoodData> menu;

    private void Start()
    {
        ui.ChangeGoalLabel(goal);
        ui.ChangeMoneyLabel(money);
        StartCoroutine(GenerateCustomer());
    }

    private void OnEnable()
    {
        Customers.onRemovedFromWaitList += SwapList;
        Customers.onFinished += RemoveCustomer;
    }

    private void OnDisable()
    {
        Customers.onRemovedFromWaitList -= SwapList;
        Customers.onFinished -= RemoveCustomer;
    }

    private void UpadateCustomerDisplay()
    {
        // Update the visuals of the food.
        // For a food in food and a start x and y
        // add the food with an offset in the x direction
        for (int i = 0; i < waitingCustomers.Count; i++)
        {
            if (i >= maxCustomers)
                waitingCustomers[i].SetActive(false);
            else
                waitingCustomers[i].SetActive(true);
            waitingCustomers[i].transform.position = new Vector3(customerSpawnPosition.position.x + (i * customerOffset), customerSpawnPosition.position.y);
        }
    }

    private void SwapList(GameObject customer)
    {
        waitingCustomers.Remove(customer);
        sittingCustomers.Add(customer);
        UpadateCustomerDisplay();
    }

    private void RemoveCustomer(GameObject customer, float tip)
    {
        // calculate the money made
        money += (int)(tip);
        ui.ChangeMoneyLabel(money);

        // remove the customer
        waitingCustomers.Remove(customer);
        sittingCustomers.Remove(customer);
        Destroy(customer);
    }

    private IEnumerator GenerateCustomer()
    {
        GameObject child = Instantiate(customer, this.transform);
        child.GetComponent<Customers>().SetResteraunt(this);
        waitingCustomers.Add(child);
        UpadateCustomerDisplay();
        yield return new WaitForSeconds(Random.Range(3, 10));
        StartCoroutine(GenerateCustomer());
    }

    // The only update method because we must check time
    private void Update()
    {
        // Finished game when the resteraunt is closed, and there are no more customers
        if (time > closingTime && sittingCustomers.Count + waitingCustomers.Count == 0)
        {
            if (money > goal)
                ui.SetWonLabelText("You Won! \n Press the screen to Restart");
            else
                ui.SetWonLabelText("You Lost! \n Press the screen to Restart");
            Time.timeScale = 0;
        }
        else if (time > closingTime)
        {
            // If the game is finished stop spawning customers
            StopAllCoroutines();
        }
        else
        {
            time += Time.deltaTime;
        }
        ui.ChangeTimeLabel(time);
    }

    public FoodData GetItemFromMenu()
    {
        return menu[Random.Range(0, menu.Count-1)];
    }
}
