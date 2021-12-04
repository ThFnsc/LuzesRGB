using LuzesRGB.Services.Lights;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace LuzesRGB.Services
{
    public class AudioToColorService : IColorizable, IDisposable
    {
        private Color _lastColor;
        private readonly IAudioProvider _audioProvider;
        private readonly IAudioToColorConverter _audioToColorConverter;

        public event EventHandler<Color> OnColorChanged;
        public event EventHandler<float[]> OnAudioData;

        public byte BrightnessCap { get; set; } = 255;

        public List<ISmartLight> SmartLights { get; set; }

        public AudioToColorService(IAudioProvider audioProvider, IAudioToColorConverter audioToColorConverter)
        {
            SmartLights = new List<ISmartLight>();

            _audioProvider = audioProvider;
            _audioToColorConverter = audioToColorConverter;

            _audioToColorConverter.OnColorAvailable += GotColor;
            audioProvider.OnAudioData += AudioProvider_OnAudioData;
        }

        private void AudioProvider_OnAudioData(object sender, float[] spectrum)
        {
            _audioToColorConverter.NewSpectrum(this, spectrum);
            OnAudioData?.Invoke(this, spectrum);
        }

        public Task Start() => _audioProvider.Start();

        public Task Stop() => _audioProvider.Stop();

        public async Task ConnectAll() =>
            await Task.WhenAll(SmartLights.Select(s => s.Connect()));

        public async Task RemoveAll()
        {
            var lights = SmartLights.ToList();
            SmartLights.Clear();
            await Task.WhenAll(lights.Select(s => s.Turn(false)));
            SmartLights.OfType<IDisposable>().Each(s => s.Dispose());
        }

        private void GotColor(object sender, Color e) =>
            _ = SetColor(e.SetMaxBrightness(BrightnessCap));

        public Task<Color> GetColor() =>
            Task.FromResult(_lastColor);

        public async Task SetColor(Color color)
        {
            _lastColor = color;
            OnColorChanged?.Invoke(this, color);
            await Task.WhenAll(SmartLights.Select(s => s.SetColor(color)));
        }

        public void Dispose() =>
            Task.Run(async () =>
            {
                _audioProvider?.Dispose();
                await RemoveAll();
            }).Wait();
    }
}
