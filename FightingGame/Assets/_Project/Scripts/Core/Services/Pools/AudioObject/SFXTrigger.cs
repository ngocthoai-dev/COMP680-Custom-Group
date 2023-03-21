using Core.Business;
using Core.EventSignal;
using UnityEngine;
using Zenject;

namespace Core
{
    public class SFXTrigger : MonoBehaviour
    {
        [SerializeField] private string _audioPath;

        private SignalBus _signalBus;

        [Inject]
        public void Construct(
            SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void Trigger(SignalBus signalBus)
        {
            if (signalBus == null) return;

            _signalBus.Fire(new PlayOneShotAudioSignal(_audioPath, AudioMixerType.Sfx)
                .SetupPosition<PlayOneShotAudioSignal>(transform.position));
        }
    }
}