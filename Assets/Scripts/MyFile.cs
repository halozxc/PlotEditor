using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MyFile : MonoBehaviour
{
    

    public static string   SaveGame()
    {
        SaveFileDlg pth = new SaveFileDlg();
               pth.structSize = System.Runtime.InteropServices.Marshal.SizeOf(pth);
               pth.filter = "data (*.dat)";
               pth.file = new string(new char[256]);
               pth.maxFile = pth.file.Length;
               pth.fileTitle = new string(new char[64]);
               pth.maxFileTitle = pth.fileTitle.Length;
               pth.initialDir = Application.dataPath;  // default path  
               pth.title = "保存项目";
               pth.defExt = "dat";
               pth.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
               if (SaveFileDialog.GetSaveFileName(pth))
               {
                   string filepath = pth.file;//选择的文件路径;  
                   return filepath;
               }

               return null;
    }
    public static string AddGame()
    {
        OpenFileDlg pth = new OpenFileDlg();
        string filepath = null;
        pth.structSize = System.Runtime.InteropServices.Marshal.SizeOf(pth);
        
        //pth.filter = "exe (*.exe)";
        pth.file = new string(new char[256]);
        pth.maxFile = pth.file.Length;
        pth.fileTitle = new string(new char[64]);
        pth.maxFileTitle = pth.fileTitle.Length;
        pth.initialDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);  // default path  
        pth.title = "请选择游戏";
        pth.defExt = "exe";
        pth.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
        //0x00080000   是否使用新版文件选择窗口
        //0x00000200   是否可以多选文件
        if (OpenFileDialog.GetOpenFileName(pth))
        {
            filepath = pth.file;//选择的文件路径;  
        }
        Debug.Log(filepath);
        return filepath;
    }
   
}
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class FileDlg
{
    public int structSize = 0;
    public IntPtr dlgOwner = IntPtr.Zero;
    public IntPtr instance = IntPtr.Zero;
    public String filter = null;
    public String customFilter = null;
    public int maxCustFilter = 0;
    public int filterIndex = 0;
    public String file = null;
    public int maxFile = 0;
    public String fileTitle = null;
    public int maxFileTitle = 0;
    public String initialDir = null;
    public String title = null;
    public int flags = 0;
    public short fileOffset = 0;
    public short fileExtension = 0;
    public String defExt = null;
    public IntPtr custData = IntPtr.Zero;
    public IntPtr hook = IntPtr.Zero;
    public String templateName = null;
    public IntPtr reservedPtr = IntPtr.Zero;
    public int reservedInt = 0;
    public int flagsEx = 0;
}
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class OpenFileDlg : FileDlg
{

}
public class OpenFileDialog
{
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([In, Out] OpenFileDlg ofd);
}
public class SaveFileDialog
{
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetSaveFileName([In, Out] SaveFileDlg ofd);
}
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class SaveFileDlg : FileDlg
{

}
