using System;
using System.Collections.Generic;
using System.Text;

namespace Haozes.Robot
{
    public delegate void HotkeyEventHandler(int HotKeyID);
    ///   <summary>   
    ///   System   wide   hotkey   wrapper.     
    ///       
    ///   Robert   Jeppesen   
    ///   Send   bugs   to   robert@durius.com   
    ///   </summary>   
    public class Hotkey : System.Windows.Forms.IMessageFilter
    {
        System.Collections.Hashtable keyIDs = new System.Collections.Hashtable();
        IntPtr hWnd;
        ///   <summary>   
        ///   Occurs   when   a   hotkey   has   been   pressed.   
        ///   </summary>   
        public event HotkeyEventHandler OnHotkey;
        public enum KeyFlags
        {
            MOD_ALT = 0x1,
            MOD_CONTROL = 0x2,
            MOD_SHIFT = 0x4,
            MOD_WIN = 0x8
        }
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern UInt32 RegisterHotKey(IntPtr hWnd, UInt32 id,
        UInt32 fsModifiers, UInt32 vk);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern UInt32 UnregisterHotKey(IntPtr hWnd, UInt32 id);
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern UInt32 GlobalAddAtom(String lpString);
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern UInt32 GlobalDeleteAtom(UInt32 nAtom);
        ///   <summary>   
        ///   Constructor.     Adds   this   instance   to   the   MessageFilters   so   that   this   class   can   raise   Hotkey   events   
        ///   </summary>   
        ///   <param   name="hWnd">A   valid   hWnd,   i.e.   form1.Handle</param>   
        public Hotkey(IntPtr hWnd)
        {
            this.hWnd = hWnd;
            System.Windows.Forms.Application.AddMessageFilter(this);
        }
        ///   <summary>   
        ///   Register   a   system   wide   hotkey.   
        ///   </summary>   
        ///   <param   name="hWnd">form1.Handle</param>   
        ///   <param   name="Key">Your   hotkey</param>   
        ///   <returns>ID   integer   for   your   hotkey.   Use   this   to   know   which   hotkey   was   pressed.</returns>   
        public int RegisterHotkey(System.Windows.Forms.Keys Key, KeyFlags keyflags)
        {
            UInt32 hotkeyid = GlobalAddAtom(System.Guid.NewGuid().ToString());
            RegisterHotKey((IntPtr)hWnd, hotkeyid, (UInt32)keyflags, (UInt32)Key);
            keyIDs.Add(hotkeyid, hotkeyid);
            return (int)hotkeyid;
        }
        ///   <summary>   
        ///   Unregister   hotkeys   and   delete   atoms.   
        ///   </summary>   
        public void UnregisterHotkeys()
        {
            System.Windows.Forms.Application.RemoveMessageFilter(this);
            foreach (UInt32 key in keyIDs.Values)
            {
                UnregisterHotKey(hWnd, key);
                GlobalDeleteAtom(key);
            }
        }
        public bool PreFilterMessage(ref   System.Windows.Forms.Message m)
        {
            if (m.Msg == 0x312)   /*WM_HOTKEY*/
            {
                if (OnHotkey != null)
                {
                    foreach (UInt32 key in keyIDs.Values)
                    {
                        if ((UInt32)m.WParam == key)
                        {
                            OnHotkey((int)m.WParam);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }  
}
