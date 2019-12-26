using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class Clouddatabase : MonoBehaviour , IObjectRecoEventHandler
{
    private CloudRecoBehaviour mCloudRecoBehaviour;
        private bool mIsScanning = false;

        private string mTargetMetadata = "";
        private IObjectRecoEventHandler _objectRecoEventHandlerImplementation;

        //initialization 
        void Start()
        {
            // registering this event handler at the cloud reco behaviour 
            mCloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();

            if (mCloudRecoBehaviour)
            {
                mCloudRecoBehaviour.RegisterEventHandler(this);
            }
        }

        public void OnInitialized(TargetFinder targetFinder)
        {
            Debug.Log ("Cloud Reco initialized");
        }

        public void OnInitError(TargetFinder.InitState initError)
        {
            Debug.Log ("Cloud Reco init error " + initError.ToString());
        }

        public void OnUpdateError(TargetFinder.UpdateState updateError)
        {
            Debug.Log ("Cloud Reco update error " + updateError.ToString());
        }

        public void OnStateChanged(bool scanning) {
            mIsScanning = scanning;
            if (scanning)
            {
                // clearing all known trackables
                var tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
                tracker.GetTargetFinder<ImageTargetFinder>().ClearTrackables(false);
            }
        }
        public ImageTargetBehaviour ImageTargetTemplate;
        public void OnNewSearchResult(TargetFinder.TargetSearchResult targetSearchResult) {
            
            TargetFinder.CloudRecoSearchResult cloudRecoSearchResult = 
                (TargetFinder.CloudRecoSearchResult)targetSearchResult;
            // doing the task with the target metadata
            mTargetMetadata = cloudRecoSearchResult.MetaData;
            // stopping the target finder (i.e. stop scanning the cloud)
            mCloudRecoBehaviour.CloudRecoEnabled = false;
            if (ImageTargetTemplate) { 
                // enabling the new result with the same ImageTargetBehaviour: 
                ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>(); 
                tracker.GetTargetFinder<ImageTargetFinder>().EnableTracking(targetSearchResult, ImageTargetTemplate.gameObject); 
            }
        }
        void OnGUI() {
            // Displaying current 'scanning' status
            GUI.Box (new Rect(100,100,200,50), mIsScanning ? "Scanning" : "Not scanning");
            // Displaying metadata of latest detected cloud-target
            GUI.Box (new Rect(100,200,200,50), "Metadata: " + mTargetMetadata);
            // If not scanning, show button, so that user can restart cloud scanning
            if (!mIsScanning) {
                if (GUI.Button(new Rect(100,300,200,50), "Restart Scanning")) {
                    // Restart TargetFinder
                    mCloudRecoBehaviour.CloudRecoEnabled = true;
                }
            }
        }        
}


