using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneDevApp.WebConfiguratorDemo
{
    public abstract class BaseInputMechanism
    {
        public abstract bool IsDown();
        public abstract bool IsUp();
        public abstract Vector2 GetScreenPosition();

        public abstract bool IsRotating();
        public abstract bool IsPanning();
        public abstract bool IsZooming();
        public abstract float GetZoomValue();
        public abstract Vector2 GetRotateValues();
        public abstract Vector2 GetPanValues();
    }

}