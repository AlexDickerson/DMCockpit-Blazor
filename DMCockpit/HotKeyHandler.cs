using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace DMCockpit
{
    public interface IHotKeyObersevable
    {
        event HotKeyHandlerDelegate HotKeyHandlerEvent;
    }

    public delegate void HotKeyHandlerDelegate(ModifierKeys modifier, string keysPressed);

    public enum ModifierKeys
    {
        Ctrl,
        Alt,
        Shift
    }

    public class HotKeyHandler : IHotKeyObersevable
    {
        private static Func<ModifierKeys, string, Task>? HotKeyAction;

        public event HotKeyHandlerDelegate? HotKeyHandlerEvent;

        public HotKeyHandler()
        {
            HotKeyAction = OnHotKeyPressed;
        }

        private async Task OnHotKeyPressed(ModifierKeys modifier, string keyPressed)
        {
            HotKeyHandlerDelegate? handler = HotKeyHandlerEvent;
            handler?.Invoke(modifier, keyPressed);
        }

        [JSInvokable]
        public static async Task JsKeyDown(KeyboardEventArgs e)
        {            
            ModifierKeys modifier;

            if (e.CtrlKey) modifier = ModifierKeys.Ctrl;
            else if (e.AltKey) modifier = ModifierKeys.Alt;
            else if (e.ShiftKey) modifier = ModifierKeys.Shift;
            else return;

            if (HotKeyAction is { } actionAsync)
            {
                await actionAsync(modifier, e.Key.ToLower());
            }
        }
    }
}
