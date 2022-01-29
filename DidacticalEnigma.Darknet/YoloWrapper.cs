using System.Runtime.InteropServices;

namespace DidacticalEnigma.Darknet
{
    public class Yolo : IDisposable
    {
        private const string YoloLibraryName = "darknet";
        private const int MaxObjects = 1000;

        [DllImport(YoloLibraryName, EntryPoint = "init")]
        private static extern int InitializeYolo(
            [MarshalAs(UnmanagedType.LPUTF8Str)] string configurationFilename,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string weightsFilename,
            int gpu,
            int batch_size);

        [DllImport(YoloLibraryName, EntryPoint = "detect_image")]
        private static extern int DetectImage(
            [MarshalAs(UnmanagedType.LPUTF8Str)] string filename,
            ref BboxContainer container);

        [DllImport(YoloLibraryName, EntryPoint = "detect_mat")]
        private static extern int DetectImage(
            IntPtr pArray,
            int nSize,
            ref BboxContainer container);

        [DllImport(YoloLibraryName, EntryPoint = "dispose")]
        private static extern int DisposeYolo();

        [StructLayout(LayoutKind.Sequential)]
        public struct bbox_t
        {
            public UInt32 x, y, w, h;    // (x,y) - top-left corner, (w, h) - width & height of bounded box
            public float prob;           // confidence - probability that the object was found correctly
            public UInt32 obj_id;        // class of object - from range [0, classes-1]
            public UInt32 track_id;      // tracking id for video (0 - untracked, 1 - inf - tracked object)
            public UInt32 frames_counter;
            public float x_3d, y_3d, z_3d;  // 3-D coordinates, if there is used 3D-stereo camera
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct BboxContainer
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MaxObjects)]
            public bbox_t[] candidates;
        }

        public Yolo(string configurationFilename, string weightsFilename, int gpu, int batchSize = 1)
        {
            InitializeYolo(configurationFilename, weightsFilename, gpu, batchSize);
        }

        public void Dispose()
        {
            DisposeYolo();
        }

        public bbox_t[] Detect(string filename)
        {
            var container = new BboxContainer();
            var count = DetectImage(filename, ref container);

            return container.candidates;
        }

        public IEnumerable<bbox_t> Detect(byte[] imageData)
        {
            var container = new BboxContainer();

            var size = Marshal.SizeOf(imageData[0]) * imageData.Length;
            var pnt = Marshal.AllocHGlobal(size);

            try
            {
                // Copy the array to unmanaged memory.
                Marshal.Copy(imageData, 0, pnt, imageData.Length);
                var count = DetectImage(pnt, imageData.Length, ref container);
                if (count == -1)
                {
                    throw new NotSupportedException($"{YoloLibraryName} has no OpenCV support");
                }
                
                return container.candidates.Take(count);
            }
            finally
            {
                // Free the unmanaged memory.
                Marshal.FreeHGlobal(pnt);
            }
        }
    }
}
