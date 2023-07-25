using System;
using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;

namespace Utils.Etc
{
    public class CpuUsage
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GetSystemTimes(out FILETIME lpIdleTime,
                    out FILETIME lpKernelTime, out FILETIME lpUserTime);

        FILETIME _prevSysKernel;
        FILETIME _prevSysUser;
        FILETIME _prevSysIdle;

        TimeSpan _prevProcTotal;

        public CpuUsage()
        {
            _prevProcTotal = TimeSpan.MinValue;
        }

        public float GetUsage(out float processorCpuUsage)
        {
            processorCpuUsage = 0.0f;

            float _processCpuUsage = 0.0f;
            FILETIME sysIdle, sysKernel, sysUser;

            Process process = Process.GetCurrentProcess();
            TimeSpan procTime = process.TotalProcessorTime;

            if (!GetSystemTimes(out sysIdle, out sysKernel, out sysUser))
            {
                return 0.0f;
            }

            if (_prevProcTotal != TimeSpan.MinValue)
            {
                ulong sysKernelDiff = SubtractTimes(sysKernel, _prevSysKernel);
                ulong sysUserDiff = SubtractTimes(sysUser, _prevSysUser);
                ulong sysIdleDiff = SubtractTimes(sysIdle, _prevSysIdle);

                ulong sysTotal = sysKernelDiff + sysUserDiff;
                long kernelTotal = (long)(sysKernelDiff - sysIdleDiff);

                if (kernelTotal < 0)
                {
                    kernelTotal = 0;
                }

                processorCpuUsage = (float)((((ulong)kernelTotal + sysUserDiff) * 100.0) / sysTotal);

                long procTotal = (procTime.Ticks - _prevProcTotal.Ticks);

                if (sysTotal > 0)
                {
                    _processCpuUsage = (short)((100.0 * procTotal) / sysTotal);
                }
            }

            _prevProcTotal = procTime;
            _prevSysKernel = sysKernel;
            _prevSysUser = sysUser;
            _prevSysIdle = sysIdle;

            return _processCpuUsage;
        }

        private UInt64 SubtractTimes(FILETIME a, FILETIME b)
        {
            ulong aInt = ((ulong)a.dwHighDateTime << 32) | (uint)a.dwLowDateTime;
            ulong bInt = ((ulong)b.dwHighDateTime << 32) | (uint)b.dwLowDateTime;

            return aInt - bInt;
        }
    }
}
