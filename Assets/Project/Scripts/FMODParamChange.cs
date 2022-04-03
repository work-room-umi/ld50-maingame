using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace umi.ld50
{
    public class FMODParamChange : MonoBehaviour
    {
        public FMODUnity.StudioEventEmitter eventEmitter;
        public string eventName;
        public void SetTime(float value)
        {
            eventEmitter.EventInstance.setParameterByName(eventName, value);
        }
    }
}
