using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Testbed
{
    public class ShoadwRunLightViewTest : MonoBehaviour
    {
        [SerializeField] private float maxSpeed = 50;
        private float nowSpeed = 0;
        [SerializeField] Slider speedSlider;
        [SerializeField] Slider distanceSlider;

        private Vector3 originPos;

        private void Awake()
        {
            originPos = transform.position;
            speedSlider.onValueChanged.AddListener(OnChangeSpeed);
            distanceSlider.onValueChanged.AddListener(OnChangeDistance);

            nowSpeed = 0;
        }

        private void Update()
        {
            this.transform.position += this.transform.forward * (nowSpeed * Time.deltaTime);
        }


        public void SetOriginPos()
        {
            this.transform.position = originPos;
        }

        public void OnChangeDistance(float a)
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, Define.MapLength * a);
        }

        public void OnChangeSpeed(float a)
        {
            Debug.Log(a);
            nowSpeed = maxSpeed * a;
        }

    }


}