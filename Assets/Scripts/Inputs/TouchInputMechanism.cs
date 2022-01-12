using UnityEngine;

namespace OneDevApp.WebConfiguratorDemo
{
    public class TouchInputMechanism : BaseInputMechanism
    {
        private int _trackedTouchId = -1;
        private Vector2 _touchPosition;

        #region Overrides of InputMechanism
        public override bool IsDown()
        {
            if (_trackedTouchId < 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    if (Input.GetTouch(i).phase == TouchPhase.Began)
                    {
                        _trackedTouchId = i;
                        return true;
                    }
                }
            }

            return false;
        }

        public override bool IsUp()
        {
            if (_trackedTouchId >= 0)
            {
                switch (Input.GetTouch(_trackedTouchId).phase)
                {
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        _trackedTouchId = -1;
                        return true;
                }
            }

            return false;
        }

        public override Vector2 GetScreenPosition()
        {
            if (_trackedTouchId >= 0)
            {
                _touchPosition = Input.GetTouch(_trackedTouchId).position;
            }

            return _touchPosition;
        }

        public override float GetZoomValue()
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPreviousPosition = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePreviousPosition = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPreviousPosition - touchOnePreviousPosition).magnitude;

            float TouchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagDiff = prevTouchDeltaMag - TouchDeltaMag;

            return -deltaMagDiff * 0.0025f;
        }

        public override Vector2 GetRotateValues()
        {
            Vector2 touchposition = Input.GetTouch(0).deltaPosition;

            return new Vector2(touchposition.x * 0.025f, touchposition.y * 0.025f);
        }

        public override Vector2 GetPanValues()
        {
            return Vector2.zero;
        }

        public override bool IsRotating()
        {
            //return (IsDown() && Input.GetTouch(_trackedTouchId).phase == TouchPhase.Moved);
            return (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved);
        }

        public override bool IsPanning()
        {
            return false;
        }

        public override bool IsZooming()
        {
            //return (IsDown() && Input.touchCount == 2);
            return (Input.touchCount == 2);
        }
        #endregion
    }

}