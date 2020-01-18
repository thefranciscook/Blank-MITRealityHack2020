using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Net;
  using System;
  using System.IO;
using UnityEngine.UI;



public class ProductViewModel : MonoBehaviour
{
    // views
    public TextMesh brandView;

    private void bindModel(ProductModel model)
    {
        Debug.Log("BLANK: Bind model: " + model.brand);
        brandView.text = model.brand;
    }

    static string ProductId_LuckyCharms = "0016000123991";
    static string ProductId_KellogsOriginal = "0032000016170";

    // openfoodfacts.org REST API
    // doc: https://en.wiki.openfoodfacts.org/API/Read/Product
    // first string parameter: product id = barcode
    static string ProductGETUrl = "https://world.openfoodfacts.org/api/v0/product/{0}.json";

    private ProductModel GetProduct(string productId)
      {
          Debug.Log("BLANK: Get product called");

          HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format(ProductGETUrl, productId));
          HttpWebResponse response = (HttpWebResponse)request.GetResponse();
          StreamReader reader = new StreamReader(response.GetResponseStream());
          string jsonResponse = reader.ReadToEnd();
          //WeatherInfo info = JsonUtility.FromJson<WeatherInfo>(jsonResponse);
          // parse manually, openfood's JSON object is craaaaazy complex schema

          return new ProductModel(jsonResponse);
      }

    

    // Start is called before the first frame update
    void Start()
    {
        /*
        Debug.Log("BLANK: Start");

        ProductModel model = GetProduct(ProductId_LuckyCharms);

        bindModel(model);
        */

        //GetData();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetData()
    {
        GameObject temp = GameObject.Find("Quad");
        brandView = temp.transform.GetChild(0).gameObject.GetComponent<TextMesh>();


        if(brandView != null && brandView.ToString().Length > 0)
        {
            Debug.Log("BLANK: get data");

            //Converting TextMesh to String
            ProductModel model = GetProduct(brandView.ToString());

            bindModel(model);

            brandView.text = model.ToString();

            Debug.Log("model---------------" + model);
        }
        else
        {
            Debug.Log("BLANK: empty metadata");
        }
        
    }
}
