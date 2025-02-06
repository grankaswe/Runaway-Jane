using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class HUD : MonoBehaviour
{
    VisualElement _root;

    private void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
    }

    void Start()
    {
        TextField titleField = _root.Q<TextField>("TitleField");
        UnsignedIntegerField valueField = _root.Q<UnsignedIntegerField>("ValueField");
        ProgressBar progressBar = _root.Q<ProgressBar>("Defalut");
        valueField.RegisterCallback<NavigationSubmitEvent>(evt => progressBar.value = valueField.value);

        titleField.RegisterCallback<ChangeEvent<string>>(x => progressBar.title = x.newValue);
        valueField.RegisterCallback<ChangeEvent<uint>>(x => progressBar.value = x.newValue);
        TextField infoField = _root.Q<TextField>("infoField");
        Label Textinfo = _root.Q<Label>("Textinfo");
        infoField.RegisterCallback<NavigationSubmitEvent>(evt => Textinfo.text = infoField.value);
    }

    void Update()
    {
        
    }
}
