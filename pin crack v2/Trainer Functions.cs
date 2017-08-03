/*
 * Api calls from kernel32.dll, used to open a process, read and write it's memory, and
 * allocate/deallocate memory from it
 * 
 */
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
namespace TrainerFunctions
    {
    class UNUSED_APIS
        {
        internal class ProcessMemoryReaderApi
            {
            [Flags]
            public enum ProcessAccessType
                {
                PROCESS_TERMINATE = (0x0001),
                PROCESS_CREATE_THREAD = (0x0002),
                PROCESS_SET_SESSIONID = (0x0004),
                PROCESS_VM_OPERATION = (0x0008),
                PROCESS_VM_READ = (0x0010),
                PROCESS_VM_WRITE = (0x0020),
                PROCESS_DUP_HANDLE = (0x0040),
                PROCESS_CREATE_PROCESS = (0x0080),
                PROCESS_SET_QUOTA = (0x0100),
                PROCESS_SET_INFORMATION = (0x0200),
                PROCESS_QUERY_INFORMATION = (0x0400)
                }

            [DllImport("kernel32.dll")]
            public static extern IntPtr OpenProcess(UInt32 dwDesiredAccess, Int32 bInheritHandle, UInt32 dwProcessId);

            [DllImport("kernel32.dll")]
            public static extern Int32 CloseHandle(IntPtr hObject);

            [DllImport("kernel32.dll")]
            public static extern Int32 ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesRead);

            [DllImport("kernel32.dll")]
            public static extern Int32 WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesWritten);

            }
        //kernel32 api
        internal class Kernel32Api
            {

            internal class MemoryProtection
                {
                public const UInt32 PAGE_READWRITE = 0x04;
                }

            internal class MemoryAllocType
                {
                public const UInt32 COMMIT = 0x1000;
                public const UInt32 RELEASE = 0x8000;
                }




            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern IntPtr VirtualAllocEx(
                IntPtr hProcess,
                IntPtr lpAddress,
                UIntPtr dwSize,
                uint flAllocationType,
                uint flProtect);

            [DllImport("kernel32.dll")]
            public static extern bool VirtualFreeEx(
                IntPtr hProcess,
                IntPtr lpAddress,
                UIntPtr dwSize,
                UInt32 dwFreeType);

            }
        }
    //method shortcuts
    ////////////////////////////////////////////////
    public class AllFunctions
        {
        /// <summary>
        /// Select a process to be opened.
        /// </summary>
        public Process ReadProcess
            {
            get
                {
                return m_ReadProcess;
                }
            set
                {
                m_ReadProcess = value;
                }
            }

        private Process m_ReadProcess = null;

        private IntPtr m_hProcess = IntPtr.Zero;
        ///
        /// <summary>
        /// Opens the specified process
        /// Specify it by declaring a Process array and assigning it via GetProcessesByName
        /// then using ReadProcess = myProcess[0];.
        /// </summary>
        /// 
        public void OpenProcess()
            {
            UNUSED_APIS.ProcessMemoryReaderApi.ProcessAccessType access;
            access = UNUSED_APIS.ProcessMemoryReaderApi.ProcessAccessType.PROCESS_VM_READ
                | UNUSED_APIS.ProcessMemoryReaderApi.ProcessAccessType.PROCESS_VM_WRITE
                | UNUSED_APIS.ProcessMemoryReaderApi.ProcessAccessType.PROCESS_VM_OPERATION;
            m_hProcess = UNUSED_APIS.ProcessMemoryReaderApi.OpenProcess((uint)access, 1, (uint)m_ReadProcess.Id);
            }
        ///
        /// <summary>
        /// Closes the handle, saving memory. Do this after you Write, Read, Allocate space, or DeAllocate space.
        /// </summary>
        /// 
        public void CloseHandle()
            {
            int iRetValue;
            iRetValue = UNUSED_APIS.ProcessMemoryReaderApi.CloseHandle(m_hProcess);
            if (iRetValue == 0)
                throw new Exception("CloseHandle failed");
            }
        ///
        /// <summary>
        /// Reads 'MemoryAddress' from the previously opened process and can assign it to a byte array.
        /// uint bytesToRead represents the size of memory to read. Examples: int-4, long-8, double and float-8.
        /// </summary>
        /// <param name="MemoryAddress"></param>
        /// <param name="bytesToRead"></param>
        /// <param name="bytesRead"></param>
        /// <returns></returns>
        /// 
        public byte[] ReadProcessMemory(IntPtr MemoryAddress, uint bytesToRead)
            {
            byte[] buffer = new byte[bytesToRead];

            IntPtr ptrBytesRead;
            UNUSED_APIS.ProcessMemoryReaderApi.ReadProcessMemory(m_hProcess, MemoryAddress, buffer, bytesToRead, out ptrBytesRead);
            //bytesRead = ptrBytesRead.ToInt32();
            return buffer;
            }
        ///
        /// <summary>
        /// Writes 'bytesToWrite' to 'MemoryAddress' in the previously open process.
        /// </summary>
        /// <param name="MemoryAddress"></param>
        /// <param name="bytesToWrite"></param>
        /// <param name="bytesWritten"></param>
        /// 
        public void WriteProcessMemory(IntPtr MemoryAddress, byte[] bytesToWrite)
            {
            IntPtr ptrBytesWritten;
            UNUSED_APIS.ProcessMemoryReaderApi.WriteProcessMemory(m_hProcess, MemoryAddress, bytesToWrite, (uint)bytesToWrite.Length, out ptrBytesWritten);
            // bytesWritten = ptrBytesWritten.ToInt32();
            }

        //public declarations used to free the memory and for use after allocation
        public IntPtr lpAddress;
        public uint sizeAllocated;
        ///
        /// <summary>
        /// Allocates memory, in a uint size, but not at a specified address.
        /// </summary>
        /// <param name="sizeToAlloc"></param>
        /// 
        public void VirtualAllocEx(uint sizeToAlloc)
            {
            sizeAllocated = sizeToAlloc;
            lpAddress = UNUSED_APIS.Kernel32Api.VirtualAllocEx(m_hProcess, IntPtr.Zero, new UIntPtr(sizeAllocated), UNUSED_APIS.Kernel32Api.MemoryAllocType.COMMIT, UNUSED_APIS.Kernel32Api.MemoryProtection.PAGE_READWRITE);
            }
        ///
        /// <summary>
        /// Frees the LAST allocated chunk of memory.
        /// </summary>
        /// 
        public void VirtualFreeEx()
            {
            UNUSED_APIS.Kernel32Api.VirtualFreeEx(m_hProcess, lpAddress, UIntPtr.Zero, UNUSED_APIS.Kernel32Api.MemoryAllocType.RELEASE);
            }
        }
    }