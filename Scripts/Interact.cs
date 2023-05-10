using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.ExecuteEvents;

public class Interact : MonoBehaviour
{
    [SerializeField] Material highlight;
    Material oldMaterial;

    private Controller input;
    private Interactable clickedObject;

    void Awake()
    {
        input = new Controller();
        input.Enable();
    }

    void OnEnable()
    {
        input.Player.Interact.performed += InteractWithWorld;
    }
    void OnDisable()
    {
        input.Player.Interact.performed -= InteractWithWorld;
        input.Disable();
    }

    private void InteractWithWorld(InputAction.CallbackContext context)
    {
        // restart game
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Main");
        }

        // locate mouse
        Vector2 position = context.action.ReadValue<Vector2>();
        Collider2D hit = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(position));
        if (hit != null)
        {
            if (hit.tag == "Interactable")
            {
                Interactable interactable = hit.gameObject.GetComponent<Interactable>();

                if (clickedObject == null)
                {
                    clickedObject = interactable;
                    oldMaterial = clickedObject.GetComponentInChildren<SpriteRenderer>().material;
                    clickedObject.GetComponentInChildren<SpriteRenderer>().material = highlight;
                    bool use = clickedObject.Use();
                    if (use)
                    {
                        clickedObject = null;
                    }
                }
                else if (clickedObject == interactable)
                {
                    clickedObject.GetComponentInChildren<SpriteRenderer>().material.shader = oldMaterial.shader;
                    clickedObject = null;
                }
                else
                {
                    clickedObject.InteractWith(interactable);
                    clickedObject.GetComponentInChildren<SpriteRenderer>().material.shader = oldMaterial.shader;
                    clickedObject = null;
                }
            }
        }
    }
}
