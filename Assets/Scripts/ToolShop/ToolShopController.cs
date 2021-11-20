using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolShopController : MonoBehaviour
{
    public LinkedDataSO LD;
    public GameObject ui_prefab;
    public Transform content;


    Transform _current_selected;
    Transform current_selected
    {
        get { return _current_selected; }
        set
        {
            if (_current_selected)
            {
                _current_selected.gameObject.SetActive(false);
            }
            _current_selected = value;
            _current_selected.gameObject.SetActive(true);
        }
    }


    private void Start()
    {
        

        foreach (Transform item in content)
        {
            Destroy(item.gameObject);
        }

        foreach (KeyValuePair<string,GameObject> key_item in LD.tools)
        {
            GameObject item = key_item.Value;
            GameObject ui_obj = Instantiate(ui_prefab);

            ui_obj.transform.SetParent(content);
            ui_obj.transform.localScale = Vector3.one;
            ToolItemUI ui = ui_obj.GetComponent<ToolItemUI>();
            ui.icon.sprite = item.GetComponentInChildren<SpriteRenderer>().sprite;

            ui.locked.gameObject.SetActive(false);
            ui.button.enabled = false;
            ui.used.gameObject.SetActive(false);

            if (LD.IsToolPurchase(key_item.Key))
            {
                ui.button.enabled = true;
                ui.button.onClick.AddListener(() =>
                {
                    current_selected = ui.used;
                    LD.SetCurrentTool(key_item.Key);
                });
            }
            else
            {
                ui.locked.gameObject.SetActive(true);
            }

        }
    }
}
