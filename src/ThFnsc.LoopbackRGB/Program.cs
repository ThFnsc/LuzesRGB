using ThFnsc.LoopbackRGB;
using ThFnsc.LoopbackRGB.Services.AudioProviders;
using ThFnsc.LoopbackRGB.Services.ColorProcessors;
using ThFnsc.LoopbackRGB.Services.Devices;
using ThFnsc.LoopbackRGB.Services.FFT;

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(conf =>
        conf.ServiceName = "ThFnsc.LoopbackRGB")
    .ConfigureServices(services =>
    {
        services.AddSingleton<IAudioProvider, NAudioLoopbackAudioProvider>();
        services.AddSingleton<IFFTCalculator, AccordFFT>();
        services.AddSingleton<IColoreableDevice, SerialRGBLed>();
        services.AddSingleton<IColorProcessor, BassPriorityColorProcessor>();
        services.AddHostedService<ColorWorker>();
    })
    .Build();

await host.RunAsync();
