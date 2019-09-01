using System;
using System.Collections;
using UnityEngine;

namespace Util.CoroutineHandler
{
    public class WaiUntil : CustomYieldInstruction
    {
        private readonly Func<bool> m_Predicate;

        public WaiUntil(Func<bool> predicate)
        {
            m_Predicate = predicate;
        }

        public override bool keepWaiting => !m_Predicate();
    }
}