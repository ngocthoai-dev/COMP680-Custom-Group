using Core.Business;
using Core.EventSignal;
using UnityEngine;
using Zenject;

namespace Core
{
    public class SoundData
    {
        public string Path;
        public float? Volume = 1;
        public float? Pitch = 1;
    }

    public class SoundDataLoader
    {
        private readonly Business.ILogger _logger;
        private readonly SignalBus _signalBus;

        public SoundDataLoader(
            Business.ILogger logger,
            SignalBus signalBus)
        {
            _logger = logger;
            _signalBus = signalBus;
        }

        public void FireOneShotNormalyAudioAsRandom(SoundData audioData)
        {
            _signalBus.Fire(
                new PlayOneShotAudioSignal(
                    audioData,
                    AudioMixerType.Sfx)
                .SetupPosition<PlayOneShotAudioSignal>(Vector3.zero));
        }

        public void FireOneShotNormalyAudioAsRandom(string path)
        {
            _signalBus.Fire(
                new PlayOneShotAudioSignal(
                    path,
                    AudioMixerType.Sfx)
                .SetupPosition<PlayOneShotAudioSignal>(Vector3.zero));
        }
    }
}