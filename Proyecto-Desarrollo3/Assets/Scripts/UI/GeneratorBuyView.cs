using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class GeneratorBuyView : MonoBehaviour
{
    [SerializeField] private Button btnBuy = null;
    [SerializeField] private TMP_Text txtName = null;
    [SerializeField] private TMP_Text txtPrice = null;
    [SerializeField] private Image imgIcon = null;

    private string id = null;
    
    public string Id { get => id; }
    
    public void Init(string id, string name, double price, Action<string> onTryBuyGenerator)
    {
        this.id = id;
        txtName.text = name;
        txtPrice.text = price.ToString(CultureInfo.InvariantCulture);
        
        btnBuy.onClick.AddListener( () =>
        {
            onTryBuyGenerator.Invoke(this.id);
        });
    }

    public void UpdateData(double price)
    {
        txtPrice.text = price.ToString(CultureInfo.InvariantCulture);
    }
}