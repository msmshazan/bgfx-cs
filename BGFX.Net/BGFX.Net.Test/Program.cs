using System;
using System.Runtime.InteropServices;
using SDL2;
using ImGuiNET;

namespace BGFX.Net.Test
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var platform = "x64";
            if (IntPtr.Size == 4) platform = "x86";
            NativeMethods.LoadLibrary($"{platform}/SDL2.dll");
            Bgfx.InitializeLibrary();
            ushort resolutionWidth = 800;
            ushort resolutionHeight = 600;
            var windowhandle = SDL.SDL_CreateWindow("hello", 10, 10, resolutionWidth, resolutionHeight, SDL.SDL_WindowFlags.SDL_WINDOW_ALLOW_HIGHDPI);
            var wm = new SDL.SDL_SysWMinfo();
            SDL.SDL_GetWindowWMInfo(windowhandle, ref wm);
            Bgfx.SetPlatformData(wm.info.win.window);
            Bgfx.Initialize(resolutionWidth,resolutionHeight);
            ImGui.SetCurrentContext(ImGui.CreateContext());
            var IO = ImGui.GetIO();
            IO.ImeWindowHandle = wm.info.win.window;
            IO.BackendFlags |= ImGuiBackendFlags.HasMouseCursors;         // We can honor GetMouseCursor() values (optional)
            IO.BackendFlags |= ImGuiBackendFlags.HasSetMousePos;          // We can honor io.WantSetMousePos requests (optional, rarely used)
            //IO.BackendPlatformName = new NullTerminatedString("imgui_impl_win32".ToCharArray());
            IO.KeyMap[(int)ImGuiKey.Tab] = (int)SDL.SDL_Scancode.SDL_SCANCODE_TAB;
            IO.KeyMap[(int)ImGuiKey.LeftArrow] = (int)SDL.SDL_Scancode.SDL_SCANCODE_LEFT;
            IO.KeyMap[(int)ImGuiKey.RightArrow] = (int)SDL.SDL_Scancode.SDL_SCANCODE_RIGHT;
            IO.KeyMap[(int)ImGuiKey.UpArrow] = (int)SDL.SDL_Scancode.SDL_SCANCODE_UP;
            IO.KeyMap[(int)ImGuiKey.DownArrow] = (int)SDL.SDL_Scancode.SDL_SCANCODE_DOWN;
            IO.KeyMap[(int)ImGuiKey.PageUp] = (int)SDL.SDL_Scancode.SDL_SCANCODE_PAGEUP;
            IO.KeyMap[(int)ImGuiKey.PageDown] = (int)SDL.SDL_Scancode.SDL_SCANCODE_PAGEDOWN;
            IO.KeyMap[(int)ImGuiKey.Home] = (int)SDL.SDL_Scancode.SDL_SCANCODE_HOME;
            IO.KeyMap[(int)ImGuiKey.End] = (int)SDL.SDL_Scancode.SDL_SCANCODE_END;
            IO.KeyMap[(int)ImGuiKey.Insert] = (int)SDL.SDL_Scancode.SDL_SCANCODE_INSERT;
            IO.KeyMap[(int)ImGuiKey.Delete] = (int)SDL.SDL_Scancode.SDL_SCANCODE_DELETE;
            IO.KeyMap[(int)ImGuiKey.Backspace] = (int)SDL.SDL_Scancode.SDL_SCANCODE_BACKSPACE;
            IO.KeyMap[(int)ImGuiKey.Space] = (int)SDL.SDL_Scancode.SDL_SCANCODE_SPACE;
            IO.KeyMap[(int)ImGuiKey.Enter] = (int)SDL.SDL_Scancode.SDL_SCANCODE_KP_ENTER;
            IO.KeyMap[(int)ImGuiKey.Escape] = (int)SDL.SDL_Scancode.SDL_SCANCODE_ESCAPE;
            IO.KeyMap[(int)ImGuiKey.KeyPadEnter] = (int)SDL.SDL_Scancode.SDL_SCANCODE_RETURN;
            IO.KeyMap[(int)ImGuiKey.A] = (int)SDL.SDL_Scancode.SDL_SCANCODE_A;
            IO.KeyMap[(int)ImGuiKey.C] = (int)SDL.SDL_Scancode.SDL_SCANCODE_C;
            IO.KeyMap[(int)ImGuiKey.V] = (int)SDL.SDL_Scancode.SDL_SCANCODE_V;
            IO.KeyMap[(int)ImGuiKey.X] = (int)SDL.SDL_Scancode.SDL_SCANCODE_X;
            IO.KeyMap[(int)ImGuiKey.Y] = (int)SDL.SDL_Scancode.SDL_SCANCODE_Y;
            IO.KeyMap[(int)ImGuiKey.Z] = (int)SDL.SDL_Scancode.SDL_SCANCODE_Z;
            IO.Fonts.AddFontDefault();
            IO.Fonts.GetTexDataAsRGBA32(out IntPtr pixels,out var fwidth,out var fheight);
            IO.Fonts.TexID = new IntPtr( Bgfx.CreateTexture2D((ushort)fwidth,(ushort)fheight,false,1,Bgfx.TextureFormat.BGRA8,(ulong) (Bgfx.SamplerFlags.U_CLAMP | Bgfx.SamplerFlags.V_CLAMP | Bgfx.SamplerFlags.MIN_POINT| Bgfx.SamplerFlags.MAG_POINT),Bgfx.MakeRef(pixels,(uint)(32*fwidth*fheight)) ).idx);
            var transform = new Bgfx.Transform();
            transform.Allocate(1);
            transform.Data[4] = 43;
            Bgfx.SetViewClear(0, (ushort) (Bgfx.ClearFlags.COLOR | Bgfx.ClearFlags.DEPTH), 0x6495edff, 0, 0);
            Bgfx.SetViewMode(0, Bgfx.ViewMode.SEQUENTIAL);
            Bgfx.SetDebug((uint) Bgfx.DebugFlags.STATS);
            Bgfx.Reset(resolutionWidth,resolutionHeight, Bgfx.ResetFlags.VSYNC,Bgfx.TextureFormat.BGRA8);
            var running = true;
            var bgfxcaps = Bgfx.GetCaps();
            var ortho = new RangeAccessor<float>(Marshal.AllocHGlobal(16 * Marshal.SizeOf<float>()), 16);
            
            GC.Collect();
            while (running)
            {
                SDL.SDL_PollEvent(out var Event);
                if (Event.window.type == SDL.SDL_EventType.SDL_QUIT) running = false;
                
                ImGui.NewFrame();
                if (ImGui.Begin("test"))
                {
                    ImGui.Text("Hello SANIC");
                }
                ImGui.End();
                ImGui.EndFrame();
                ImGui.Render();
                Bgfx.SetViewRect(0, 0, 0, resolutionWidth, resolutionHeight);
                Bgfx.SetViewRect(255, 0, 0, resolutionWidth, resolutionHeight);
                Bgfx.Touch(0);
                {
                    var drawdata = ImGui.GetDrawData();
                    int viewID = 255;
                    Bgfx.SetViewTransform((ushort)viewID, IntPtr.Zero, ortho.Ptr);
                }
                Bgfx.Frame(false);

            }
        }

        private static class NativeMethods
        {
            [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
            public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);
        }
    }
}