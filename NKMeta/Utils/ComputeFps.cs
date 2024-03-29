﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NKMeta.Utils
{
    public class ComputeFps
    {
        private DateTime prevNow = DateTime.UtcNow;
        private int _fpsCount = 0;
        public int Fps { get; set; }

        public void ComputedFps()
        {
            _fpsCount++;

            DateTime now = DateTime.UtcNow;
            if ((now - prevNow).TotalMilliseconds > 1000)
            {
                Fps = _fpsCount;

                _fpsCount = 0;
                prevNow = now;
            }
        }
    }
}
