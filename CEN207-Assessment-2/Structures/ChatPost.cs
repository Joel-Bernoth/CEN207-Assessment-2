using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CEN207_Assessment_2.Structures
{

    public class ChatPost
    {
        public static byte[] Serialize(ChatPost_Struct post)
        {
            int size = Marshal.SizeOf(post);
            byte[] arr = new byte[size];

            IntPtr ptr = IntPtr.Zero;
            try
            {
                ptr = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(post, ptr, true);
                Marshal.Copy(ptr, arr, 0, size);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
            return arr;
        }

        public static ChatPost_Struct DeSerialize(byte[] arr)
        {
            ChatPost_Struct post = new ChatPost_Struct();

            int size = Marshal.SizeOf(post);
            IntPtr ptr = IntPtr.Zero;

            try
            {
                ptr = Marshal.AllocHGlobal(size);
                Marshal.Copy(arr, 0, ptr, size);
                post = (ChatPost_Struct)Marshal.PtrToStructure(ptr, post.GetType());
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
            return post;
        }
    }

    public struct ChatPost_Struct
    {

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        private string name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1000)]
        private string body;



        public string Name
        {
            get => this.name;
            set => this.name = value;
        }

        public string Body
        {
            get { return this.body; }
            set
            {
                this.body = value;
            }
        }
    }
}
