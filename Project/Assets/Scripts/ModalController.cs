using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModalController : MonoBehaviour
{
    public Button button;
    public ModalVfx modalVfx;
    private void Awake()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(()=>
        {
            modalVfx.Hide();
        });
        gameObject.SetActive(false);
    }

    public void Hide()
    {
        modalVfx.Hide();
    }
}
