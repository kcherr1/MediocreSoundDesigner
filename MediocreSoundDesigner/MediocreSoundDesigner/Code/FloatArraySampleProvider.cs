using NAudio.Wave;

namespace MediocreSoundDesigner;

// Class to act as reader for Sample array
public class FloatArraySampleProvider : ISampleProvider
{
    private readonly float[] samples;
    private int position;

    public WaveFormat WaveFormat { get; }

    public FloatArraySampleProvider(float[] samples, WaveFormat waveFormat)
    {
        this.samples = samples;
        this.WaveFormat = waveFormat;
        this.position = 0;
    }

    public int Read(Span<float> buffer)
    {
        int samplesToCopy = Math.Min(
            buffer.Length,
            samples.Length - position
        );

        samples.AsSpan(position, samplesToCopy).CopyTo(buffer);

        position += samplesToCopy;

        return samplesToCopy;
    }
}