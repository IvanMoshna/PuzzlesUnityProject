using Beetles.Entities;
using UnityEngine;

namespace Beetles.Behavior {

    public class Behavior : MonoBehaviour{

        private void Awake() {
            Setup();
        }

        private void OnEnable() {
            GetComponent<Entity>().Signals.Add(this);
        }

        private void OnDisable() {
            GetComponent<Entity>().Signals.Remove(this);
        }

        protected virtual void Setup() {}

    }

}