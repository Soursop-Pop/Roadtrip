using System.Collections;
using UnityEngine;

namespace Urpixel
{
    public class UrpixelVideoMovePlayer : MonoBehaviour
    {
        private Camera _cam;
        private Vector3 _moveDirection;
        private Vector3 _right;
        private float _moveSpeed = 2.1f;

        private int inputIndex;

        private void Start()
        {
            _cam = Camera.main;

            StartCoroutine(Inputs());
        }

        private void Update()
        {
            MovePlayer();
            RotateCamera();
        }

        private void MovePlayer()
        {
            _moveDirection = Vector3.zero;

            switch (inputIndex)
            {
                case 0:
                    _moveDirection = Vector3.right;
                    break;

                case 1:
                    _moveDirection += Vector3.forward;
                    break;
            }

            transform.position += _moveDirection.normalized * (_moveSpeed * Time.deltaTime);
        }

        private void RotateCamera()
        {
            //if(inputIndex == 1)

        }

        private IEnumerator Inputs()
        {
            float waitCount = 0f;
            inputIndex = 0;

            while (waitCount < 2f)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                waitCount += Time.deltaTime;
            }

            waitCount = 0f;
            inputIndex = 1;
            float rotation;

            while (waitCount < 2f)
            {
                float t = waitCount / 2f;
                rotation = Mathf.Lerp(45f, -45f, t);
                _cam.transform.eulerAngles = new Vector3(30f, rotation, 0f);
                yield return new WaitForSeconds(Time.deltaTime);
                waitCount += Time.deltaTime;
            }

            inputIndex = 0;
        }
    }
}