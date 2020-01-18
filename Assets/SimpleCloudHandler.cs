using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

using Vuforia;


public class SimpleCloudHandler : MonoBehaviour, IObjectRecoEventHandler
{
    private bool mIsScanning = false;
    public string mTargetMetadata = "";

    

    public Material quadMaterial;

    public ImageTargetBehaviour behaviour;
    private CloudRecoBehaviour cloud;
    public GameObject quad;
    public TextMesh textMesh;

    public float distance;

    //Json
    public ProductViewModel pvm;


    public void OnInitialized(TargetFinder targetFinder)
    {
        Debug.Log("Cloud Reco initialized");
    }
    public void OnInitError(TargetFinder.InitState initError)
    {
        Debug.Log("Cloud Reco init error " + initError.ToString());
    }
    public void OnUpdateError(TargetFinder.UpdateState updateError)
    {
        Debug.Log("Cloud Reco update error " + updateError.ToString());
    }

    public void OnStateChanged(bool scanning)
    {

        mIsScanning = scanning;
        if (scanning)
        {
            ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
            tracker.GetTargetFinder<ImageTargetFinder>().ClearTrackables(false);
        }

    }


    // Here we handle a cloud target recognition event
    public void OnNewSearchResult(TargetFinder.TargetSearchResult targetSearchResult)
    {

        GameObject newImageTarget = Instantiate(behaviour.gameObject) as GameObject;
        quad = newImageTarget.transform.GetChild(1).gameObject;
        textMesh = quad.transform.GetChild(1).gameObject.GetComponent<TextMesh>();

        pvm = newImageTarget.transform.GetChild(0).gameObject.GetComponent<ProductViewModel>();

        GameObject augmentation = null;
        if(augmentation != null)
        {
            augmentation.transform.SetParent(newImageTarget.transform);
        }

        if (behaviour)
        {
            ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
            //ImageTargetBehaviour behaviour = (ImageTargetBehaviour)tracker.GetTargetFinder<TargetFinder>().EnableTracking(targetSearchResult, newImageTarget);


            IEnumerable<TargetFinder> targetfinders = tracker.GetTargetFinders();

            List<TargetFinder> convertedTargetFinders = targetfinders.ToList();

            if (convertedTargetFinders.Count() > 0)

            {

                ImageTargetBehaviour ImageTargetBehaviour = (ImageTargetBehaviour)convertedTargetFinders[0].EnableTracking(targetSearchResult, newImageTarget);

            }

        }

        TargetFinder.CloudRecoSearchResult cloudRecoSearchResult = (TargetFinder.CloudRecoSearchResult)targetSearchResult;
        
        // do something with the target metadata
        mTargetMetadata = cloudRecoSearchResult.MetaData;

        //ChangeTextNColor();
        pvm.GetData(this.mTargetMetadata);

        cloud.CloudRecoEnabled = true;

    }


    
    void OnGUI()
    {
        // Display current 'scanning' status
        GUI.Box(new Rect(100, 100, 200, 50), mIsScanning ? "Scanning" : "Not scanning");
        // Display metadata of latest detected cloud-target
        GUI.Box(new Rect(100, 200, 200, 50), "Metadata: " + mTargetMetadata);
        // If not scanning, show button
        // so that user can restart cloud scanning

    }
    

    // Use this for initialization 
    void Start()
    {

       

        CloudRecoBehaviour cloudReco = GetComponent<CloudRecoBehaviour>();
        if (cloudReco)
        {
            cloudReco.RegisterEventHandler(this);
        }

        cloud = cloudReco;

    }


    private void Update()
    {

        TrackableDistace();
        pvm.updateVisibleRange(distance);
        
    }


    public void TrackableDistace()
    {
        Vector3 delta = Camera.main.transform.position - quad.transform.position;

        distance = delta.magnitude;

        pvm.updateVisibleRange(distance);

        Debug.Log("-------------Distance:" + distance);
    }


    /*
    void ChangeTextNColor()
    {

        if (mTargetMetadata == "0032000016170")
        {
            pvm.GetData(mTargetMetadata);

            

            //textMesh.text = "kelloggs";
            textMesh.text = pvm.brandView.text;
            quadMaterial.color = Color.cyan;
            Debug.Log("----------------Kelloggs-------------------");
            pvm.GetData(this.mTargetMetadata);
        }

        if (mTargetMetadata == "0016000123991")
        {
            pvm.GetData(mTargetMetadata);


            //textMesh.text = "lucky_charms";
            textMesh.text = pvm.brandView.text;
            quadMaterial.color = Color.yellow;
            Debug.Log("----------------Lucky Charms-------------------");
            pvm.GetData(mTargetMetadata);
        }
    }
    */




}