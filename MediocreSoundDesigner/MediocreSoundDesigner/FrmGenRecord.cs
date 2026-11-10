using MediocreSoundDesigner.Properties;
using NAudio.Wave;

namespace MediocreSoundDesigner;

public partial class FrmGenRecord : FrmEditMaster
{
    private WaveIn waveSource = null;
    private List<float> recordedSamples;
    private readonly object recordedSamplesLock = new();
    private readonly System.Windows.Forms.Timer waveformUpdateTimer;
    private int renderedSampleCount;
    private bool isRecording;

    public FrmGenRecord()
    {
        InitializeComponent();
        waveformUpdateTimer = new System.Windows.Forms.Timer { Interval = 100 };
        waveformUpdateTimer.Tick += WaveformUpdateTimer_Tick;
    }

    protected override void Init()
    {
        base.Init();
        recordedSamples = new();
        renderedSampleCount = 0;
        isRecording = false;
        btnRecord.BackgroundImage = Resources.record;
        waveformUpdateTimer.Start();
    }

    protected override void Apply()
    {
        StopIfRecording();
        UpdateLiveWaveform(force: true);
    }

    protected override void Cancel()
    {
        StopIfRecording();
        waveformUpdateTimer.Stop();
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        StopIfRecording();
        waveformUpdateTimer.Stop();
        waveformUpdateTimer.Dispose();
        base.OnFormClosing(e);
    }

    private void StopIfRecording()
    {
        if (isRecording)
        {
            waveSource?.StopRecording();
            isRecording = false;
        }
    }

    private void btnRecord_Click(object sender, EventArgs e)
    {
        if (isRecording)
        {
            waveSource?.StopRecording();
            isRecording = false;
            btnRecord.BackgroundImage = Resources.record;
        }
        else
        {
            try
            {
                isRecording = true;
                lock (recordedSamplesLock)
                {
                    recordedSamples.Clear();
                }
                renderedSampleCount = 0;
                btnRecord.BackgroundImage = Resources.stop;

                if (WaveIn.DeviceCount == 0)
                {
                    throw new InvalidOperationException("No audio input devices are available.");
                }

                int deviceNumber = Settings.AudioInputDeviceNumber;
                if (deviceNumber < 0 || deviceNumber >= WaveIn.DeviceCount)
                {
                    deviceNumber = 0;
                    Settings.AudioInputDeviceNumber = deviceNumber;
                }

                // Always record in the app's internal format
                waveSource = new WaveIn
                {
                    DeviceNumber = deviceNumber,
                    WaveFormat = new WaveFormat(Track.INTERNAL_SAMPLE_RATE, 16, Track.INTERNAL_CHANNELS)
                };

                waveSource.DataAvailable += WaveSource_DataAvailable;
                waveSource.RecordingStopped += WaveSource_RecordingStopped;

                waveSource.StartRecording();
            }
            catch (Exception ex)
            {
                isRecording = false;
                btnRecord.BackgroundImage = Resources.record;
                MessageBox.Show($"Failed to start recording: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void WaveSource_DataAvailable(object sender, WaveInEventArgs e)
    {
        int bytesPerSample = waveSource.WaveFormat.BitsPerSample / 8;
        int channels = waveSource.WaveFormat.Channels;
        int frameSize = bytesPerSample * channels;
        int frameCount = e.BytesRecorded / frameSize;

        lock (recordedSamplesLock)
        {
            for (int i = 0; i < frameCount; i++)
            {
                float sum = 0;
                for (int c = 0; c < channels; c++)
                {
                    int offset = (i * frameSize) + (c * bytesPerSample);
                    sum += BitConverter.ToInt16(e.Buffer, offset) / 32768f;
                }
                recordedSamples.Add(sum / channels);
            }
        }
    }

    private void WaveformUpdateTimer_Tick(object sender, EventArgs e)
    {
        UpdateLiveWaveform();
    }

    private void UpdateLiveWaveform(bool force = false)
    {
        float[] samples;
        lock (recordedSamplesLock)
        {
            if (!force && recordedSamples.Count == renderedSampleCount)
            {
                return;
            }

            samples = recordedSamples.ToArray();
        }

        renderedSampleCount = samples.Length;
        track?.ReplaceSamples(samples);
        track?.PanDrawSamples?.Invalidate();

        if (track?.PanDrawSamples?.FindForm() is FrmMain mainForm)
        {
            mainForm.RefreshTrackLayout();
        }
    }

    // Executed automatically after StopRecording() has completed
    private void WaveSource_RecordingStopped(object sender, StoppedEventArgs e)
    {
        if (waveSource != null)
        {
            waveSource.Dispose();
            waveSource = null;
        }

        MessageBox.Show("Recording saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}
