﻿#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Tray.cs
// Version:  2020-01-13 13:03
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Threading;

    /// <summary>
    ///     Provides the functionality to manage the items of the system tray area.
    /// </summary>
    public static class Tray
    {
        /// <summary>
        ///     Refreshes the symbols on system tray.
        /// </summary>
        public static void Refresh()
        {
            var arrays = new[]
            {
                new[]
                {
                    "Shell_TrayWnd",
                    "TrayNotifyWnd",
                    "SysPager",
                    "ToolbarWindow32"
                },
                new[]
                {
                    "NotifyIconOverflowWindow",
                    "ToolbarWindow32"
                }
            };
            foreach (var array in arrays)
                try
                {
                    var hWnd = IntPtr.Zero;
                    foreach (var str in array)
                    {
                        WinApi.NativeHelper.FindNestedWindow(ref hWnd, str);
                        if (hWnd == IntPtr.Zero)
                            throw new NullReferenceException();
                    }
                    MouseMove:
                    WinApi.NativeMethods.GetClientRect(hWnd, out var rect1);
                    for (var x = 0; x < rect1.Right; x += 5)
                    {
                        for (var y = 0; y < rect1.Bottom; y += 5)
                            WinApi.NativeHelper.SendMessage(hWnd, (uint)WinApi.WindowMenuFlags.WmMouseMove, IntPtr.Zero, new IntPtr((y << 16) + x));
                        WinApi.NativeMethods.GetClientRect(hWnd, out var rect2);
                        if (rect1 != rect2)
                            goto MouseMove;
                    }
                }
                catch (Exception ex) when (ex.IsCaught())
                {
                    Log.Write(ex);
                }
        }

        /// <summary>
        ///     Refreshes the symbols on system tray asynchronous.
        /// </summary>
        /// <param name="num">
        ///     Number of refreshes.
        /// </param>
        /// <param name="wait">
        ///     Delay between refreshes in milliseconds.
        /// </param>
        public static void RefreshAsync(int num = 1, int wait = 100)
        {
            try
            {
                if (num < byte.MinValue)
                    num = byte.MinValue;
                if (num > byte.MaxValue)
                    num = byte.MaxValue;
                var thread = new Thread(() =>
                {
                    for (var i = 0; i < num; i++)
                    {
                        Refresh();
                        if (num < 2 || wait < 1)
                            continue;
                        Thread.Sleep(wait);
                    }
                })
                {
                    IsBackground = true
                };
                thread.Start();
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
        }
    }
}
