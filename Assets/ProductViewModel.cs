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

    // visibility range views
    public GameObject visibleFarView;
    public GameObject visibleMidView;
    public GameObject visibleCloseView;

    public void updateVisibleRange(float? distanceMeter)
    {
        Debug.Log("BLANK: updateVisibleRange: distance: " + distanceMeter);

        if (distanceMeter.HasValue && visibleFarView && visibleMidView && visibleCloseView)
        {
            if (distanceMeter >= 2)
            {
                // show nothing
                Debug.Log("BLANK: updateVisibleRange: Too far.");

                visibleFarView.SetActive(false);
                visibleMidView.SetActive(false);
                visibleCloseView.SetActive(false);
            }
            else if (distanceMeter >= 0.8)
            {
                // show high-level info
                Debug.Log("BLANK: updateVisibleRange: High-level info");

                visibleFarView.SetActive(true);
                visibleMidView.SetActive(false);
                visibleCloseView.SetActive(false);
            }
            else if (distanceMeter >= 0.3)
            {
                // show mid-detailed info
                Debug.Log("BLANK: updateVisibleRange: Mid-level info");

                visibleFarView.SetActive(false);
                visibleMidView.SetActive(true);
                visibleCloseView.SetActive(false);
            }
            else if(distanceMeter < 0.3)
            {
                // show highly-detailed info
                Debug.Log("BLANK: updateVisibleRange: Detailed info");

                visibleFarView.SetActive(false);
                visibleMidView.SetActive(false);
                visibleCloseView.SetActive(true);
            }
        }
        
    }


    private void bindModel(ProductModel model)
    {
        Debug.Log("BLANK: Bind model: " + model.brand);
        brandView.text = model.brand;
    }

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

    public void GetData(string productCode)
    {

        Debug.Log("BLANK: get data");

        //GameObject temp = GameObject.Find("Quad");
        //brandView = temp.transform.GetChild(0).gameObject.GetComponent<TextMesh>();
        //string productCodeFromImageMetadata = brandView.text;

        Debug.Log("BLANK: get data: code: " + productCode);

        if (productCode != null && productCode.Length > 0)
        {
            Debug.Log("BLANK: get data with code: " + productCode);

            //Converting TextMesh to String
            ProductModel model = GetProduct(productCode);

            bindModel(model);

            Debug.Log("model---------------" + model);
        }
        else
        {
            Debug.Log("BLANK: empty metadata");
        }
        
    }
}
