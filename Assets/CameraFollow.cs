using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NameSpaceName {

    public class CameraFollow : MonoBehaviour
    {

        public Transform target;

	public float smoothSpeed = 0.125f;
        Vector3 velocity;

        public Vector3 offset;

        private void Start()
        {
            offset = transform.position - target.position;
        }
        void LateUpdate()
        {
            Vector3 desiredPosition = target.position + offset;
          //  Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
          //  transform.position = smoothedPosition;
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);

            transform.LookAt(target);
          //  transform.eulerAngles = new Vector3(36.0f,target.eulerAngles.y, 0);
        }

    }
}
