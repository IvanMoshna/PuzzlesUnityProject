using Common;
using UnityEngine;

namespace Beetles.Controllers
{
    public class Controller : MonoBehaviour
    {
        public bool IsReceiver = true;

        protected virtual void OnEnable()
        {
            if (IsReceiver)
            {
                ToolBox.Signals.Add(this);
            }
        }

        private void OnDisable()
        {
            if (IsReceiver)
            {
                ToolBox.Signals.Remove(this);
            }
        }
    }
}