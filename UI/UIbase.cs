using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

namespace DapanzAI.UI
{
    public class UIbase:MonoBehaviour
    {
        public virtual void OnShow() { }
        public virtual void OnClose() { }
        public virtual void OnHide() { }
        public virtual void Resume() { }
    }
}
