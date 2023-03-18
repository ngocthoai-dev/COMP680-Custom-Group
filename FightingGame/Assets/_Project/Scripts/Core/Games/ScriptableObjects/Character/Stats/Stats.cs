using Core.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Core.SO
{
    [Serializable]
    public struct ItemStats
    {
        [DebugOnly] public SerializableDictionary<StatType, Stat> Stats;

        public void AddItemStats(ItemStats stats, Dictionary<StatType, float> maxValues)
        {
            for (int i = 0; i < (int)StatType.Total; i++)
            {
                StatType t = StatType.HP + i;
                if (stats.Stats.ContainsKey(t))
                    Stats[t].AddBaseStat(stats.Stats[t], maxValues.ContainsKey(t) ? maxValues[t] : float.MaxValue);
            }
        }

        public void AddModifier(StatType type, IStatModifier mod)
        {
            Stats[type].AddModifier(mod);
        }

        public Stat GetStats(StatType type)
        {
            return Stats[type];
        }

        public void ApplyStats(SerializableDictionary<StatType, int> statsLvl,
            SerializableDictionary<StatType, List<float>> defaultStats)
        {
            Stats = new();
            foreach (var pair in statsLvl)
                Stats.Add(pair.Key, new Stat()
                {
                    BaseValue = defaultStats[pair.Key][pair.Value]
                });
        }

        public ItemStats Duplicate()
        {
            ItemStats newItem = new()
            {
                Stats = new()
            };
            foreach (var pair in Stats)
                newItem.Stats.Add(pair.Key, new Stat()
                {
                    BaseValue = pair.Value.BaseValue
                });
            return newItem;
        }
    }

    [Serializable]
    public class Stat : IComparable
    {
        public float BaseValue;

        [JsonIgnore]
        private float _permanentlyAdded;

        [JsonIgnore]
        private float _permanentlyReduced;

        [JsonIgnore]
        private readonly List<IStatModifier> _modifiers = new();

        [JsonIgnore]
        public float Value
        {
            get
            {
                float finalValue = BaseValue;
                _modifiers.ForEach(x => finalValue += x.ApplyModify(finalValue));
                return finalValue + _permanentlyAdded - _permanentlyReduced;
            }
        }

        public float ValueWithoutPermanentlyParams()
        {
            float finalValue = BaseValue;
            _modifiers.ForEach(x => finalValue += x.ApplyModify(finalValue));
            return finalValue;
        }

        public void AddModifier(IStatModifier modifier)
        {
            if (modifier != null)
                _modifiers.Add(modifier);
        }

        public void RemoveModifier(IStatModifier modifier)
        {
            if (modifier != null)
                _modifiers.Remove(modifier);
        }

        public void AddBaseStat(Stat anotherStat, float maxValue)
        {
            BaseValue += anotherStat.Value;
            if (BaseValue > maxValue)
                BaseValue = maxValue;
        }

        public void AddPermanentlyAmount(float amount)
        {
            _permanentlyAdded += amount;
        }

        public void ReducePermanentlyAmount(float amount)
        {
            _permanentlyReduced += amount;
        }

        public int CompareTo(object obj)
        {
            Stat bObj = (Stat)obj;

            if (bObj.Value > Value)
            {
                return 1;
            }
            else if (bObj.Value == Value)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}