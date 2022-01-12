using UnityEngine;

namespace OneDevApp.WebConfiguratorDemo
{
    public class WebConfiguratorCamera : MonoBehaviour
    {

        public Transform camTarget;
        public Vector3 targetOffset;
        public float distance = 5.0f;
        public float maxDistance = 20;
        public float minDistance = .6f;
        public float xSpeed = 200.0f;
        public float ySpeed = 200.0f;
        public int yMinLimit = -80;
        public int yMaxLimit = 80;
        public int zoomRate = 40;
        public float panSpeed = 0.3f;
        public float zoomDampening = 5.0f;

        private float xDeg = 0.0f;
        private float yDeg = 0.0f;
        private float currentDistance;
        private float desiredDistance;
        private Quaternion currentRotation;
        private Quaternion desiredRotation;
        private Quaternion rotation;
        private Vector3 position;

        public bool IsCameraFreezed { get; set; } = false;
        public bool IsInputBlocked { get; set; } = false;
        public bool IsZoomBlocked { get; set; } = false;

        private BaseInputMechanism baseInput;

        private void Awake()
        {

#if UNITY_EDITOR
            baseInput = new MouseInputMechanism();
#elif UNITY_ANDROID || UNITY_IOS
            baseInput = new TouchInputMechanism();
#else
            baseInput = new MouseInputMechanism();
#endif
        }

        private void Start()
        {
            Init();
        }


        public void Init()
        {
            //If there is no target, create a temporary target at 'distance' from the cameras current viewpoint
            if (!camTarget)
            {
                GameObject go = new GameObject("Cam Target");
                go.transform.position = transform.position + (transform.forward * distance);
                camTarget = go.transform;
            }

            distance = Vector3.Distance(transform.position, camTarget.position);
            currentDistance = distance;
            desiredDistance = distance;

            //be sure to grab the current rotations as starting points.
            position = transform.position;
            rotation = transform.rotation;
            currentRotation = transform.rotation;
            desiredRotation = transform.rotation;

            xDeg = Vector3.Angle(Vector3.right, transform.right);
            yDeg = Vector3.Angle(Vector3.up, transform.up);
        }

        private void Update()
        {
            if (!IsInputBlocked)
            {
                if (baseInput.IsRotating())
                {
                    OnRotateInputEvent(baseInput.GetRotateValues());
                }

                if (baseInput.IsPanning())
                {
                    OnPanInputEvent(baseInput.GetPanValues());
                }

                if (baseInput.IsZooming() && !IsZoomBlocked)
                {
                    OnZoomInputEvent(baseInput.GetZoomValue());
                }
            }
        }

        private void LateUpdate()
        {
            if (!IsCameraFreezed)
            {
                /////////OrbitAngle

                //Clamp the vertical axis for the orbit
                yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);

                // set camera rotation 
                desiredRotation = Quaternion.Euler(yDeg, xDeg, 0);

                currentRotation = transform.rotation;

                rotation = Quaternion.Lerp(currentRotation, desiredRotation, Time.deltaTime * zoomDampening);
                transform.rotation = rotation;

                //clamp the zoom min/max
                desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
                // For smoothing of the zoom, lerp distance
                currentDistance = Mathf.Lerp(currentDistance, desiredDistance, Time.deltaTime * zoomDampening);

                // calculate position based on the new currentDistance 
                position = camTarget.position - (rotation * Vector3.forward * currentDistance + targetOffset);
                transform.position = position;
            }
        }

        private float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360)
                angle += 360;
            if (angle > 360)
                angle -= 360;
            return Mathf.Clamp(angle, min, max);
        }

        public void SetCameraLookAtPos(Vector3 rotationOffset)
        {
            xDeg = rotationOffset.y;
            //Clamp the vertical axis for the orbit
            yDeg = ClampAngle(rotationOffset.x, yMinLimit, yMaxLimit);

            desiredDistance = -1;
        }

        private void OnPanInputEvent(Vector2 panValue)
        {
            /////////OrbitMove

            //grab the rotation of the camera so we can move in a psuedo local XY space
            camTarget.Translate(Vector3.right * -panValue.x * panSpeed);
            camTarget.Translate(transform.up * -panValue.y * panSpeed, Space.World);
        }

        private void OnRotateInputEvent(Vector2 rotateValue)
        {
            xDeg += rotateValue.x * xSpeed * 0.02f;
            yDeg -= rotateValue.y * ySpeed * 0.02f;
        }

        private void OnZoomInputEvent(float zoomValue)
        {
            /////////Orbit Position

            // affect the desired Zoom distance if we roll the scrollwheel
            desiredDistance -= zoomValue * Time.deltaTime * zoomRate * Mathf.Abs(desiredDistance);
        }
    }

}