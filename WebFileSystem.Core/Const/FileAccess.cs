using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFileSystem.Const {
    public class FileAccess {
        #region Access

        public class Access {
            private byte[] Accesses { get; set; }
            public Access(string access = "-rwxrwxrwx")
            {
                Accesses = ToAccess(access);
            }
            public string AccessString {
                get {
                    return FileAccess.ToString(Accesses);
                }
            }
            public int AccessInt {
                get {
                    return FileAccess.ToInt(Accesses);
                }
            }
        }
        private static int ToInt(byte[] access)
        {
            if (access == null || access.Length != 4) { throw new ArgumentOutOfRangeException(); }
            return ToPermissionInt(access[0]) * 1000 + ToPermissionInt(access[1]) * 100 + ToPermissionInt(access[2]) * 10 + ToPermissionInt(access[3]);
        }
        private static string ToString(byte[] access)
        {
            if (access == null || access.Length != 4) { throw new ArgumentOutOfRangeException(); }
            return '-' + ToPermissionString(access[1]) + ToPermissionString(access[2]) + ToPermissionString(access[3]);
        }
        private static byte[] ToAccess(string str)
        {
            if (string.IsNullOrEmpty(str) || str.Length < 9) { throw new ArgumentOutOfRangeException(); }
            var len = str.Length;
            var other = str.Substring(len - 3, 3);
            var group = str.Substring(len - 6, 3);
            var owner = str.Substring(len - 9, 3);
            return new byte[] {
                0, ToPermission(owner),ToPermission(group),ToPermission(other),
            };
        }
        private static byte[] ToAccess(int i)
        {
            if (i > 777 || i < 0) { throw new Exception(); }
            var other = i % 10;
            var group = (i % 100) / 10;
            var owner = (i % 1000) / 100;
            return new byte[] {
                0, ToPermission(owner),ToPermission(group),ToPermission(other),
            };
        }

        #endregion

        #region Permission

        [Flags]
        public enum Permission {
            No = 0,
            Execute = 1,
            Write = 2,
            Read = 4
        }
        private static byte ToPermission(string access)
        {
            if (string.IsNullOrEmpty(access) || access.Length != 3) { throw new ArgumentOutOfRangeException(); }
            var a = access.ToLower();
            var fileAccess = Permission.No;
            if (a.IndexOf(Read) > -1) { fileAccess = fileAccess | Permission.Read; }
            if (a.IndexOf(Write) > -1) { fileAccess = fileAccess | Permission.Write; }
            if (a.IndexOf(Execute) > -1) { fileAccess = fileAccess | Permission.Execute; }
            return (byte)fileAccess;
        }
        private static byte ToPermission(int access)
        {
            if (access < 0 || access > 7) { throw new ArgumentOutOfRangeException(); }
            return (byte)(access & 0xFF);
        }
        private static string ToPermissionString(byte b)
        {
            switch (b)
            {
                case 0x0:
                    return "---";
                case 0x1:
                    return "--x";
                case 0x2:
                    return "-w-";
                case 0x3:
                    return "-wx";
                case 0x4:
                    return "r--";
                case 0x5:
                    return "r-x";
                case 0x6:
                    return "rw-";
                case 0x7:
                    return "rwx";
                default:
                    throw new Exception();
            }
        }
        private static int ToPermissionInt(byte b)
        {
            switch (b)
            {
                case 0x0:
                    return 0;
                case 0x1:
                    return 1;
                case 0x2:
                    return 2;
                case 0x3:
                    return 3;
                case 0x4:
                    return 4;
                case 0x5:
                    return 5;
                case 0x6:
                    return 6;
                case 0x7:
                    return 7;
                default:
                    throw new Exception();
            }
        }

        #endregion

        public const char Read = 'r';
        public const char Write = 'w';
        public const char Execute = 'x';
    }

}
