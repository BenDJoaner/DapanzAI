using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DapanzAI.UI
{
    [CreateAssetMenu(menuName = "UI配置集合", fileName = "UIConfig")]
    [Serializable]
    public class UIAssetsConfig: ScriptableObject
    {
        public Dictionary<string, GameObject> m_UIMaps;

        [SerializeField]
        public List<GameObject> m_UIList;

        private void OnEnable()
        {
#if UNITY_EDITOR
            m_UIMaps = new Dictionary<string, UI>();
#else
            m_UIMaps = new Dictionary<string, GameObject>(m_UIList.Count);
#endif
            if (m_UIList != null)
            {
                foreach (var obj in m_UIList)
                {
                    if (obj == null)
                    {
#if UNITY_EDITOR
                        Debug.LogErrorFormat(this, $"[{name}]UIAssets中有UI为空！");
#else
                        Debug.LogErrorFormat($"[{name}]UIAssets中有UI为空！");
#endif
                        continue;
                    }

                    if (m_UIMaps.ContainsKey(obj.name))
                    {
#if UNITY_EDITOR
                        Debug.LogErrorFormat(this, $"[{name}]UIAssets中已存在名叫[{obj.name}]的UI！");
#else
                        Debug.LogErrorFormat($"[{name}]UIAssets中已存在名叫[{obj.name}]的UI！");
#endif
                        continue;
                    }
                    m_UIMaps.Add(obj.name, obj);
                }
            }
        }

        public void AddUIList(List<GameObject> objList)
        {
            if (m_UIList == null)
            {
                m_UIList = new List<GameObject>();
            }
            m_UIList.AddRange(objList);
        }

        public GameObject GetUIByIndex(int index)
        {
            return m_UIList[index];
        }

        public GameObject GetUIByName(string name)
        {
            GameObject obj;
            if (m_UIMaps.TryGetValue(name, out obj))
            {
                return obj;
            }
            return null;
        }
    }
}
