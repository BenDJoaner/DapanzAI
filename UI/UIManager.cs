using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DapanzAI.UI
{
    public enum CavansLayer
    {
        MESH,
        TOP
    }
    public class UIManager : MonoBehaviour
    {
        private static UIManager instance;
        public UIAssetsConfig uiList;
        public Canvas TopCanvas;
        public Canvas MeshCanvas;

        private Dictionary<string, UIbase> uiOpened;
        public static UIManager Instance
        {
            get
            {
                if (instance != null)
                    return instance;

                instance = FindObjectOfType<UIManager>();

                if (instance != null)
                    return instance;

                GameObject ItemManager = new GameObject("UIManager");
                instance = ItemManager.AddComponent<UIManager>();

                return instance;
            }
        }

        public void ShowModule(string _name, CavansLayer _layer = CavansLayer.MESH)
        {
            uiOpened.TryGetValue(_name, out UIbase ui);
            if (ui)
            {
                ui.gameObject.SetActive(true);
                ui.Resume();
            }
            else
            {
                Canvas targetLayer = _layer == CavansLayer.MESH ? MeshCanvas : TopCanvas;
                GameObject preobj = uiList.GetUIByName(_name);
                if (preobj)
                {
                    GameObject ui_obj = Instantiate(preobj, targetLayer.transform);
                    ui = ui_obj.GetComponent<UIbase>();
                    uiOpened.Add(_name, ui);
                    ui.OnShow();
                }
                else
                {
                    Debug.LogErrorFormat($"没找到[{_name}]！请把预制体添加到UIConfig中");
                }
            }
        }

        public void UnloadModule(string _name)
        {
            uiOpened.TryGetValue(_name, out UIbase ui);
            if (ui)
            {
                ui.OnClose();
                uiOpened.Remove(_name);
                Destroy(ui.gameObject);
            }
            else
            {
                Debug.LogErrorFormat($"关闭失败！没有找到打开中的:[{_name}]！");
            }
        }

        public void HideModule(string _name)
        {
            uiOpened.TryGetValue(_name, out UIbase ui);
            if (ui)
            {
                ui.OnHide();
                ui.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogErrorFormat($"隐藏失败！没有找到打开中的:[{_name}]！");
            }
        }

        public void UnloadAll()
        {
            foreach(var info in uiOpened)
            {
                UnloadModule(info.Key);
            }
        }

        public void HideAll()
        {
            foreach (var info in uiOpened)
            {
                if (info.Value.gameObject.activeSelf)
                {
                    HideModule(info.Key);
                }
            }
        }

        public void ResumeAll()
        {
            foreach (var info in uiOpened)
            {
                if (!info.Value.gameObject.activeSelf)
                {
                    ShowModule(info.Key);
                }
            }
        }
    }
}
