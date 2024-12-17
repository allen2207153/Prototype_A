
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Keto
{
    public class DissolveTest : MonoBehaviour
    {
        public float DissolveSpeed = 0.01f;
        public float DissolveYield = 0.1f;

        public ParticleSystem Particle = null;

        private const string DISSOVE_AMOUNT = "_DissolveAmount";

        private SkinnedMeshRenderer[] m_skinnedMeshRenderers = null;
        private List<Material> m_materials = new List<Material>();

        private float m_dissolveStart = -0.2f;
        private float m_dissolveEnd = 1.2f;

        private void Awake()
        {
            m_skinnedMeshRenderers = this.GetComponentsInChildren<SkinnedMeshRenderer>();
            for (int i = 0; i < m_skinnedMeshRenderers.Length; i++)
            {
                for (int j = 0; j < m_skinnedMeshRenderers.Length; j++)
                {
                    m_materials.Add(m_skinnedMeshRenderers[j].material);
                }
            }
        }
        public void Reset()
        {
            StopAllCoroutines();
            foreach (Material matertial in m_materials)
            {
                matertial.SetFloat(DISSOVE_AMOUNT, m_dissolveStart);
            }
        }

        public void Dissolve()
        {
            StartCoroutine(DissolveCoroutine());
        }

        private IEnumerator DissolveCoroutine()
        {
            if (Particle != null)
            {
                Particle.Play();
            }

            if (m_materials.Count > 0)
            {
                float dissovleAmount = m_dissolveStart;
                float speedMulti = 1f;
                while (dissovleAmount < m_dissolveEnd)
                {
                    dissovleAmount += DissolveSpeed * speedMulti;
                    speedMulti += 0.1f;
                    foreach (Material matertial in m_materials)
                    {
                        matertial.SetFloat(DISSOVE_AMOUNT, dissovleAmount);
                    }
                    yield return new WaitForSeconds(DissolveYield);
                }
            }
        }
    }
}
