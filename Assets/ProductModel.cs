using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Net;
using System.IO;

public enum Level
{
    Low,
    Medium,
    High
}

[Serializable]
public class ProductModel
{
    public string code;
    //public string productCode;
    public string brand;
    // categories: first from categories
    public string category;
    public string countryOfOrigin;

    /* PHYISICAL PACKAGING */

    // packaging_tags
    public bool? hasPlasticPackaging; // true: plastique
    
    /* NUTRIENTS */

    // nutriscore_data
    // good if high:    fruits and vegetables, fibers, and protein
    // good if low:     energy, sugar, saturated fatty acids, and sodium
    public string grade;

    // nutrient_levels_tags
    // nutrient_levels
    public Level? fat;
    public Level? saturatedFat;
    public Level? sugars;
    public Level? salt;

    // nutrients data
    public KeyValuePair<string, string> nutriments;

    /* INGREDIENTS */

    // ingredients_analysis_tags;
    public bool? isPalmOil = null;      // null: palm-oil-content-unknown
    public bool? isVegan = null;        // false: non-vegan
    public bool? isVegetarian = null;   // null: vegetarian-status-unknown, false: non-vegetarian

    // ingredients: per serving
    public List<IngredientModel> ingredients;
    

    // ingredients_text
    public string textForVoice;

    private string J2S(JSONObject propertyObject)
    {
        return propertyObject.ToString().Replace("\"", "");
    }

    public ProductModel(string json)
    {
        Debug.Log("BLANK: JSON parse started");
        Debug.Log(json);

        // https://github.com/mtschoen/JSONObject
        JSONObject rootObject = new JSONObject(json);
        JSONObject productObject = rootObject["product"];
        JSONObject nutriscoreObject = productObject["nutriscore_data"];

        this.code = J2S(rootObject["code"]);
        this.brand = J2S(productObject["brands"]);
        this.category = J2S(productObject["pnns_groups_1"]);
        this.countryOfOrigin = J2S(productObject["countries"]);
        this.hasPlasticPackaging = J2S(productObject["packaging"]).Contains("plast");

        this.grade = J2S(nutriscoreObject["grade"]);


        /*this.fat = nutriscoreObject["grade"].ToString();
        this.saturatedFat = nutriscoreObject["grade"].ToString();
        this.sugars = nutriscoreObject["grade"].ToString();
        this.salt = nutriscoreObject["grade"].ToString();*/


        Debug.Log("BLANK: json code: plastic:" + this.hasPlasticPackaging);
    }

}
