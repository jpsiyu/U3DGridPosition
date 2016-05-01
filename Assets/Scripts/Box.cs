using UnityEngine;
using System.Collections;

public class Box : MonoBehaviour{

    public int id;
    public bool gained;

    private string gainedStr = "Gained";
    private string unGainedStr = "UnGained";

    private UILabel label;
    private UILabel label2;

    void Awake() {
        BindElements();
    }

    void Update() {
        string statusStr = gained ? gainedStr : unGainedStr;
        label.text = id.ToString();
        label2.text = statusStr;

    }

    private void BindElements() {
        label = gameObject.transform.FindChild("Label").GetComponent<UILabel>();
        label2 = gameObject.transform.FindChild("Label2").GetComponent<UILabel>();
    }

}
