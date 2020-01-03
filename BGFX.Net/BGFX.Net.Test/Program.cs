using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SDL2;

namespace BGFX.Net.Test
{
    public static class NativeMethods
    {
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)]string lpFileName);
    }

    class Program
    {
        static unsafe void Main(string[] args)
        {
            var platform = "x64";
            if(IntPtr.Size == 4)
            {
                platform = "x86";
            }

            {
                NativeMethods.LoadLibrary($"{platform}/SDL2.dll");
                NativeMethods.LoadLibrary($"{platform}/BGFX.Interop.dll");
            }
            var windowhandle = SDL.SDL_CreateWindow("hello", 10,10, 800, 600, SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE);
            SDL.SDL_SysWMinfo wm = new SDL.SDL_SysWMinfo();
            SDL.SDL_GetWindowWMInfo(windowhandle,ref wm);
            var platformdata = new BGFX.PlatformData();
            platformdata.nwh = wm.info.win.window;
            BGFX.set_platform_data(ref platformdata);
            var init = new BGFX.Init();
            BGFX.init_ctor(ref init);
            init.type = BGFX.RendererType.Direct3D11;
            init.vendorId = (ushort)BGFX.PciIdFlags.None;
            init.resolution.width = 800;
            init.resolution.height = 600;
            init.resolution.reset = (uint)BGFX.ResetFlags.None;
            BGFX.init(ref init);
            BGFX.set_view_clear(0, (ushort) (BGFX.ClearFlags.Color | BGFX.ClearFlags.Depth), 0x6495edff, 0, 0);
            BGFX.set_view_mode(0, BGFX.ViewMode.Sequential);
            BGFX.set_debug((uint)BGFX.DebugFlags.Stats);
            var running = true;
            while (running)
            {
                SDL.SDL_PollEvent(out var Event);
                if (Event.window.type == SDL.SDL_EventType.SDL_QUIT)
                {
                    running = false;
                }
                BGFX.set_view_rect(0, 0, 0, 800, 600);
                BGFX.touch(0);
                BGFX.frame(false);
            }
        }
    }
}
