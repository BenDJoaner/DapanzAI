using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

namespace DapanzAI.UI
{
    public interface IUIBase
    {

    }
    public class UIbase:MonoBehaviour
    {
        public virtual void OnShow(){}
        public virtual void OnClose(){}
    }
}
