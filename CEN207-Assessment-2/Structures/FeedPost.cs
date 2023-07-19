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

    public class FeedPost
    {
        public static byte[] Serialize(FeedPost_Struct post)
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

        public static FeedPost_Struct DeSerialize(byte[] arr)
        {
            FeedPost_Struct post = new FeedPost_Struct();

            int size = Marshal.SizeOf(post);
            IntPtr ptr = IntPtr.Zero;

            try
            {
                ptr = Marshal.AllocHGlobal(size);
                Marshal.Copy(arr, 0, ptr, size);
                post = (FeedPost_Struct)Marshal.PtrToStructure(ptr, post.GetType());
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
            return post;
        }
    }

    public struct FeedPost_Struct
    {

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        private string userName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        private string name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        private string title;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1000)]
        private string body;


        public string UserName
        {
            get { return this.userName; }
            set
            {
                if (value.Any(c => c == ' '))
                    throw new Exception("Invalid name. (It cannot contain spaces)");

                this.userName = value;
            }
        }

        public string Name
        {
            get => this.userName;
            set => this.userName = value;
        }

        public string Title
        {
            get { return this.title; }
            set
            {
                this.title = value;
            }
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
