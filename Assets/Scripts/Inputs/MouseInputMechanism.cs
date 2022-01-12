using UnityEngine;

namespace OneDevApp.WebConfiguratorDemo
{
    public class MouseInputMechanism : BaseInputMechanism
    {
        public int Button = 0;

        #region Overrides of InputMechanism
        public override bool IsDown()
        {
            return Input.GetMouseButtonDown(Button);
        }

        public override bool IsUp()
        {
            return Input.GetMouseButtonUp(Button);
        }

        public override Vector2 GetScreenPosition()
        {
            return Input.mousePosition;
        }

        public override float GetZoomValue()
        {
            return Input.GetAxis("Mouse ScrollWheel");
        }

        public override Vector2 GetRotateValues()
        {
            return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }

        public override Vector2 GetPanValues()
        {
            return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }

        public override bool IsRotating()
        {
            return Input.GetMouseButton(0);
        }

        public override bool IsPanning()
        {
            return Input.GetMouseButton(1);
        }

        public override bool IsZooming()
        {
            return Input.GetAxis("Mouse ScrollWheel") != 0;
        }
        #endregion
    }


}