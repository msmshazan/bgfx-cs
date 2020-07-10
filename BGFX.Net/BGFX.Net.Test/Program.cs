using System;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SDL2;
using ImGuiNET;

namespace BGFX.Net.Test
{
    internal class Program
    {
        public struct Effect
        {
            public Bgfx.ProgramHandle program;
            public Bgfx.ShaderHandle vertexShader;
            public Bgfx.ShaderHandle fragmentShader;

            public Effect(byte[] vertexData,byte[] fragmentData)
            {
                vertexShader = Bgfx.CreateShader(vertexData);
                fragmentShader = Bgfx.CreateShader(fragmentData);
                program = Bgfx.CreateProgram(vertexShader, fragmentShader, true);
            }
        }

        static Effect LoadEffect(string vsFile,string fsFile)
        {
            var vsData = File.ReadAllBytes(vsFile);
            var fsData = File.ReadAllBytes(fsFile);
            var effect = new Effect(vsData,fsData);
            return effect;
        }

        static void mtxOrtho(out float[] _result, float _left, float _right, float _bottom, float _top, float _near,
            float _far, float _offset, bool _homogeneousNdc, bool _isLeftHand = true)
        {
            _result = new float[16];
            float aa = 2.0f / (_right - _left);
            float bb = 2.0f / (_top - _bottom);
            float cc = (_homogeneousNdc ? 2.0f : 1.0f) / (_far - _near);
            float dd = (_left + _right) / (_left - _right);
            float ee = (_top + _bottom) / (_bottom - _top);
            float ff = _homogeneousNdc
                    ? (_near + _far) / (_near - _far)
                    : _near / (_near - _far)
                ;
            _result[0] = aa;
            _result[5] = bb;
            _result[10] = _isLeftHand ? cc : -cc;
            _result[12] = dd + _offset;
            _result[13] = ee;
            _result[14] = ff;
            _result[15] = 1.0f;
        }
 
        private static void Main(string[] args)
        {
            var platform = "x64";
            if (IntPtr.Size == 4) platform = "x86";
            NativeMethods.LoadLibrary($"{platform}/SDL2.dll");
            Bgfx.InitializeLibrary();
            ushort resolutionWidth = 800;
            ushort resolutionHeight = 600;
            var windowhandle = SDL.SDL_CreateWindow("hello", 10, 10, resolutionWidth, resolutionHeight,
                SDL.SDL_WindowFlags.SDL_WINDOW_ALLOW_HIGHDPI);
            var wm = new SDL.SDL_SysWMinfo();
            SDL.SDL_GetWindowWMInfo(windowhandle, ref wm);
            Bgfx.SetPlatformData(wm.info.win.window);
            Bgfx.Initialize(resolutionWidth, resolutionHeight,Bgfx.RendererType.DIRECT_3D11);
            ImGui.SetCurrentContext(ImGui.CreateContext());
            var IO = ImGui.GetIO();
            IO.ImeWindowHandle = wm.info.win.window;
            IO.BackendFlags |= ImGuiBackendFlags.HasMouseCursors; // We can honor GetMouseCursor() values (optional)
            IO.BackendFlags |=
                ImGuiBackendFlags.HasSetMousePos; // We can honor io.WantSetMousePos requests (optional, rarely used)
            //IO.BackendPlatformName = new NullTerminatedString("imgui_impl_win32".ToCharArray());
            IO.KeyMap[(int) ImGuiKey.Tab] = (int) SDL.SDL_Scancode.SDL_SCANCODE_TAB;
            IO.KeyMap[(int) ImGuiKey.LeftArrow] = (int) SDL.SDL_Scancode.SDL_SCANCODE_LEFT;
            IO.KeyMap[(int) ImGuiKey.RightArrow] = (int) SDL.SDL_Scancode.SDL_SCANCODE_RIGHT;
            IO.KeyMap[(int) ImGuiKey.UpArrow] = (int) SDL.SDL_Scancode.SDL_SCANCODE_UP;
            IO.KeyMap[(int) ImGuiKey.DownArrow] = (int) SDL.SDL_Scancode.SDL_SCANCODE_DOWN;
            IO.KeyMap[(int) ImGuiKey.PageUp] = (int) SDL.SDL_Scancode.SDL_SCANCODE_PAGEUP;
            IO.KeyMap[(int) ImGuiKey.PageDown] = (int) SDL.SDL_Scancode.SDL_SCANCODE_PAGEDOWN;
            IO.KeyMap[(int) ImGuiKey.Home] = (int) SDL.SDL_Scancode.SDL_SCANCODE_HOME;
            IO.KeyMap[(int) ImGuiKey.End] = (int) SDL.SDL_Scancode.SDL_SCANCODE_END;
            IO.KeyMap[(int) ImGuiKey.Insert] = (int) SDL.SDL_Scancode.SDL_SCANCODE_INSERT;
            IO.KeyMap[(int) ImGuiKey.Delete] = (int) SDL.SDL_Scancode.SDL_SCANCODE_DELETE;
            IO.KeyMap[(int) ImGuiKey.Backspace] = (int) SDL.SDL_Scancode.SDL_SCANCODE_BACKSPACE;
            IO.KeyMap[(int) ImGuiKey.Space] = (int) SDL.SDL_Scancode.SDL_SCANCODE_SPACE;
            IO.KeyMap[(int) ImGuiKey.Enter] = (int) SDL.SDL_Scancode.SDL_SCANCODE_KP_ENTER;
            IO.KeyMap[(int) ImGuiKey.Escape] = (int) SDL.SDL_Scancode.SDL_SCANCODE_ESCAPE;
            IO.KeyMap[(int) ImGuiKey.KeyPadEnter] = (int) SDL.SDL_Scancode.SDL_SCANCODE_RETURN;
            IO.KeyMap[(int) ImGuiKey.A] = (int) SDL.SDL_Scancode.SDL_SCANCODE_A;
            IO.KeyMap[(int) ImGuiKey.C] = (int) SDL.SDL_Scancode.SDL_SCANCODE_C;
            IO.KeyMap[(int) ImGuiKey.V] = (int) SDL.SDL_Scancode.SDL_SCANCODE_V;
            IO.KeyMap[(int) ImGuiKey.X] = (int) SDL.SDL_Scancode.SDL_SCANCODE_X;
            IO.KeyMap[(int) ImGuiKey.Y] = (int) SDL.SDL_Scancode.SDL_SCANCODE_Y;
            IO.KeyMap[(int) ImGuiKey.Z] = (int) SDL.SDL_Scancode.SDL_SCANCODE_Z;
            IO.Fonts.AddFontDefault();
            IO.Fonts.GetTexDataAsRGBA32(out IntPtr pixels, out var fwidth, out var fheight);
            IO.Fonts.TexID = new IntPtr(Bgfx.CreateTexture2D((ushort) fwidth, (ushort) fheight, false, 1,
                Bgfx.TextureFormat.RGBA8,
                (Bgfx.SamplerFlags.U_CLAMP | Bgfx.SamplerFlags.V_CLAMP | Bgfx.SamplerFlags.MIN_POINT |
                         Bgfx.SamplerFlags.MAG_POINT|Bgfx.SamplerFlags.MIP_POINT), Bgfx.MakeRef(pixels, (uint) (4 * fwidth * fheight))).Idx);
            var transform = new Bgfx.Transform();
            transform.Allocate(1);
            transform.Data[4] = 43;
            Bgfx.SetViewClear(0, (ushort) (Bgfx.ClearFlags.COLOR | Bgfx.ClearFlags.DEPTH), 0x6495edff, 0, 0);
            Bgfx.SetViewMode(0, Bgfx.ViewMode.SEQUENTIAL);
            Bgfx.SetViewMode(255, Bgfx.ViewMode.SEQUENTIAL);
            Bgfx.SetDebug( Bgfx.DebugFlags.NONE);
            Bgfx.Reset(resolutionWidth, resolutionHeight, Bgfx.ResetFlags.VSYNC, Bgfx.TextureFormat.BGRA8);
            var running = true;
            var bgfxcaps = Bgfx.GetCaps();
            mtxOrtho(out var ortho, 0, resolutionWidth, resolutionHeight, 0, 0, 1000.0f, 0, bgfxcaps.Homogenousdepth);
            var ImguiVertexLayout = new Bgfx.VertexLayout();
             
            ImguiVertexLayout.Begin(Bgfx.RendererType.NOOP);
            ImguiVertexLayout.Add(Bgfx.Attrib.POSITION,Bgfx.AttribType.FLOAT,2,false,false);
            ImguiVertexLayout.Add(Bgfx.Attrib.TEX_COORD0, Bgfx.AttribType.FLOAT, 2, false, false);
            ImguiVertexLayout.Add(Bgfx.Attrib.COLOR0,Bgfx.AttribType.UINT8,4,true,false);
            ImguiVertexLayout.End();
            var WhitePixelTexture = Bgfx.CreateTexture2D(1,1,false,1,Bgfx.TextureFormat.RGBA8,Bgfx.SamplerFlags.V_CLAMP | Bgfx.SamplerFlags.U_CLAMP | Bgfx.SamplerFlags.MIN_POINT | Bgfx.SamplerFlags.MAG_POINT | Bgfx.SamplerFlags.MIP_POINT,new uint[] { 0xffffffff });
            var TextureUniform = Bgfx.CreateUniform("s_texture",Bgfx.UniformType.SAMPLER,1);
            
            var ImGuiShader = LoadEffect("vs_imgui.bin","fs_imgui.bin");
            
            while (running)
            {
                SDL.SDL_PollEvent(out var Event);
                if (Event.window.type == SDL.SDL_EventType.SDL_QUIT) running = false;

                ImGui.NewFrame();
                if (ImGui.Begin("test"))
                {
                    ImGui.Text("Hello");
                    ImGui.Button("click");
                }
                ImGui.End();
                ImGui.EndFrame();
                ImGui.Render();
                Bgfx.SetViewRect(0, 0, 0, resolutionWidth, resolutionHeight);
                Bgfx.SetViewRect(255, 0, 0, resolutionWidth, resolutionHeight);
                IO.DisplaySize = new Vector2(resolutionWidth,resolutionHeight);
                IO.DisplayFramebufferScale = new Vector2(1);
                Bgfx.Touch(0);
                {
                    var drawdata = ImGui.GetDrawData();
                    ushort viewID = 255;
                    Bgfx.SetViewTransform( viewID, null, ortho);
                    
                    {
                        // Render command lists
                        for (int ii = 0, num = drawdata.CmdListsCount; ii < num; ++ii)
                        {
                            var drawList = drawdata.CmdListsRange[ii];
                            var numVertices = drawList.VtxBuffer.Size;
                            var numIndices = drawList.IdxBuffer.Size;
                            var tib = Bgfx.AllocateTransientIndexBuffer((uint) numIndices);
                            var tvb = Bgfx.AllocateTransientVertexBuffer((uint) numVertices, ImguiVertexLayout);
                            tvb.CopyIntoBuffer(drawList.VtxBuffer.Data,
                                (uint) numVertices * (uint) Unsafe.SizeOf<ImDrawVert>());
                            tib.CopyIntoBuffer(drawList.IdxBuffer.Data,
                                (uint) numIndices * (uint) Unsafe.SizeOf<ushort>());
                            uint offset = 0;
                            for (int i = 0; i < drawList.CmdBuffer.Size; i++)
                            {
                                var cmd = drawList.CmdBuffer[i];
                                if (cmd.UserCallback != IntPtr.Zero)
                                {
                                    // cmd->UserCallback(drawList, cmd);
                                }
                                else if (cmd.ElemCount > 0)
                                {
                                    var state =  Bgfx.StateFlags.WRITE_RGB
                                                | Bgfx.StateFlags.WRITE_A
                                                | Bgfx.StateFlags.MSAA;

                                    var TexHandle = new Bgfx.TextureHandle();

                                    if (cmd.TextureId.ToInt32() != 0)
                                    {
                                        TexHandle.Idx = (ushort) cmd.TextureId.ToInt32();
                                    }
                                    else
                                    {
                                        TexHandle.Idx = WhitePixelTexture.Idx;
                                    }

                                    state |= Bgfx.STATE_BLEND_FUNC(Bgfx.StateFlags.BLEND_SRC_ALPHA,Bgfx.StateFlags.BLEND_INV_SRC_ALPHA);
                                    ushort xx = (ushort) ((cmd.ClipRect.X > 0.0f ? cmd.ClipRect.X : 0.0f));
                                    ushort yy = (ushort) ((cmd.ClipRect.Y > 0.0f ? cmd.ClipRect.Y : 0.0f));
                                    ushort zz = (ushort) ((cmd.ClipRect.Z > 65535.0f ? 65535.0f : cmd.ClipRect.Z) - xx);
                                    ushort ww = (ushort) ((cmd.ClipRect.W > 65535.0f ? 65535.0f : cmd.ClipRect.W) - yy);
                                    Bgfx.SetScissor(xx,yy,zz,ww);
                                    Bgfx.SetState(state, 0);
                                    Bgfx.SetTexture(0, TextureUniform, TexHandle);
                                    Bgfx.SetTransientVertexBuffer(0, tvb, 0, (uint)numVertices);
                                    Bgfx.SetTransientIndexBuffer(tib, offset, cmd.ElemCount);
                                    Bgfx.Submit((ushort)viewID, ImGuiShader.program, 0, false);
                                }
                                offset += cmd.ElemCount;
                            }
                        }
                    }
                    
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