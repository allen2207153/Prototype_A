using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Keto
{
    public class Tester : MonoBehaviour
    {
        public GameObject SampleA = null;
        public GameObject SampleB = null;
        public GameObject SampleC = null;
        public GameObject SampleD = null;
        public GameObject SampleE = null;
        private void Start()
        {
            SampleA.SetActive(true);
            SampleB.SetActive(false);
            SampleC.SetActive(false);
            SampleD.SetActive(false);
            SampleE.SetActive(false);

            SampleA.GetComponent<DissolveTest>().Reset();
        }

        void OnGUI()
        {
            if (GUI.Button(new Rect(150, 100, 150, 130), "SampleA"))
            {
                SampleA.SetActive(true);
                SampleB.SetActive(false);
                SampleC.SetActive(false);
                SampleD.SetActive(false);
                SampleE.SetActive(false);
                SampleA.GetComponent<DissolveTest>().Reset();
                SampleA.GetComponent<DissolveTest>().Dissolve();
            }

            if (GUI.Button(new Rect(150, 300, 150, 130), "SampleB"))
            {
                SampleA.SetActive(false);
                SampleB.SetActive(true);
                SampleC.SetActive(false);
                SampleD.SetActive(false);
                SampleE.SetActive(false);
                SampleB.GetComponent<DissolveTest>().Reset();
                SampleB.GetComponent<DissolveTest>().Dissolve();
            }

            if (GUI.Button(new Rect(150, 500, 150, 130), "SampleC"))
            {
                SampleA.SetActive(false);
                SampleB.SetActive(false);
                SampleC.SetActive(true);
                SampleD.SetActive(false);
                SampleE.SetActive(false);
                SampleC.GetComponent<DissolveTest>().Reset();
                SampleC.GetComponent<DissolveTest>().Dissolve();
            }

            if (GUI.Button(new Rect(150, 700, 150, 130), "SampleD"))
            {
                SampleA.SetActive(false);
                SampleB.SetActive(false);
                SampleC.SetActive(false);
                SampleD.SetActive(true);
                SampleE.SetActive(false);
                SampleD.GetComponent<DissolveTest>().Reset();
                SampleD.GetComponent<DissolveTest>().Dissolve();
            }

            if (GUI.Button(new Rect(150, 900, 150, 130), "SampleE"))
            {
                SampleA.SetActive(false);
                SampleB.SetActive(false);
                SampleC.SetActive(false);
                SampleD.SetActive(false);
                SampleE.SetActive(true);
                SampleE.GetComponent<DissolveTest>().Reset();
                SampleE.GetComponent<DissolveTest>().Dissolve();
            }
        }
    }
}
