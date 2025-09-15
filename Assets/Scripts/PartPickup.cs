using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartPickup : MonoBehaviour
{
    

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("SCRAP COLLECTED");
            PartsCollection.currentScrap++;
            PartsCollection.onPartsCollected?.Invoke();
            Destroy(this.gameObject);
            //you destroy this object, add on to the part collection mission, and other things if the mission is finished
        }
    }
}
