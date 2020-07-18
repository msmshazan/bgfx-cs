using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BGFX.Private.Net;

namespace BGFX.Net
{
    public unsafe struct RangeAccessor<T> where T : struct
    {
        private static readonly int SizeOfT = Unsafe.SizeOf<T>();

        public readonly void* data;
        public readonly int count;
        public IntPtr Ptr => new IntPtr(data);
        public RangeAccessor(IntPtr data, int count) : this(data.ToPointer(), count) { }
        public RangeAccessor(void* data, int count)
        {
            this.data = data;
            this.count = count;
        }

        public ref T this[int index]
        {
            get
            {
                if (index < 0 || index >= count)
                {
                    throw new IndexOutOfRangeException();
                }

                return ref Unsafe.AsRef<T>((byte*)data + SizeOfT * index);
            }
        }
    }

    public unsafe struct RangePtrAccessor<T> where T : struct
    {
        public readonly void* data;
        public readonly int count;

        public RangePtrAccessor(IntPtr data, int count) : this(data.ToPointer(), count) { }
        public RangePtrAccessor(void* data, int count)
        {
            this.data = data;
            this.count = count;
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= count)
                {
                    throw new IndexOutOfRangeException();
                }

                return Unsafe.Read<T>((byte*)data + sizeof(void*) * index);
            }
        }
    }


    public static partial class Bgfx
    {
        public static void InitializeLibrary()
        {
            var platform = "x64";
            if (IntPtr.Size == 4) platform = "x86";
            {
                NativeMethods.LoadLibrary($"{platform}/BGFX.Interop.dll");
            }
        }

        private static class NativeMethods
        {
            [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
            public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);
        }

       
    }


    public static partial class Bgfx
    {
        public enum Access
        {
            /// <summary>
            /// Read.
            /// </summary>
            READ,

            /// <summary>
            /// Write.
            /// </summary>
            WRITE,

            /// <summary>
            /// Read and write.
            /// </summary>
            READ_WRITE,

            COUNT
        }

        public enum Attrib
        {
            /// <summary>
            /// a_position
            /// </summary>
            POSITION,

            /// <summary>
            /// a_normal
            /// </summary>
            NORMAL,

            /// <summary>
            /// a_tangent
            /// </summary>
            TANGENT,

            /// <summary>
            /// a_bitangent
            /// </summary>
            BITANGENT,

            /// <summary>
            /// a_color0
            /// </summary>
            COLOR0,

            /// <summary>
            /// a_color1
            /// </summary>
            COLOR1,

            /// <summary>
            /// a_color2
            /// </summary>
            COLOR2,

            /// <summary>
            /// a_color3
            /// </summary>
            COLOR3,

            /// <summary>
            /// a_indices
            /// </summary>
            INDICES,

            /// <summary>
            /// a_weight
            /// </summary>
            WEIGHT,

            /// <summary>
            /// a_texcoord0
            /// </summary>
            TEX_COORD0,

            /// <summary>
            /// a_texcoord1
            /// </summary>
            TEX_COORD1,

            /// <summary>
            /// a_texcoord2
            /// </summary>
            TEX_COORD2,

            /// <summary>
            /// a_texcoord3
            /// </summary>
            TEX_COORD3,

            /// <summary>
            /// a_texcoord4
            /// </summary>
            TEX_COORD4,

            /// <summary>
            /// a_texcoord5
            /// </summary>
            TEX_COORD5,

            /// <summary>
            /// a_texcoord6
            /// </summary>
            TEX_COORD6,

            /// <summary>
            /// a_texcoord7
            /// </summary>
            TEX_COORD7,

            COUNT
        }

        public enum AttribType
        {
            /// <summary>
            /// Uint8
            /// </summary>
            UINT8,

            /// <summary>
            /// Uint10, availability depends on: `BGFX_CAPS_VERTEX_ATTRIB_UINT10`.
            /// </summary>
            UINT10,

            /// <summary>
            /// Int16
            /// </summary>
            INT16,

            /// <summary>
            /// Half, availability depends on: `BGFX_CAPS_VERTEX_ATTRIB_HALF`.
            /// </summary>
            HALF,

            /// <summary>
            /// Float
            /// </summary>
            FLOAT,

            COUNT
        }

        public enum BackbufferRatio
        {
            /// <summary>
            /// Equal to backbuffer.
            /// </summary>
            EQUAL,

            /// <summary>
            /// One half size of backbuffer.
            /// </summary>
            HALF,

            /// <summary>
            /// One quarter size of backbuffer.
            /// </summary>
            QUARTER,

            /// <summary>
            /// One eighth size of backbuffer.
            /// </summary>
            EIGHTH,

            /// <summary>
            /// One sixteenth size of backbuffer.
            /// </summary>
            SIXTEENTH,

            /// <summary>
            /// Double size of backbuffer.
            /// </summary>
            DOUBLE,

            COUNT
        }

        [Flags]
        public enum BufferFlags : ushort
        {
            /// <summary>
            /// 1 8-bit value
            /// </summary>
            COMPUTE_FORMAT8_X1 = 0x0001,

            /// <summary>
            /// 2 8-bit values
            /// </summary>
            COMPUTE_FORMAT8_X2 = 0x0002,

            /// <summary>
            /// 4 8-bit values
            /// </summary>
            COMPUTE_FORMAT8_X4 = 0x0003,

            /// <summary>
            /// 1 16-bit value
            /// </summary>
            COMPUTE_FORMAT16_X1 = 0x0004,

            /// <summary>
            /// 2 16-bit values
            /// </summary>
            COMPUTE_FORMAT16_X2 = 0x0005,

            /// <summary>
            /// 4 16-bit values
            /// </summary>
            COMPUTE_FORMAT16_X4 = 0x0006,

            /// <summary>
            /// 1 32-bit value
            /// </summary>
            COMPUTE_FORMAT32_X1 = 0x0007,

            /// <summary>
            /// 2 32-bit values
            /// </summary>
            COMPUTE_FORMAT32_X2 = 0x0008,

            /// <summary>
            /// 4 32-bit values
            /// </summary>
            COMPUTE_FORMAT32_X4 = 0x0009,
            COMPUTE_FORMAT_SHIFT = 0,
            COMPUTE_FORMAT_MASK = 0x000f,

            /// <summary>
            /// Type `int`.
            /// </summary>
            COMPUTE_TYPE_INT = 0x0010,

            /// <summary>
            /// Type `uint`.
            /// </summary>
            COMPUTE_TYPE_UINT = 0x0020,

            /// <summary>
            /// Type `float`.
            /// </summary>
            COMPUTE_TYPE_FLOAT = 0x0030,
            COMPUTE_TYPE_SHIFT = 4,
            COMPUTE_TYPE_MASK = 0x0030,
            NONE = 0x0000,

            /// <summary>
            /// Buffer will be read by shader.
            /// </summary>
            COMPUTE_READ = 0x0100,

            /// <summary>
            /// Buffer will be used for writing.
            /// </summary>
            COMPUTE_WRITE = 0x0200,

            /// <summary>
            /// Buffer will be used for storing draw indirect commands.
            /// </summary>
            DRAW_INDIRECT = 0x0400,

            /// <summary>
            /// Allow dynamic index/vertex buffer resize during update.
            /// </summary>
            ALLOW_RESIZE = 0x0800,

            /// <summary>
            /// Index buffer contains 32-bit indices.
            /// </summary>
            INDEX32 = 0x1000,
            COMPUTE_READ_WRITE = 0x0300
        }

        [Flags]
        public enum CapsFlags : ulong
        {
            /// <summary>
            /// Alpha to coverage is supported.
            /// </summary>
            ALPHA_TO_COVERAGE = 0x0000000000000001,

            /// <summary>
            /// Blend independent is supported.
            /// </summary>
            BLEND_INDEPENDENT = 0x0000000000000002,

            /// <summary>
            /// Compute shaders are supported.
            /// </summary>
            COMPUTE = 0x0000000000000004,

            /// <summary>
            /// Conservative rasterization is supported.
            /// </summary>
            CONSERVATIVE_RASTER = 0x0000000000000008,

            /// <summary>
            /// Draw indirect is supported.
            /// </summary>
            DRAW_INDIRECT = 0x0000000000000010,

            /// <summary>
            /// Fragment depth is accessible in fragment shader.
            /// </summary>
            FRAGMENT_DEPTH = 0x0000000000000020,

            /// <summary>
            /// Fragment ordering is available in fragment shader.
            /// </summary>
            FRAGMENT_ORDERING = 0x0000000000000040,

            /// <summary>
            /// Read/Write frame buffer attachments are supported.
            /// </summary>
            FRAMEBUFFER_RW = 0x0000000000000080,

            /// <summary>
            /// Graphics debugger is present.
            /// </summary>
            GRAPHICS_DEBUGGER = 0x0000000000000100,
            RESERVED = 0x0000000000000200,

            /// <summary>
            /// HDR10 rendering is supported.
            /// </summary>
            HDR10 = 0x0000000000000400,

            /// <summary>
            /// HiDPI rendering is supported.
            /// </summary>
            HIDPI = 0x0000000000000800,

            /// <summary>
            /// 32-bit indices are supported.
            /// </summary>
            INDEX32 = 0x0000000000001000,

            /// <summary>
            /// Instancing is supported.
            /// </summary>
            INSTANCING = 0x0000000000002000,

            /// <summary>
            /// Occlusion query is supported.
            /// </summary>
            OCCLUSION_QUERY = 0x0000000000004000,

            /// <summary>
            /// Renderer is on separate thread.
            /// </summary>
            RENDERER_MULTITHREADED = 0x0000000000008000,

            /// <summary>
            /// Multiple windows are supported.
            /// </summary>
            SWAP_CHAIN = 0x0000000000010000,

            /// <summary>
            /// 2D texture array is supported.
            /// </summary>
            TEXTURE2D_ARRAY = 0x0000000000020000,

            /// <summary>
            /// 3D textures are supported.
            /// </summary>
            TEXTURE3D = 0x0000000000040000,

            /// <summary>
            /// Texture blit is supported.
            /// </summary>
            TEXTURE_BLIT = 0x0000000000080000,

            /// <summary>
            /// All texture compare modes are supported.
            /// </summary>
            TEXTURE_COMPARE_RESERVED = 0x0000000000100000,

            /// <summary>
            /// Texture compare less equal mode is supported.
            /// </summary>
            TEXTURE_COMPARE_LEQUAL = 0x0000000000200000,

            /// <summary>
            /// Cubemap texture array is supported.
            /// </summary>
            TEXTURE_CUBE_ARRAY = 0x0000000000400000,

            /// <summary>
            /// CPU direct access to GPU texture memory.
            /// </summary>
            TEXTURE_DIRECT_ACCESS = 0x0000000000800000,

            /// <summary>
            /// Read-back texture is supported.
            /// </summary>
            TEXTURE_READ_BACK = 0x0000000001000000,

            /// <summary>
            /// Vertex attribute half-float is supported.
            /// </summary>
            VERTEX_ATTRIB_HALF = 0x0000000002000000,

            /// <summary>
            /// Vertex attribute 10_10_10_2 is supported.
            /// </summary>
            VERTEX_ATTRIB_UINT10 = 0x0000000004000000,

            /// <summary>
            /// Rendering with VertexID only is supported.
            /// </summary>
            VERTEX_ID = 0x0000000008000000,

            /// <summary>
            /// All texture compare modes are supported.
            /// </summary>
            TEXTURE_COMPARE_ALL = 0x0000000000300000
        }

        [Flags]
        public enum CapsFormatFlags : ushort
        {
            /// <summary>
            /// Texture format is not supported.
            /// </summary>
            TEXTURE_NONE = 0x0000,

            /// <summary>
            /// Texture format is supported.
            /// </summary>
            TEXTURE2D = 0x0001,

            /// <summary>
            /// Texture as sRGB format is supported.
            /// </summary>
            TEXTURE2D_SRGB = 0x0002,

            /// <summary>
            /// Texture format is emulated.
            /// </summary>
            TEXTURE2D_EMULATED = 0x0004,

            /// <summary>
            /// Texture format is supported.
            /// </summary>
            TEXTURE3D = 0x0008,

            /// <summary>
            /// Texture as sRGB format is supported.
            /// </summary>
            TEXTURE3D_SRGB = 0x0010,

            /// <summary>
            /// Texture format is emulated.
            /// </summary>
            TEXTURE3D_EMULATED = 0x0020,

            /// <summary>
            /// Texture format is supported.
            /// </summary>
            TEXTURE_CUBE = 0x0040,

            /// <summary>
            /// Texture as sRGB format is supported.
            /// </summary>
            TEXTURE_CUBE_SRGB = 0x0080,

            /// <summary>
            /// Texture format is emulated.
            /// </summary>
            TEXTURE_CUBE_EMULATED = 0x0100,

            /// <summary>
            /// Texture format can be used from vertex shader.
            /// </summary>
            TEXTURE_VERTEX = 0x0200,

            /// <summary>
            /// Texture format can be used as image from compute shader.
            /// </summary>
            TEXTURE_IMAGE = 0x0400,

            /// <summary>
            /// Texture format can be used as frame buffer.
            /// </summary>
            TEXTURE_FRAMEBUFFER = 0x0800,

            /// <summary>
            /// Texture format can be used as MSAA frame buffer.
            /// </summary>
            TEXTURE_FRAMEBUFFER_MSAA = 0x1000,

            /// <summary>
            /// Texture can be sampled as MSAA.
            /// </summary>
            TEXTURE_MSAA = 0x2000,

            /// <summary>
            /// Texture format supports auto-generated mips.
            /// </summary>
            TEXTURE_MIP_AUTOGEN = 0x4000
        }

        [Flags]
        public enum ClearFlags : ushort
        {
            /// <summary>
            /// No clear flags.
            /// </summary>
            NONE = 0x0000,

            /// <summary>
            /// Clear color.
            /// </summary>
            COLOR = 0x0001,

            /// <summary>
            /// Clear depth.
            /// </summary>
            DEPTH = 0x0002,

            /// <summary>
            /// Clear stencil.
            /// </summary>
            STENCIL = 0x0004,

            /// <summary>
            /// Discard frame buffer attachment 0.
            /// </summary>
            DISCARD_COLOR0 = 0x0008,

            /// <summary>
            /// Discard frame buffer attachment 1.
            /// </summary>
            DISCARD_COLOR1 = 0x0010,

            /// <summary>
            /// Discard frame buffer attachment 2.
            /// </summary>
            DISCARD_COLOR2 = 0x0020,

            /// <summary>
            /// Discard frame buffer attachment 3.
            /// </summary>
            DISCARD_COLOR3 = 0x0040,

            /// <summary>
            /// Discard frame buffer attachment 4.
            /// </summary>
            DISCARD_COLOR4 = 0x0080,

            /// <summary>
            /// Discard frame buffer attachment 5.
            /// </summary>
            DISCARD_COLOR5 = 0x0100,

            /// <summary>
            /// Discard frame buffer attachment 6.
            /// </summary>
            DISCARD_COLOR6 = 0x0200,

            /// <summary>
            /// Discard frame buffer attachment 7.
            /// </summary>
            DISCARD_COLOR7 = 0x0400,

            /// <summary>
            /// Discard frame buffer depth attachment.
            /// </summary>
            DISCARD_DEPTH = 0x0800,

            /// <summary>
            /// Discard frame buffer stencil attachment.
            /// </summary>
            DISCARD_STENCIL = 0x1000,
            DISCARD_COLOR_MASK = 0x07f8,
            DISCARD_MASK = 0x1ff8
        }

        [Flags]
        public enum CubeMapFlags : uint
        {
            /// <summary>
            /// Cubemap +x.
            /// </summary>
            POSITIVE_X = 0x00000000,

            /// <summary>
            /// Cubemap -x.
            /// </summary>
            NEGATIVE_X = 0x00000001,

            /// <summary>
            /// Cubemap +y.
            /// </summary>
            POSITIVE_Y = 0x00000002,

            /// <summary>
            /// Cubemap -y.
            /// </summary>
            NEGATIVE_Y = 0x00000003,

            /// <summary>
            /// Cubemap +z.
            /// </summary>
            POSITIVE_Z = 0x00000004,

            /// <summary>
            /// Cubemap -z.
            /// </summary>
            NEGATIVE_Z = 0x00000005
        }

        [Flags]
        public enum DebugFlags : uint
        {
            /// <summary>
            /// No debug.
            /// </summary>
            NONE = 0x00000000,

            /// <summary>
            /// Enable wireframe for all primitives.
            /// </summary>
            WIREFRAME = 0x00000001,

            /// <summary>
            /// Enable infinitely fast hardware test. No draw calls will be submitted to driver.
            /// It's useful when profiling to quickly assess bottleneck between CPU and GPU.
            /// </summary>
            IFH = 0x00000002,

            /// <summary>
            /// Enable statistics display.
            /// </summary>
            STATS = 0x00000004,

            /// <summary>
            /// Enable debug text display.
            /// </summary>
            TEXT = 0x00000008,

            /// <summary>
            /// Enable profiler.
            /// </summary>
            PROFILER = 0x00000010
        }

        public enum Fatal
        {
            DEBUG_CHECK,
            INVALID_SHADER,
            UNABLE_TO_INITIALIZE,
            UNABLE_TO_CREATE_TEXTURE,
            DEVICE_LOST,

            COUNT
        }

        public enum OcclusionQueryResult
        {
            /// <summary>
            /// Query failed test.
            /// </summary>
            INVISIBLE,

            /// <summary>
            /// Query passed test.
            /// </summary>
            VISIBLE,

            /// <summary>
            /// Query result is not available yet.
            /// </summary>
            NO_RESULT,

            COUNT
        }

        [Flags]
        public enum PciIdFlags : ushort
        {
            /// <summary>
            /// Autoselect adapter.
            /// </summary>
            NONE = 0x0000,

            /// <summary>
            /// Software rasterizer.
            /// </summary>
            SOFTWARE_RASTERIZER = 0x0001,

            /// <summary>
            /// AMD adapter.
            /// </summary>
            AMD = 0x1002,

            /// <summary>
            /// Intel adapter.
            /// </summary>
            INTEL = 0x8086,

            /// <summary>
            /// nVidia adapter.
            /// </summary>
            NVIDIA = 0x10de
        }

        public enum RendererType
        {
            /// <summary>
            /// No rendering.
            /// </summary>
            NOOP,

            /// <summary>
            /// Direct3D 9.0
            /// </summary>
            DIRECT_3D9,

            /// <summary>
            /// Direct3D 11.0
            /// </summary>
            DIRECT_3D11,

            /// <summary>
            /// Direct3D 12.0
            /// </summary>
            DIRECT_3D12,

            /// <summary>
            /// GNM
            /// </summary>
            GNM,

            /// <summary>
            /// Metal
            /// </summary>
            METAL,

            /// <summary>
            /// NVN
            /// </summary>
            NVN,

            /// <summary>
            /// OpenGL ES 2.0+
            /// </summary>
            OPEN_GLES,

            /// <summary>
            /// OpenGL 2.1+
            /// </summary>
            OPEN_GL,

            /// <summary>
            /// Vulkan
            /// </summary>
            VULKAN,

            COUNT
        }

        public enum RenderFrame
        {
            /// <summary>
            /// Renderer context is not created yet.
            /// </summary>
            NO_CONTEXT,

            /// <summary>
            /// Renderer context is created and rendering.
            /// </summary>
            RENDER,

            /// <summary>
            /// Renderer context wait for main thread signal timed out without rendering.
            /// </summary>
            TIMEOUT,

            /// <summary>
            /// Renderer context is getting destroyed.
            /// </summary>
            EXITING,

            COUNT
        }

        [Flags]
        public enum ResetFlags : uint
        {
            /// <summary>
            /// Enable 2x MSAA.
            /// </summary>
            MSAA_X2 = 0x00000010,

            /// <summary>
            /// Enable 4x MSAA.
            /// </summary>
            MSAA_X4 = 0x00000020,

            /// <summary>
            /// Enable 8x MSAA.
            /// </summary>
            MSAA_X8 = 0x00000030,

            /// <summary>
            /// Enable 16x MSAA.
            /// </summary>
            MSAA_X16 = 0x00000040,
            MSAA_SHIFT = 4,
            MSAA_MASK = 0x00000070,

            /// <summary>
            /// No reset flags.
            /// </summary>
            NONE = 0x00000000,

            /// <summary>
            /// Not supported yet.
            /// </summary>
            FULLSCREEN = 0x00000001,

            /// <summary>
            /// Enable V-Sync.
            /// </summary>
            VSYNC = 0x00000080,

            /// <summary>
            /// Turn on/off max anisotropy.
            /// </summary>
            MAXANISOTROPY = 0x00000100,

            /// <summary>
            /// Begin screen capture.
            /// </summary>
            CAPTURE = 0x00000200,

            /// <summary>
            /// Flush rendering after submitting to GPU.
            /// </summary>
            FLUSH_AFTER_RENDER = 0x00002000,

            /// <summary>
            /// This flag specifies where flip occurs. Default behavior is that flip occurs
            /// before rendering new frame. This flag only has effect when `BGFX_CONFIG_MULTITHREADED=0`.
            /// </summary>
            FLIP_AFTER_RENDER = 0x00004000,

            /// <summary>
            /// Enable sRGB backbuffer.
            /// </summary>
            SRGB_BACKBUFFER = 0x00008000,

            /// <summary>
            /// Enable HDR10 rendering.
            /// </summary>
            HDR10 = 0x00010000,

            /// <summary>
            /// Enable HiDPI rendering.
            /// </summary>
            HIDPI = 0x00020000,

            /// <summary>
            /// Enable depth clamp.
            /// </summary>
            DEPTH_CLAMP = 0x00040000,

            /// <summary>
            /// Suspend rendering.
            /// </summary>
            SUSPEND = 0x00080000,
            FULLSCREEN_SHIFT = 0,
            FULLSCREEN_MASK = 0x00000001,
            RESERVED_SHIFT = 31,
            RESERVED_MASK = 0x80000000
        }

        [Flags]
        public enum ResolveFlags : uint
        {
            /// <summary>
            /// No resolve flags.
            /// </summary>
            NONE = 0x00000000,

            /// <summary>
            /// Auto-generate mip maps on resolve.
            /// </summary>
            AUTO_GEN_MIPS = 0x00000001
        }

        [Flags]
        public enum SamplerFlags : uint
        {
            /// <summary>
            /// Wrap U mode: Mirror
            /// </summary>
            U_MIRROR = 0x00000001,

            /// <summary>
            /// Wrap U mode: Clamp
            /// </summary>
            U_CLAMP = 0x00000002,

            /// <summary>
            /// Wrap U mode: Border
            /// </summary>
            U_BORDER = 0x00000003,
            U_SHIFT = 0,
            U_MASK = 0x00000003,

            /// <summary>
            /// Wrap V mode: Mirror
            /// </summary>
            V_MIRROR = 0x00000004,

            /// <summary>
            /// Wrap V mode: Clamp
            /// </summary>
            V_CLAMP = 0x00000008,

            /// <summary>
            /// Wrap V mode: Border
            /// </summary>
            V_BORDER = 0x0000000c,
            V_SHIFT = 2,
            V_MASK = 0x0000000c,

            /// <summary>
            /// Wrap W mode: Mirror
            /// </summary>
            W_MIRROR = 0x00000010,

            /// <summary>
            /// Wrap W mode: Clamp
            /// </summary>
            W_CLAMP = 0x00000020,

            /// <summary>
            /// Wrap W mode: Border
            /// </summary>
            W_BORDER = 0x00000030,
            W_SHIFT = 4,
            W_MASK = 0x00000030,

            /// <summary>
            /// Min sampling mode: Point
            /// </summary>
            MIN_POINT = 0x00000040,

            /// <summary>
            /// Min sampling mode: Anisotropic
            /// </summary>
            MIN_ANISOTROPIC = 0x00000080,
            MIN_SHIFT = 6,
            MIN_MASK = 0x000000c0,

            /// <summary>
            /// Mag sampling mode: Point
            /// </summary>
            MAG_POINT = 0x00000100,

            /// <summary>
            /// Mag sampling mode: Anisotropic
            /// </summary>
            MAG_ANISOTROPIC = 0x00000200,
            MAG_SHIFT = 8,
            MAG_MASK = 0x00000300,

            /// <summary>
            /// Mip sampling mode: Point
            /// </summary>
            MIP_POINT = 0x00000400,
            MIP_SHIFT = 10,
            MIP_MASK = 0x00000400,

            /// <summary>
            /// Compare when sampling depth texture: less.
            /// </summary>
            COMPARE_LESS = 0x00010000,

            /// <summary>
            /// Compare when sampling depth texture: less or equal.
            /// </summary>
            COMPARE_LEQUAL = 0x00020000,

            /// <summary>
            /// Compare when sampling depth texture: equal.
            /// </summary>
            COMPARE_EQUAL = 0x00030000,

            /// <summary>
            /// Compare when sampling depth texture: greater or equal.
            /// </summary>
            COMPARE_GEQUAL = 0x00040000,

            /// <summary>
            /// Compare when sampling depth texture: greater.
            /// </summary>
            COMPARE_GREATER = 0x00050000,

            /// <summary>
            /// Compare when sampling depth texture: not equal.
            /// </summary>
            COMPARE_NOTEQUAL = 0x00060000,

            /// <summary>
            /// Compare when sampling depth texture: never.
            /// </summary>
            COMPARE_NEVER = 0x00070000,

            /// <summary>
            /// Compare when sampling depth texture: always.
            /// </summary>
            COMPARE_ALWAYS = 0x00080000,
            COMPARE_SHIFT = 16,
            COMPARE_MASK = 0x000f0000,
            BORDER_COLOR_SHIFT = 24,
            BORDER_COLOR_MASK = 0x0f000000,
            RESERVED_SHIFT = 28,
            RESERVED_MASK = 0xf0000000,
            NONE = 0x00000000,

            /// <summary>
            /// Sample stencil instead of depth.
            /// </summary>
            SAMPLE_STENCIL = 0x00100000,
            POINT = 0x00000540,
            UVW_MIRROR = 0x00000015,
            UVW_CLAMP = 0x0000002a,
            UVW_BORDER = 0x0000003f,
            BITS_MASK = 0x000f07ff
        }

        [Flags]
        public enum StateFlags : ulong
        {
            /// <summary>
            /// Enable R write.
            /// </summary>
            WRITE_R = 0x0000000000000001,

            /// <summary>
            /// Enable G write.
            /// </summary>
            WRITE_G = 0x0000000000000002,

            /// <summary>
            /// Enable B write.
            /// </summary>
            WRITE_B = 0x0000000000000004,

            /// <summary>
            /// Enable alpha write.
            /// </summary>
            WRITE_A = 0x0000000000000008,

            /// <summary>
            /// Enable depth write.
            /// </summary>
            WRITE_Z = 0x0000004000000000,

            /// <summary>
            /// Enable RGB write.
            /// </summary>
            WRITE_RGB = 0x0000000000000007,

            /// <summary>
            /// Write all channels mask.
            /// </summary>
            WRITE_MASK = 0x000000400000000f,

            /// <summary>
            /// Enable depth test, less.
            /// </summary>
            DEPTH_TEST_LESS = 0x0000000000000010,

            /// <summary>
            /// Enable depth test, less or equal.
            /// </summary>
            DEPTH_TEST_LEQUAL = 0x0000000000000020,

            /// <summary>
            /// Enable depth test, equal.
            /// </summary>
            DEPTH_TEST_EQUAL = 0x0000000000000030,

            /// <summary>
            /// Enable depth test, greater or equal.
            /// </summary>
            DEPTH_TEST_GEQUAL = 0x0000000000000040,

            /// <summary>
            /// Enable depth test, greater.
            /// </summary>
            DEPTH_TEST_GREATER = 0x0000000000000050,

            /// <summary>
            /// Enable depth test, not equal.
            /// </summary>
            DEPTH_TEST_NOTEQUAL = 0x0000000000000060,

            /// <summary>
            /// Enable depth test, never.
            /// </summary>
            DEPTH_TEST_NEVER = 0x0000000000000070,

            /// <summary>
            /// Enable depth test, always.
            /// </summary>
            DEPTH_TEST_ALWAYS = 0x0000000000000080,
            DEPTH_TEST_SHIFT = 4,
            DEPTH_TEST_MASK = 0x00000000000000f0,

            /// <summary>
            /// 0, 0, 0, 0
            /// </summary>
            BLEND_ZERO = 0x0000000000001000,

            /// <summary>
            /// 1, 1, 1, 1
            /// </summary>
            BLEND_ONE = 0x0000000000002000,

            /// <summary>
            /// Rs, Gs, Bs, As
            /// </summary>
            BLEND_SRC_COLOR = 0x0000000000003000,

            /// <summary>
            /// 1-Rs, 1-Gs, 1-Bs, 1-As
            /// </summary>
            BLEND_INV_SRC_COLOR = 0x0000000000004000,

            /// <summary>
            /// As, As, As, As
            /// </summary>
            BLEND_SRC_ALPHA = 0x0000000000005000,

            /// <summary>
            /// 1-As, 1-As, 1-As, 1-As
            /// </summary>
            BLEND_INV_SRC_ALPHA = 0x0000000000006000,

            /// <summary>
            /// Ad, Ad, Ad, Ad
            /// </summary>
            BLEND_DST_ALPHA = 0x0000000000007000,

            /// <summary>
            /// 1-Ad, 1-Ad, 1-Ad ,1-Ad
            /// </summary>
            BLEND_INV_DST_ALPHA = 0x0000000000008000,

            /// <summary>
            /// Rd, Gd, Bd, Ad
            /// </summary>
            BLEND_DST_COLOR = 0x0000000000009000,

            /// <summary>
            /// 1-Rd, 1-Gd, 1-Bd, 1-Ad
            /// </summary>
            BLEND_INV_DST_COLOR = 0x000000000000a000,

            /// <summary>
            /// f, f, f, 1; f = min(As, 1-Ad)
            /// </summary>
            BLEND_SRC_ALPHA_SAT = 0x000000000000b000,

            /// <summary>
            /// Blend factor
            /// </summary>
            BLEND_FACTOR = 0x000000000000c000,

            /// <summary>
            /// 1-Blend factor
            /// </summary>
            BLEND_INV_FACTOR = 0x000000000000d000,
            BLEND_SHIFT = 12,
            BLEND_MASK = 0x000000000ffff000,

            /// <summary>
            /// Blend add: src + dst.
            /// </summary>
            BLEND_EQUATION_ADD = 0x0000000000000000,

            /// <summary>
            /// Blend subtract: src - dst.
            /// </summary>
            BLEND_EQUATION_SUB = 0x0000000010000000,

            /// <summary>
            /// Blend reverse subtract: dst - src.
            /// </summary>
            BLEND_EQUATION_REVSUB = 0x0000000020000000,

            /// <summary>
            /// Blend min: min(src, dst).
            /// </summary>
            BLEND_EQUATION_MIN = 0x0000000030000000,

            /// <summary>
            /// Blend max: max(src, dst).
            /// </summary>
            BLEND_EQUATION_MAX = 0x0000000040000000,
            BLEND_EQUATION_SHIFT = 28,
            BLEND_EQUATION_MASK = 0x00000003f0000000,

            /// <summary>
            /// Cull clockwise triangles.
            /// </summary>
            CULL_CW = 0x0000001000000000,

            /// <summary>
            /// Cull counter-clockwise triangles.
            /// </summary>
            CULL_CCW = 0x0000002000000000,
            CULL_SHIFT = 36,
            CULL_MASK = 0x0000003000000000,
            ALPHA_REF_SHIFT = 40,
            ALPHA_REF_MASK = 0x0000ff0000000000,

            /// <summary>
            /// Tristrip.
            /// </summary>
            PT_TRISTRIP = 0x0001000000000000,

            /// <summary>
            /// Lines.
            /// </summary>
            PT_LINES = 0x0002000000000000,

            /// <summary>
            /// Line strip.
            /// </summary>
            PT_LINESTRIP = 0x0003000000000000,

            /// <summary>
            /// Points.
            /// </summary>
            PT_POINTS = 0x0004000000000000,
            PT_SHIFT = 48,
            PT_MASK = 0x0007000000000000,
            POINT_SIZE_SHIFT = 52,
            POINT_SIZE_MASK = 0x00f0000000000000,

            /// <summary>
            /// Enable MSAA rasterization.
            /// </summary>
            MSAA = 0x0100000000000000,

            /// <summary>
            /// Enable line AA rasterization.
            /// </summary>
            LINEAA = 0x0200000000000000,

            /// <summary>
            /// Enable conservative rasterization.
            /// </summary>
            CONSERVATIVE_RASTER = 0x0400000000000000,

            /// <summary>
            /// No state.
            /// </summary>
            NONE = 0x0000000000000000,

            /// <summary>
            /// Front counter-clockwise (default is clockwise).
            /// </summary>
            FRONT_CCW = 0x0000008000000000,

            /// <summary>
            /// Enable blend independent.
            /// </summary>
            BLEND_INDEPENDENT = 0x0000000400000000,

            /// <summary>
            /// Enable alpha to coverage.
            /// </summary>
            BLEND_ALPHA_TO_COVERAGE = 0x0000000800000000,

            /// <summary>
            /// Default state is write to RGB, alpha, and depth with depth test less enabled, with clockwise
            /// culling and MSAA (when writing into MSAA frame buffer, otherwise this flag is ignored).
            /// </summary>
            DEFAULT = 0x010000500000001f,
            MASK = 0xffffffffffffffff,
            RESERVED_SHIFT = 61,
            RESERVED_MASK = 0xe000000000000000
        }

        [Flags]
        public enum StencilFlags : uint
        {
            FUNC_REF_SHIFT = 0,
            FUNC_REF_MASK = 0x000000ff,
            FUNC_RMASK_SHIFT = 8,
            FUNC_RMASK_MASK = 0x0000ff00,
            NONE = 0x00000000,
            MASK = 0xffffffff,
            DEFAULT = 0x00000000,

            /// <summary>
            /// Enable stencil test, less.
            /// </summary>
            TEST_LESS = 0x00010000,

            /// <summary>
            /// Enable stencil test, less or equal.
            /// </summary>
            TEST_LEQUAL = 0x00020000,

            /// <summary>
            /// Enable stencil test, equal.
            /// </summary>
            TEST_EQUAL = 0x00030000,

            /// <summary>
            /// Enable stencil test, greater or equal.
            /// </summary>
            TEST_GEQUAL = 0x00040000,

            /// <summary>
            /// Enable stencil test, greater.
            /// </summary>
            TEST_GREATER = 0x00050000,

            /// <summary>
            /// Enable stencil test, not equal.
            /// </summary>
            TEST_NOTEQUAL = 0x00060000,

            /// <summary>
            /// Enable stencil test, never.
            /// </summary>
            TEST_NEVER = 0x00070000,

            /// <summary>
            /// Enable stencil test, always.
            /// </summary>
            TEST_ALWAYS = 0x00080000,
            TEST_SHIFT = 16,
            TEST_MASK = 0x000f0000,

            /// <summary>
            /// Zero.
            /// </summary>
            OP_FAIL_S_ZERO = 0x00000000,

            /// <summary>
            /// Keep.
            /// </summary>
            OP_FAIL_S_KEEP = 0x00100000,

            /// <summary>
            /// Replace.
            /// </summary>
            OP_FAIL_S_REPLACE = 0x00200000,

            /// <summary>
            /// Increment and wrap.
            /// </summary>
            OP_FAIL_S_INCR = 0x00300000,

            /// <summary>
            /// Increment and clamp.
            /// </summary>
            OP_FAIL_S_INCRSAT = 0x00400000,

            /// <summary>
            /// Decrement and wrap.
            /// </summary>
            OP_FAIL_S_DECR = 0x00500000,

            /// <summary>
            /// Decrement and clamp.
            /// </summary>
            OP_FAIL_S_DECRSAT = 0x00600000,

            /// <summary>
            /// Invert.
            /// </summary>
            OP_FAIL_S_INVERT = 0x00700000,
            OP_FAIL_S_SHIFT = 20,
            OP_FAIL_S_MASK = 0x00f00000,

            /// <summary>
            /// Zero.
            /// </summary>
            OP_FAIL_Z_ZERO = 0x00000000,

            /// <summary>
            /// Keep.
            /// </summary>
            OP_FAIL_Z_KEEP = 0x01000000,

            /// <summary>
            /// Replace.
            /// </summary>
            OP_FAIL_Z_REPLACE = 0x02000000,

            /// <summary>
            /// Increment and wrap.
            /// </summary>
            OP_FAIL_Z_INCR = 0x03000000,

            /// <summary>
            /// Increment and clamp.
            /// </summary>
            OP_FAIL_Z_INCRSAT = 0x04000000,

            /// <summary>
            /// Decrement and wrap.
            /// </summary>
            OP_FAIL_Z_DECR = 0x05000000,

            /// <summary>
            /// Decrement and clamp.
            /// </summary>
            OP_FAIL_Z_DECRSAT = 0x06000000,

            /// <summary>
            /// Invert.
            /// </summary>
            OP_FAIL_Z_INVERT = 0x07000000,
            OP_FAIL_Z_SHIFT = 24,
            OP_FAIL_Z_MASK = 0x0f000000,

            /// <summary>
            /// Zero.
            /// </summary>
            OP_PASS_Z_ZERO = 0x00000000,

            /// <summary>
            /// Keep.
            /// </summary>
            OP_PASS_Z_KEEP = 0x10000000,

            /// <summary>
            /// Replace.
            /// </summary>
            OP_PASS_Z_REPLACE = 0x20000000,

            /// <summary>
            /// Increment and wrap.
            /// </summary>
            OP_PASS_Z_INCR = 0x30000000,

            /// <summary>
            /// Increment and clamp.
            /// </summary>
            OP_PASS_Z_INCRSAT = 0x40000000,

            /// <summary>
            /// Decrement and wrap.
            /// </summary>
            OP_PASS_Z_DECR = 0x50000000,

            /// <summary>
            /// Decrement and clamp.
            /// </summary>
            OP_PASS_Z_DECRSAT = 0x60000000,

            /// <summary>
            /// Invert.
            /// </summary>
            OP_PASS_Z_INVERT = 0x70000000,
            OP_PASS_Z_SHIFT = 28,
            OP_PASS_Z_MASK = 0xf0000000
        }

        [Flags]
        public enum TextureFlags : ulong
        {
            NONE = 0x0000000000000000,

            /// <summary>
            /// Texture will be used for MSAA sampling.
            /// </summary>
            MSAA_SAMPLE = 0x0000000800000000,

            /// <summary>
            /// Render target no MSAA.
            /// </summary>
            RT = 0x0000001000000000,

            /// <summary>
            /// Texture will be used for compute write.
            /// </summary>
            COMPUTE_WRITE = 0x0000100000000000,

            /// <summary>
            /// Sample texture as sRGB.
            /// </summary>
            SRGB = 0x0000200000000000,

            /// <summary>
            /// Texture will be used as blit destination.
            /// </summary>
            BLIT_DST = 0x0000400000000000,

            /// <summary>
            /// Texture will be used for read back from GPU.
            /// </summary>
            READ_BACK = 0x0000800000000000,

            /// <summary>
            /// Render target MSAAx2 mode.
            /// </summary>
            RT_MSAA_X2 = 0x0000002000000000,

            /// <summary>
            /// Render target MSAAx4 mode.
            /// </summary>
            RT_MSAA_X4 = 0x0000003000000000,

            /// <summary>
            /// Render target MSAAx8 mode.
            /// </summary>
            RT_MSAA_X8 = 0x0000004000000000,

            /// <summary>
            /// Render target MSAAx16 mode.
            /// </summary>
            RT_MSAA_X16 = 0x0000005000000000,
            RT_MSAA_SHIFT = 36,
            RT_MSAA_MASK = 0x0000007000000000,

            /// <summary>
            /// Render target will be used for writing
            /// </summary>
            RT_WRITE_ONLY = 0x0000008000000000,
            RT_SHIFT = 36,
            RT_MASK = 0x000000f000000000
        }

        public enum TextureFormat
        {
            /// <summary>
            /// DXT1 R5G6B5A1
            /// </summary>
            BC1,

            /// <summary>
            /// DXT3 R5G6B5A4
            /// </summary>
            BC2,

            /// <summary>
            /// DXT5 R5G6B5A8
            /// </summary>
            BC3,

            /// <summary>
            /// LATC1/ATI1 R8
            /// </summary>
            BC4,

            /// <summary>
            /// LATC2/ATI2 RG8
            /// </summary>
            BC5,

            /// <summary>
            /// BC6H RGB16F
            /// </summary>
            BC6_H,

            /// <summary>
            /// BC7 RGB 4-7 bits per color channel, 0-8 bits alpha
            /// </summary>
            BC7,

            /// <summary>
            /// ETC1 RGB8
            /// </summary>
            ETC1,

            /// <summary>
            /// ETC2 RGB8
            /// </summary>
            ETC2,

            /// <summary>
            /// ETC2 RGBA8
            /// </summary>
            ETC2_A,

            /// <summary>
            /// ETC2 RGB8A1
            /// </summary>
            ETC2_A1,

            /// <summary>
            /// PVRTC1 RGB 2BPP
            /// </summary>
            PTC12,

            /// <summary>
            /// PVRTC1 RGB 4BPP
            /// </summary>
            PTC14,

            /// <summary>
            /// PVRTC1 RGBA 2BPP
            /// </summary>
            PTC12_A,

            /// <summary>
            /// PVRTC1 RGBA 4BPP
            /// </summary>
            PTC14_A,

            /// <summary>
            /// PVRTC2 RGBA 2BPP
            /// </summary>
            PTC22,

            /// <summary>
            /// PVRTC2 RGBA 4BPP
            /// </summary>
            PTC24,

            /// <summary>
            /// ATC RGB 4BPP
            /// </summary>
            ATC,

            /// <summary>
            /// ATCE RGBA 8 BPP explicit alpha
            /// </summary>
            ATCE,

            /// <summary>
            /// ATCI RGBA 8 BPP interpolated alpha
            /// </summary>
            ATCI,

            /// <summary>
            /// ASTC 4x4 8.0 BPP
            /// </summary>
            ASTC4_X4,

            /// <summary>
            /// ASTC 5x5 5.12 BPP
            /// </summary>
            ASTC5_X5,

            /// <summary>
            /// ASTC 6x6 3.56 BPP
            /// </summary>
            ASTC6_X6,

            /// <summary>
            /// ASTC 8x5 3.20 BPP
            /// </summary>
            ASTC8_X5,

            /// <summary>
            /// ASTC 8x6 2.67 BPP
            /// </summary>
            ASTC8_X6,

            /// <summary>
            /// ASTC 10x5 2.56 BPP
            /// </summary>
            ASTC10_X5,

            /// <summary>
            /// Compressed formats above.
            /// </summary>
            UNKNOWN,
            R1,
            A8,
            R8,
            R8_I,
            R8_U,
            R8_S,
            R16,
            R16_I,
            R16_U,
            R16_F,
            R16_S,
            R32_I,
            R32_U,
            R32_F,
            RG8,
            RG8_I,
            RG8_U,
            RG8_S,
            RG16,
            RG16_I,
            RG16_U,
            RG16_F,
            RG16_S,
            RG32_I,
            RG32_U,
            RG32_F,
            RGB8,
            RGB8_I,
            RGB8_U,
            RGB8_S,
            RGB9_E5_F,
            BGRA8,
            RGBA8,
            RGBA8_I,
            RGBA8_U,
            RGBA8_S,
            RGBA16,
            RGBA16_I,
            RGBA16_U,
            RGBA16_F,
            RGBA16_S,
            RGBA32_I,
            RGBA32_U,
            RGBA32_F,
            R5_G6_B5,
            RGBA4,
            RGB5_A1,
            RGB10_A2,
            RG11_B10_F,

            /// <summary>
            /// Depth formats below.
            /// </summary>
            UNKNOWN_DEPTH,
            D16,
            D24,
            D24_S8,
            D32,
            D16_F,
            D24_F,
            D32_F,
            D0_S8,

            COUNT
        }

        public enum Topology
        {
            /// <summary>
            /// Triangle list.
            /// </summary>
            TRI_LIST,

            /// <summary>
            /// Triangle strip.
            /// </summary>
            TRI_STRIP,

            /// <summary>
            /// Line list.
            /// </summary>
            LINE_LIST,

            /// <summary>
            /// Line strip.
            /// </summary>
            LINE_STRIP,

            /// <summary>
            /// Point list.
            /// </summary>
            POINT_LIST,

            COUNT
        }

        public enum TopologyConvert
        {
            /// <summary>
            /// Flip winding order of triangle list.
            /// </summary>
            TRI_LIST_FLIP_WINDING,

            /// <summary>
            /// Flip winding order of trinagle strip.
            /// </summary>
            TRI_STRIP_FLIP_WINDING,

            /// <summary>
            /// Convert triangle list to line list.
            /// </summary>
            TRI_LIST_TO_LINE_LIST,

            /// <summary>
            /// Convert triangle strip to triangle list.
            /// </summary>
            TRI_STRIP_TO_TRI_LIST,

            /// <summary>
            /// Convert line strip to line list.
            /// </summary>
            LINE_STRIP_TO_LINE_LIST,

            COUNT
        }

        public enum TopologySort
        {
            DIRECTION_FRONT_TO_BACK_MIN,
            DIRECTION_FRONT_TO_BACK_AVG,
            DIRECTION_FRONT_TO_BACK_MAX,
            DIRECTION_BACK_TO_FRONT_MIN,
            DIRECTION_BACK_TO_FRONT_AVG,
            DIRECTION_BACK_TO_FRONT_MAX,
            DISTANCE_FRONT_TO_BACK_MIN,
            DISTANCE_FRONT_TO_BACK_AVG,
            DISTANCE_FRONT_TO_BACK_MAX,
            DISTANCE_BACK_TO_FRONT_MIN,
            DISTANCE_BACK_TO_FRONT_AVG,
            DISTANCE_BACK_TO_FRONT_MAX,

            COUNT
        }

        public enum UniformType
        {
            /// <summary>
            /// Sampler.
            /// </summary>
            SAMPLER,

            /// <summary>
            /// Reserved, do not use.
            /// </summary>
            END,

            /// <summary>
            /// 4 floats vector.
            /// </summary>
            VEC4,

            /// <summary>
            /// 3x3 matrix.
            /// </summary>
            MAT3,

            /// <summary>
            /// 4x4 matrix.
            /// </summary>
            MAT4,

            COUNT
        }

        public enum ViewMode
        {
            /// <summary>
            /// Default sort order.
            /// </summary>
            DEFAULT,

            /// <summary>
            /// Sort in the same order in which submit calls were called.
            /// </summary>
            SEQUENTIAL,

            /// <summary>
            /// Sort draw call depth in ascending order.
            /// </summary>
            DEPTH_ASCENDING,

            /// <summary>
            /// Sort draw call depth in descending order.
            /// </summary>
            DEPTH_DESCENDING,

            COUNT
        }

        public static void SetPlatformData(IntPtr WindowHandle)
        {
            unsafe
            { 
                UnsafeBgfx.PlatformData platformData = new UnsafeBgfx.PlatformData();
                platformData.nwh = WindowHandle.ToPointer();
                UnsafeBgfx.set_platform_data(&platformData);
            }
        }

        public struct Init
        {
            public UnsafeBgfx.Init init;

            public TextureFormat format => (TextureFormat)init.resolution.format;
        }

        public static Init Initialize(uint resolutionWidth,uint resolutionHeight,RendererType renderer)
        {
            var Result = new Init();
            UnsafeBgfx.Init init = new UnsafeBgfx.Init();
            unsafe
            {
                UnsafeBgfx.init_ctor(&init);
                init.type = (UnsafeBgfx.RendererType) renderer;
                init.vendorId = (ushort)UnsafeBgfx.PciIdFlags.None;
                init.resolution.width = resolutionWidth;
                init.resolution.height = resolutionHeight;
                init.resolution.reset = (uint)Bgfx.ResetFlags.NONE;
                UnsafeBgfx.init(&init);
            }

            Result.init = init;
            return Result;
        }

        public static IntPtr MakeRef(IntPtr data,uint size)
        {
            unsafe
            {
               return new IntPtr( UnsafeBgfx.make_ref(data.ToPointer(),size));
            }
        }

        public static void Frame(bool capture)
        {
            unsafe
            {
                UnsafeBgfx.frame(capture);
            }
        }


        public static void Reset(uint width,uint height,ResetFlags resetFlags,TextureFormat format)
        {
            unsafe
            {
                UnsafeBgfx.reset(width,height,(uint)resetFlags, (UnsafeBgfx.TextureFormat)format);
            }
        }
        public struct Transform
        {
            public void Allocate(ushort num)
            {
                unsafe
                {
                    _transform.num = num;
                    _ = UnsafeBgfx.alloc_transform((UnsafeBgfx.Transform *)Unsafe.AsPointer(ref _transform),num);
                }
            }

            private UnsafeBgfx.Transform _transform;

            public RangeAccessor<float> Data
            {
                get
                {
                    unsafe
                    {
                        if ((IntPtr)_transform.data == IntPtr.Zero)
                        {
                            throw new OutOfMemoryException("Allocate() not called");
                        }

                        return new RangeAccessor<float>(_transform.data, 16 * _transform.num);
                    }
                }
            }


        }

        public static void SetDebug(DebugFlags flag)
        {
            UnsafeBgfx.set_debug((uint) flag);
        }

        public static void SetViewMode(ushort i, ViewMode sequential)
        {
            UnsafeBgfx.set_view_mode(i, (UnsafeBgfx.ViewMode)sequential);
        }

        public static void SetViewClear(ushort i, ushort flags, uint i1, float i2, byte i3)
        {
             UnsafeBgfx.set_view_clear(i,flags,i1,i2,i3);
        }

        public static void Touch(ushort i)
        {
            UnsafeBgfx.touch(i);
        }

        public static void SetViewRect(ushort p0, ushort p1, ushort p2, ushort resolutionWidth, ushort resolutionHeight)
        {
            UnsafeBgfx.set_view_rect(p0,p1,p2,resolutionWidth,resolutionHeight);
        }

        public static void SetViewTransform(ushort viewId, float[] view, float[] projection)
        {
            unsafe
            {
                UnsafeBgfx.set_view_transform(viewId, view != null ?  Unsafe.AsPointer(ref view[0]) : null,projection != null ? Unsafe.AsPointer(ref projection[0]):null);
            }
        }

        public struct BgfxCaps
        {
            public UnsafeBgfx.Caps caps;
            public bool Homogenousdepth => (caps.homogeneousDepth != 0);
        }

        public struct TransientVertexBuffer
        {
            public UnsafeBgfx.TransientVertexBuffer tvb;

            public void CopyIntoBuffer(IntPtr source,uint size)
            {
                unsafe
                {
                    Unsafe.CopyBlock(tvb.data,source.ToPointer(),size);
                }
            }
        }

        public struct TransientIndexBuffer
        {
            public UnsafeBgfx.TransientIndexBuffer tib;
            public void CopyIntoBuffer(IntPtr source, uint size)
            {
                unsafe
                {
                    Unsafe.CopyBlock(tib.data, source.ToPointer(), size);
                }
            }
        }

        

        public struct VertexLayout
        {
            public UnsafeBgfx.VertexLayout vertexLayout;

            public void Begin(RendererType rendererType)
            {
                unsafe
                {
                    UnsafeBgfx.vertex_layout_begin((UnsafeBgfx.VertexLayout*)Unsafe.AsPointer(ref vertexLayout), (UnsafeBgfx.RendererType)rendererType);
                }
            }

            public void Add(Attrib attrib,AttribType attribType,byte num, bool normalized,bool asInt)
            {
                unsafe
                {
                    UnsafeBgfx.vertex_layout_add((UnsafeBgfx.VertexLayout*)Unsafe.AsPointer(ref vertexLayout), (UnsafeBgfx.Attrib)attrib,num, (UnsafeBgfx.AttribType)attribType,normalized,asInt);
                }
            }

            public void End()
            {
                unsafe
                {
                    UnsafeBgfx.vertex_layout_end((UnsafeBgfx.VertexLayout*) Unsafe.AsPointer(ref vertexLayout));
                }
            }
        }

        public static TransientVertexBuffer AllocateTransientVertexBuffer(uint num,VertexLayout layout)
        {
            var result = new TransientVertexBuffer();
            unsafe
            {
                UnsafeBgfx.alloc_transient_vertex_buffer(&result.tvb,num,&layout.vertexLayout);   
            }
            return result;
        }
        public static TransientIndexBuffer AllocateTransientIndexBuffer(uint num)
        {
            var result = new TransientIndexBuffer();
            unsafe
            {
                UnsafeBgfx.alloc_transient_index_buffer(&result.tib,num);
            }
            return result;
        }

        public static BgfxCaps GetCaps()
        {
            var caps = new BgfxCaps();
            unsafe
            {
                caps.caps = (*UnsafeBgfx.get_caps());
            }
            return caps;
        }

        public struct ShaderHandle
        {
            public UnsafeBgfx.ShaderHandle shader;
        }
        public struct ProgramHandle
        {
            public UnsafeBgfx.ProgramHandle program;
        }
        public struct Uniform
        {
            public UnsafeBgfx.UniformHandle handle;
        }

        public static ShaderHandle CreateShader(byte[] bytes)
        {
            var shader = new ShaderHandle();
            unsafe
            {
                var data = CopyIntoBuffer(bytes);
                shader.shader = UnsafeBgfx.create_shader(data);
            }
            return shader;
        }

        public static ProgramHandle CreateProgram(ShaderHandle vertex,ShaderHandle fragment,bool destroyShader)
        {
            var program = new ProgramHandle
            {
                program = UnsafeBgfx.create_program(vertex.shader, fragment.shader, destroyShader)
            };
            return program;
        }

        public static Uniform CreateUniform(string name, UniformType type, ushort num)
        {
            var uniform = new Uniform
            {
                handle = UnsafeBgfx.create_uniform(name, (UnsafeBgfx.UniformType) type, num)
            };
            return uniform;
        }
        public struct TextureHandle
        {
            public UnsafeBgfx.TextureHandle handle;

            public ushort Idx
            {
                get => handle.idx;
                set => handle.idx = value;
            }
        }
        public static TextureHandle CreateTexture2D(ushort width, ushort height, bool hasmips, ushort layers, TextureFormat bgra8, SamplerFlags flags, IntPtr makeRef)
        {
            unsafe
            {
                var texHandle = new TextureHandle
                {
                    handle = UnsafeBgfx.create_texture_2d(width, height, hasmips, layers, (UnsafeBgfx.TextureFormat)bgra8, (ulong)flags, (UnsafeBgfx.Memory*)makeRef.ToPointer())
                };
                return texHandle;
            }
        }

        public static void UpdateTexture2D(TextureHandle texture,ushort layer,byte mip,ushort x,ushort y,ushort width,ushort height,IntPtr memory,ushort pitch)
        {
            unsafe
            {
                UnsafeBgfx.update_texture_2d(texture.handle,layer,mip,x,y,width,height, (UnsafeBgfx.Memory*)memory.ToPointer(),pitch);
            }
        }

        public static void UpdateTexture2D<T>(TextureHandle texture, ushort layer, byte mip, ushort x, ushort y, ushort width, ushort height, T[] pixelBytes, ushort pitch) where T : struct
        {
            unsafe
            {
                var data = CopyIntoBuffer(pixelBytes);
                UnsafeBgfx.update_texture_2d(texture.handle,layer,mip,x,y,width,height,data,pitch);
            }
        }

        private static unsafe UnsafeBgfx.Memory* CopyIntoBuffer<T>(T[] bytes)
        {
            var size = (uint)(bytes.Length * Unsafe.SizeOf<T>());
            var data = UnsafeBgfx.alloc(size);
            Unsafe.CopyBlock(data->data, Unsafe.AsPointer(ref bytes[0]), size);
            return data;
        }

        public static TextureHandle CreateTexture2D<T>(ushort outWidth, ushort outHeight, bool b, ushort i, TextureFormat bgra8, SamplerFlags flags,T[] pixelBytes) where T : struct 
        {
            unsafe
            {
                var data = CopyIntoBuffer(pixelBytes);
                var texHandle = new TextureHandle
                {
                    handle = UnsafeBgfx.create_texture_2d(outWidth, outHeight, b, i, (UnsafeBgfx.TextureFormat) bgra8,(ulong) flags, data)
                };
                return texHandle;
            }
        }
        public static void SetState(StateFlags state,uint rgba)
        {
            UnsafeBgfx.set_state((ulong)state,rgba);
        }
        
        public static void SetTexture(byte state,Uniform uniform,TextureHandle texture,TextureFlags flags = (TextureFlags)uint.MaxValue)
        {
            UnsafeBgfx.set_texture(state,uniform.handle,texture.handle,(uint)flags);
        }

        public static void SetTransientVertexBuffer(byte stream, TransientVertexBuffer tvb, uint startVertex,
            uint numVertices)
        {
            unsafe
            {
                UnsafeBgfx.set_transient_vertex_buffer(stream, &tvb.tvb, startVertex, numVertices);
            }
        }

        public static void SetTransientIndexBuffer(TransientIndexBuffer tib, uint firstIndex, uint numIndices)
        {
            unsafe
            {
                UnsafeBgfx.set_transient_index_buffer(&tib.tib,firstIndex,numIndices);
            }
        }

        public static void SetScissor(ushort x, ushort y, ushort width, ushort height)
        {
            UnsafeBgfx.set_scissor(x, y,width,height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StateFlags STATE_BLEND_FUNC(StateFlags src,StateFlags dst)
        {
            return STATE_BLEND_FUNC_SEPARATE(src, dst, src, dst);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StateFlags STATE_BLEND_FUNC_SEPARATE(StateFlags srcRGB,StateFlags dstRGB,StateFlags srcA,StateFlags dstA)
        {
            return (StateFlags) ((((ulong)(srcRGB) | ((ulong)(dstRGB) << 4)))| (((ulong) (srcA) | ((ulong) (dstA) << 4)) << 8));
        }

        public static void Submit(ushort viewId, ProgramHandle program, uint depth, bool flags)
        {
            UnsafeBgfx.submit(viewId, program.program, depth, (byte)(flags ? 1: 0));
        }
    }
}