using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class MyView : MonoBehaviour {

    private UIScrollView scrollView;
    private UIPanel panel;
    private SpringPanel springPanel;
    private UIGrid grid;
    private GameObject boxPrefab;
    private UILabel title;
    private string boxPrefabPath = "Assets/Prefabs/Box.prefab";
    private UIButton addBtn;
    private UIButton gainBtn;
    private UIButton posBtn;
    private Dictionary<int, Box> boxDict = new Dictionary<int, Box>();

    private int counter;

    void Awake() {
        BindElements();
        
    }

    void Start() {
        title.text = "MyView";
        InitData();
    }

    private void AddData() {
        GameObject GO = AddPrefabToTarget(grid.gameObject, boxPrefab);
        Box box = GO.AddComponent<Box>();
        box.id = counter;
        counter++;
        grid.Reposition();
        boxDict.Add(box.id, box);
    }

    private void InitData() {
        for (int i = 0; i < 10; i++)
            AddData();

    }

    private void GainData() {
        int rand = UnityEngine.Random.Range(0, boxDict.Count);
        if (boxDict.ContainsKey(rand)){
            boxDict[rand].gained = true;
            grid.Reposition();
        }
    }

    /// <summary>
    /// 获取目标控件的位置
    /// </summary>
    private Vector3 GetCenterPosition() {
        Vector3 centerPosition = Vector3.zero;
        Box boxTemp = null;

        foreach (KeyValuePair<int, Box> kvp in boxDict) {
            if (!kvp.Value.gained) {
                if (boxTemp == null) {
                    boxTemp = kvp.Value;
                }else if(kvp.Value.id > boxTemp.id){
                    boxTemp = kvp.Value;
                }
            }
        }

        if (boxTemp != null)
            centerPosition = -boxTemp.transform.localPosition; 

        return centerPosition;
    }

    /// <summary>
    /// 将目标控件移动到尾部需要的位置偏移
    /// </summary>
    private Vector3 GetToTailOffset() {
        int offset = (int)(panel.GetViewSize().x - grid.cellWidth);
        return new Vector3(offset, 0, 0);
    }

    private void SetPos() {
        Vector3 newPos = GetCenterPosition();
        Vector3 offset = GetToTailOffset();
        scrollView.MoveRelative(newPos - scrollView.transform.localPosition + offset);
        scrollView.RestrictWithinBounds(true);
    }

    private GameObject AddPrefabToTarget(GameObject target, GameObject prefab) {
        GameObject GO = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
        GO.transform.SetParent(target.transform);
        return GO;
    }

    private int GridSort(Transform a, Transform b) {
        Box boxA = a.GetComponent<Box>();
        Box boxB = b.GetComponent<Box>();
        if (boxA.gained && !boxB.gained) return -1;
        else if (!boxA.gained && boxB.gained) return 1;
        else
        {
            if (boxA.id < boxB.id) return 1;
            else if (boxA.id == boxB.id) return 0;
            else return -1;
        }
    }

    private void BindElements() {
        scrollView = gameObject.transform.FindChild("ScrollView").GetComponent<UIScrollView>();
        panel = gameObject.transform.FindChild("ScrollView").GetComponent<UIPanel>();
        springPanel = gameObject.transform.FindChild("ScrollView").GetComponent<SpringPanel>();

        grid = gameObject.transform.FindChild("ScrollView/Grid").GetComponent<UIGrid>();
        grid.onCustomSort = GridSort;
        title = gameObject.transform.FindChild("Title/Label").GetComponent<UILabel>();
        boxPrefab = Resources.LoadAssetAtPath(boxPrefabPath, typeof(GameObject)) as GameObject;
        addBtn = gameObject.transform.FindChild("AddBtn").GetComponent<UIButton>();
        EventDelegate.Add(addBtn.onClick, AddData);
        gainBtn = gameObject.transform.FindChild("GainBtn").GetComponent<UIButton>();
        EventDelegate.Add(gainBtn.onClick, GainData);
        posBtn = gameObject.transform.FindChild("PosBtn").GetComponent<UIButton>();
        EventDelegate.Add(posBtn.onClick, SetPos);
    }
}
