using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System;
using System.Security.Cryptography;

public class Customers : Interactable
{
    public static event System.Action<FoodData> onFoodOrder;
    public static event System.Action<GameObject> onRemovedFromWaitList;
    public static event System.Action<GameObject, float> onFinished;

    [SerializeField] BoxCollider2D coll;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] SpriteRenderer foodSprite;
    BoxCollider2D table;
    Restraunt restraunt;

    [SerializeField] float timeWaited = 0.0f;
    float waitTime = 10f;
    FoodData order;

    enum MiniState
    {
        WAITING,
        ANGRY,
        FAILURE,
        SUCCESS
    }

    enum State
    {
        SEATING,
        ORDERING,
        FOOD,
        FAILED,
        SUCCEDED
    }

    State m_pastState;
    State m_state = State.SEATING;
    MiniState m_miniState = MiniState.WAITING;

    private void Start()
    {
        ChangeMiniState();
        RestartCoroutine();
    }

    private void RestartCoroutine()
    {
        if (m_state == State.SUCCEDED)
            waitTime = 2f;
        StopAllCoroutines();
        StartCoroutine(Waiting(ChangeState, waitTime));
    }

    public override void InteractWith(Interactable other)
    {
        if (other is Table table)
        {
            // if the customer is already at a table, dont reseat them
            if (this.table != null)
                return;

            // remove the customer from reseraunt wait list
            onRemovedFromWaitList?.Invoke(this.gameObject);

            // Move the sprite to the table
            this.table = table.GetComponent<BoxCollider2D>();
            this.coll.size = this.table.size;
            this.table.enabled = false;
            this.transform.position = this.table.transform.position;
            this.sprite.transform.position = new Vector3(this.transform.position.x - 1, this.transform.position.y, this.transform.position.z);

            sprite.color = Color.black;
            m_state = State.FOOD;
            m_miniState = MiniState.WAITING;
            StopAllCoroutines();
            StartCoroutine(Ordering(waitTime));
        }
    }

    private void ChangeMiniState()
    {
        switch (m_miniState)
        {
            case MiniState.WAITING:
                m_miniState = MiniState.ANGRY;
                sprite.color = Color.blue;
                break;
            case MiniState.ANGRY:
                m_miniState = MiniState.FAILURE;
                sprite.color = Color.yellow;
                break;
            case MiniState.FAILURE:
                m_pastState = m_state;
                m_state = State.FAILED;
                sprite.color = Color.red;
                break;
        }
    }

    private void ChangeState()
    {
        switch (m_state)
        {
            case State.SEATING:
                ChangeMiniState();
                break;
            case State.FOOD:
                ChangeMiniState();
                break;
            case State.FAILED:
                RestartTable();
                onFinished?.Invoke(this.gameObject, 0f);
                break;
            case State.SUCCEDED:
                RestartTable();
                onFinished?.Invoke(this.gameObject, timeWaited);
                break;
        }
        RestartCoroutine();
    }

    private void RestartTable()
    {
        if (this.table)
            this.table.enabled = true;
    }

    private void DetermineOrder()
    {
        order = restraunt.GetItemFromMenu();
        foodSprite.enabled = true;
        foodSprite.color = order.GetColor();
        onFoodOrder?.Invoke(order);
    }

    public bool GiveCustomerFood(FoodData food)
    {
        if (food.GetMenuName() != order.GetMenuName() || m_state == State.SEATING || m_miniState == MiniState.WAITING)
            return false;
        sprite.color = Color.green;
        m_state = State.SUCCEDED;
        RestartCoroutine();
        return true;
    }

    public void SetResteraunt(Restraunt restraunt)
    {
        this.restraunt = restraunt;
    }

    private IEnumerator Waiting(Action action, float time)
    {
        timeWaited += Time.deltaTime;
        yield return new WaitForSeconds(time);
        timeWaited += time;
        action();
    }

    private IEnumerator Ordering(float time)
    {
        timeWaited += Time.deltaTime;
        yield return new WaitForSeconds(time);
        ChangeState();
        DetermineOrder();
    }
}
