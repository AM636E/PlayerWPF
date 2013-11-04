using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio;
using NAudio.Wave;
using System.Windows.Controls;

namespace Player
{
    public interface IOutputDevicePlugin
    {
        IWavePlayer CreateDevice(int latency);
        string Name { get; }
        bool IsAvailable { get; }
        int Priority { get; }
    }
}
