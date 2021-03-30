using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DapanzAI.UI
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager instance;
        public UIAssetsConfig uiList;
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

        private int count = 0;

        public void ShowModule(string _name)
        {
            GameObject obj = uiList.GetUIByName(_name);
            UIbase ui = obj.GetComponent<UIbase>();
            ui.OnShow();
        }

        public void UnloadModule(string _name)
        {

        }
    }
}
