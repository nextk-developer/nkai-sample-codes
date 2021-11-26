using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace NKClientQuickSample.Video
{
    public class CaptureFrame
    {
        public event EventHandler<Bitmap> ResponseDrawFrameHandler;
        private VideoCapture _videocapture;
        private bool _isDraw;
        private bool _isRunProc;
        private string _url;
        public async Task<bool> Stop()
        {
            if (_isDraw == true)
                _isDraw = false;
            await Task.Run(() =>
            {
                while (true)
                {
                    if (_isRunProc == false)
                    {
                        return true;
                    }
                }
            });
            return false;
        }
        public async void StartAndUpdate(string url)
        {
            await Stop();
            ReadProc(url);
        }
        public void ReadProc(string url)
        {
            _isDraw = false;
            _url = url;
            Task.Run(() =>
            {
                Mat image = new Mat();
                _videocapture = new VideoCapture(url);
                _isRunProc = true;
                _isDraw = true;
                while (_isDraw)
                {
                    if (_videocapture.Read(image))
                        ResponseDrawFrameHandler?.Invoke(this, OpenCvSharp.Extensions.BitmapConverter.ToBitmap(image));
                    else
                    {
                        _videocapture.Dispose();
                        _videocapture = new VideoCapture(_url);
                    }
                }
                _videocapture.Dispose();
                _isRunProc = false;
            });
        }
    }
}
