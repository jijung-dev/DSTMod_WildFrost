using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deadpan.Enums.Engine.Components.Modding;
using UnityEngine;

namespace DSTMod_WildFrost
{
    public class StatusEffectWetness : StatusEffectData
    {
        public override bool RunPreAttackEvent(Hit hit)
        {
            var status = hit.attacker?.FindStatus(DSTMod.Instance.TryGet<StatusEffectData>("Wetness"));
            if ((bool)status)
            {
                float probability = status.count / 10f;
                if (UnityEngine.Random.value < probability)
                {
                    VFXHelper.SFX.TryPlaySound("Wetness_Slip");
                    hit.nullified = true;
                }
            }
            return true;
        }

        public override bool RunApplyStatusEvent(StatusEffectApply apply)
        {
            if ((bool)apply.effectData && apply.target == target && target.data.IsItem)
            {
                var status = apply.target.FindStatus(DSTMod.Instance.TryGet<StatusEffectData>("Wetness"));
                if ((bool)status && status.count + apply.count >= 10)
                {
                    //apply.effectData = null;
                    apply.count = 10 - status.count;
                }
            }

            return false;
        }
    }
}
