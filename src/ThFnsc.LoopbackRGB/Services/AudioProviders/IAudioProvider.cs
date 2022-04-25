namespace ThFnsc.LoopbackRGB.Services.AudioProviders;
public interface IAudioProvider
{
    void Start();

    event EventHandler<float[]> OnSamplesAvailable;
}
