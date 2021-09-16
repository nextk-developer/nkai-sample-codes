﻿using System;
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
        //public void Stop()
        //{
        //    if(_isRunning == true)
        //        _isRunning = false;
        //}
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

            _videocapture = new VideoCapture(url);
            _isDraw = true;

            ReadProc();
        }
        public void ReadProc()
        {
            Task.Run(() =>
            {
                _isRunProc = true;
                while (_isDraw)
                {
                    if (_videocapture.FrameCount == _videocapture.PosFrames)
                    {
                        break;
                    }
                    using (Mat image = new Mat())
                    {
                        if (_videocapture.Read(image))
                        {
                            ResponseDrawFrameHandler?.Invoke(this, OpenCvSharp.Extensions.BitmapConverter.ToBitmap(image));
                        }
                        GC.Collect();
                    }
                }
                _videocapture.Dispose();
                _isRunProc = false;
            });
        }
    }
}