// Copyright (c) 2022 Angga Permana
// Copyright (c) 2020-2021 Jeffery Myers
//
// This software is provided "as-is", without any express or implied warranty. In no event
// will the authors be held liable for any damages arising from the use of this software.
//
// Permission is granted to anyone to use this software for any purpose, including commercial
// applications, and to alter it and redistribute it freely, subject to the following restrictions:
//
//   1. The origin of this software must not be misrepresented; you must not claim that you
//   wrote the original software. If you use this software in a product, an acknowledgment
//   in the product documentation would be appreciated but is not required.
//
//   2. Altered source versions must be plainly marked as such, and must not be misrepresented
//   as being the original software.
//
//   3. This notice may not be removed or altered from any source distribution.

using System.Numerics;
using System.Runtime.InteropServices;
using ImGuiNET;
using RaylibSharp.Generated;

namespace RaylibSharp;

public unsafe class ImGuiImpl : IDisposable
{
    private readonly Dictionary<ImGuiMouseCursor, MouseCursor> _cursors;
    private readonly KeyboardKey[] _keys;
    private readonly ImGuiIOPtr _imGuiIO;
    private readonly Texture _font;
    private readonly nint _context;

    private ImGuiMouseCursor _currentCursor;

    public ImGuiImpl()
    {
        _keys = Enum.GetValues<KeyboardKey>();
        _context = ImGui.CreateContext();
        ImGui.StyleColorsDark();
        ImGui.SetCurrentContext(_context);

        _imGuiIO = ImGui.GetIO();
        _imGuiIO.Fonts.AddFontDefault();
        _imGuiIO.Fonts.GetTexDataAsRGBA32(
            out byte* pixels,
            out int width,
            out int height,
            out int bytesPerPixel
        );

        _font = Raylib.LoadTextureFromImage(
            new Image
            {
                data = pixels,
                width = width,
                height = height,
                mipmaps = 1,
                format = (int)PixelFormat.PIXELFORMAT_UNCOMPRESSED_R8G8B8A8,
            }
        );
        _imGuiIO.Fonts.SetTexID((nint)_font.id);

        _cursors = new Dictionary<ImGuiMouseCursor, MouseCursor>
        {
            [ImGuiMouseCursor.Arrow] = MouseCursor.MOUSE_CURSOR_ARROW,
            [ImGuiMouseCursor.TextInput] = MouseCursor.MOUSE_CURSOR_IBEAM,
            [ImGuiMouseCursor.Hand] = MouseCursor.MOUSE_CURSOR_POINTING_HAND,
            [ImGuiMouseCursor.ResizeAll] = MouseCursor.MOUSE_CURSOR_RESIZE_ALL,
            [ImGuiMouseCursor.ResizeEW] = MouseCursor.MOUSE_CURSOR_RESIZE_EW,
            [ImGuiMouseCursor.ResizeNESW] = MouseCursor.MOUSE_CURSOR_RESIZE_NESW,
            [ImGuiMouseCursor.ResizeNS] = MouseCursor.MOUSE_CURSOR_RESIZE_NS,
            [ImGuiMouseCursor.ResizeNWSE] = MouseCursor.MOUSE_CURSOR_RESIZE_NWSE,
            [ImGuiMouseCursor.NotAllowed] = MouseCursor.MOUSE_CURSOR_NOT_ALLOWED,
        };
        _imGuiIO.KeyMap[(int)ImGuiKey.Apostrophe] = (int)KeyboardKey.KEY_APOSTROPHE;
        _imGuiIO.KeyMap[(int)ImGuiKey.Comma] = (int)KeyboardKey.KEY_COMMA;
        _imGuiIO.KeyMap[(int)ImGuiKey.Minus] = (int)KeyboardKey.KEY_MINUS;
        _imGuiIO.KeyMap[(int)ImGuiKey.Period] = (int)KeyboardKey.KEY_PERIOD;
        _imGuiIO.KeyMap[(int)ImGuiKey.Slash] = (int)KeyboardKey.KEY_SLASH;
        _imGuiIO.KeyMap[(int)ImGuiKey._0] = (int)KeyboardKey.KEY_ZERO;
        _imGuiIO.KeyMap[(int)ImGuiKey._1] = (int)KeyboardKey.KEY_ONE;
        _imGuiIO.KeyMap[(int)ImGuiKey._2] = (int)KeyboardKey.KEY_TWO;
        _imGuiIO.KeyMap[(int)ImGuiKey._3] = (int)KeyboardKey.KEY_THREE;
        _imGuiIO.KeyMap[(int)ImGuiKey._4] = (int)KeyboardKey.KEY_FOUR;
        _imGuiIO.KeyMap[(int)ImGuiKey._5] = (int)KeyboardKey.KEY_FIVE;
        _imGuiIO.KeyMap[(int)ImGuiKey._6] = (int)KeyboardKey.KEY_SIX;
        _imGuiIO.KeyMap[(int)ImGuiKey._7] = (int)KeyboardKey.KEY_SEVEN;
        _imGuiIO.KeyMap[(int)ImGuiKey._8] = (int)KeyboardKey.KEY_EIGHT;
        _imGuiIO.KeyMap[(int)ImGuiKey._9] = (int)KeyboardKey.KEY_NINE;
        _imGuiIO.KeyMap[(int)ImGuiKey.Semicolon] = (int)KeyboardKey.KEY_SEMICOLON;
        _imGuiIO.KeyMap[(int)ImGuiKey.Equal] = (int)KeyboardKey.KEY_EQUAL;
        _imGuiIO.KeyMap[(int)ImGuiKey.A] = (int)KeyboardKey.KEY_A;
        _imGuiIO.KeyMap[(int)ImGuiKey.B] = (int)KeyboardKey.KEY_B;
        _imGuiIO.KeyMap[(int)ImGuiKey.C] = (int)KeyboardKey.KEY_C;
        _imGuiIO.KeyMap[(int)ImGuiKey.D] = (int)KeyboardKey.KEY_D;
        _imGuiIO.KeyMap[(int)ImGuiKey.E] = (int)KeyboardKey.KEY_E;
        _imGuiIO.KeyMap[(int)ImGuiKey.F] = (int)KeyboardKey.KEY_F;
        _imGuiIO.KeyMap[(int)ImGuiKey.G] = (int)KeyboardKey.KEY_G;
        _imGuiIO.KeyMap[(int)ImGuiKey.H] = (int)KeyboardKey.KEY_H;
        _imGuiIO.KeyMap[(int)ImGuiKey.I] = (int)KeyboardKey.KEY_I;
        _imGuiIO.KeyMap[(int)ImGuiKey.J] = (int)KeyboardKey.KEY_J;
        _imGuiIO.KeyMap[(int)ImGuiKey.K] = (int)KeyboardKey.KEY_K;
        _imGuiIO.KeyMap[(int)ImGuiKey.L] = (int)KeyboardKey.KEY_L;
        _imGuiIO.KeyMap[(int)ImGuiKey.M] = (int)KeyboardKey.KEY_M;
        _imGuiIO.KeyMap[(int)ImGuiKey.N] = (int)KeyboardKey.KEY_N;
        _imGuiIO.KeyMap[(int)ImGuiKey.O] = (int)KeyboardKey.KEY_O;
        _imGuiIO.KeyMap[(int)ImGuiKey.P] = (int)KeyboardKey.KEY_P;
        _imGuiIO.KeyMap[(int)ImGuiKey.Q] = (int)KeyboardKey.KEY_Q;
        _imGuiIO.KeyMap[(int)ImGuiKey.R] = (int)KeyboardKey.KEY_R;
        _imGuiIO.KeyMap[(int)ImGuiKey.S] = (int)KeyboardKey.KEY_S;
        _imGuiIO.KeyMap[(int)ImGuiKey.T] = (int)KeyboardKey.KEY_T;
        _imGuiIO.KeyMap[(int)ImGuiKey.U] = (int)KeyboardKey.KEY_U;
        _imGuiIO.KeyMap[(int)ImGuiKey.V] = (int)KeyboardKey.KEY_V;
        _imGuiIO.KeyMap[(int)ImGuiKey.W] = (int)KeyboardKey.KEY_W;
        _imGuiIO.KeyMap[(int)ImGuiKey.X] = (int)KeyboardKey.KEY_X;
        _imGuiIO.KeyMap[(int)ImGuiKey.Y] = (int)KeyboardKey.KEY_Y;
        _imGuiIO.KeyMap[(int)ImGuiKey.Z] = (int)KeyboardKey.KEY_Z;
        _imGuiIO.KeyMap[(int)ImGuiKey.LeftBracket] = (int)KeyboardKey.KEY_LEFT_BRACKET;
        _imGuiIO.KeyMap[(int)ImGuiKey.Backslash] = (int)KeyboardKey.KEY_BACKSLASH;
        _imGuiIO.KeyMap[(int)ImGuiKey.RightBracket] = (int)KeyboardKey.KEY_RIGHT_BRACKET;
        _imGuiIO.KeyMap[(int)ImGuiKey.GraveAccent] = (int)KeyboardKey.KEY_GRAVE;
        _imGuiIO.KeyMap[(int)ImGuiKey.Space] = (int)KeyboardKey.KEY_SPACE;
        _imGuiIO.KeyMap[(int)ImGuiKey.Escape] = (int)KeyboardKey.KEY_ESCAPE;
        _imGuiIO.KeyMap[(int)ImGuiKey.Enter] = (int)KeyboardKey.KEY_ENTER;
        _imGuiIO.KeyMap[(int)ImGuiKey.Tab] = (int)KeyboardKey.KEY_TAB;
        _imGuiIO.KeyMap[(int)ImGuiKey.Backspace] = (int)KeyboardKey.KEY_BACKSPACE;
        _imGuiIO.KeyMap[(int)ImGuiKey.Insert] = (int)KeyboardKey.KEY_INSERT;
        _imGuiIO.KeyMap[(int)ImGuiKey.Delete] = (int)KeyboardKey.KEY_DELETE;
        _imGuiIO.KeyMap[(int)ImGuiKey.RightArrow] = (int)KeyboardKey.KEY_RIGHT;
        _imGuiIO.KeyMap[(int)ImGuiKey.LeftArrow] = (int)KeyboardKey.KEY_LEFT;
        _imGuiIO.KeyMap[(int)ImGuiKey.DownArrow] = (int)KeyboardKey.KEY_DOWN;
        _imGuiIO.KeyMap[(int)ImGuiKey.UpArrow] = (int)KeyboardKey.KEY_UP;
        _imGuiIO.KeyMap[(int)ImGuiKey.PageUp] = (int)KeyboardKey.KEY_PAGE_UP;
        _imGuiIO.KeyMap[(int)ImGuiKey.PageDown] = (int)KeyboardKey.KEY_PAGE_DOWN;
        _imGuiIO.KeyMap[(int)ImGuiKey.Home] = (int)KeyboardKey.KEY_HOME;
        _imGuiIO.KeyMap[(int)ImGuiKey.End] = (int)KeyboardKey.KEY_END;
        _imGuiIO.KeyMap[(int)ImGuiKey.CapsLock] = (int)KeyboardKey.KEY_CAPS_LOCK;
        _imGuiIO.KeyMap[(int)ImGuiKey.ScrollLock] = (int)KeyboardKey.KEY_SCROLL_LOCK;
        _imGuiIO.KeyMap[(int)ImGuiKey.NumLock] = (int)KeyboardKey.KEY_NUM_LOCK;
        _imGuiIO.KeyMap[(int)ImGuiKey.PrintScreen] = (int)KeyboardKey.KEY_PRINT_SCREEN;
        _imGuiIO.KeyMap[(int)ImGuiKey.Pause] = (int)KeyboardKey.KEY_PAUSE;
        _imGuiIO.KeyMap[(int)ImGuiKey.F1] = (int)KeyboardKey.KEY_F1;
        _imGuiIO.KeyMap[(int)ImGuiKey.F2] = (int)KeyboardKey.KEY_F2;
        _imGuiIO.KeyMap[(int)ImGuiKey.F3] = (int)KeyboardKey.KEY_F3;
        _imGuiIO.KeyMap[(int)ImGuiKey.F4] = (int)KeyboardKey.KEY_F4;
        _imGuiIO.KeyMap[(int)ImGuiKey.F5] = (int)KeyboardKey.KEY_F5;
        _imGuiIO.KeyMap[(int)ImGuiKey.F6] = (int)KeyboardKey.KEY_F6;
        _imGuiIO.KeyMap[(int)ImGuiKey.F7] = (int)KeyboardKey.KEY_F7;
        _imGuiIO.KeyMap[(int)ImGuiKey.F8] = (int)KeyboardKey.KEY_F8;
        _imGuiIO.KeyMap[(int)ImGuiKey.F9] = (int)KeyboardKey.KEY_F9;
        _imGuiIO.KeyMap[(int)ImGuiKey.F10] = (int)KeyboardKey.KEY_F10;
        _imGuiIO.KeyMap[(int)ImGuiKey.F11] = (int)KeyboardKey.KEY_F11;
        _imGuiIO.KeyMap[(int)ImGuiKey.F12] = (int)KeyboardKey.KEY_F12;
        _imGuiIO.KeyMap[(int)ImGuiKey.LeftShift] = (int)KeyboardKey.KEY_LEFT_SHIFT;
        _imGuiIO.KeyMap[(int)ImGuiKey.LeftCtrl] = (int)KeyboardKey.KEY_LEFT_CONTROL;
        _imGuiIO.KeyMap[(int)ImGuiKey.LeftAlt] = (int)KeyboardKey.KEY_LEFT_ALT;
        _imGuiIO.KeyMap[(int)ImGuiKey.LeftSuper] = (int)KeyboardKey.KEY_LEFT_SUPER;
        _imGuiIO.KeyMap[(int)ImGuiKey.RightShift] = (int)KeyboardKey.KEY_RIGHT_SHIFT;
        _imGuiIO.KeyMap[(int)ImGuiKey.RightCtrl] = (int)KeyboardKey.KEY_RIGHT_CONTROL;
        _imGuiIO.KeyMap[(int)ImGuiKey.RightAlt] = (int)KeyboardKey.KEY_RIGHT_ALT;
        _imGuiIO.KeyMap[(int)ImGuiKey.RightSuper] = (int)KeyboardKey.KEY_RIGHT_SUPER;
        _imGuiIO.KeyMap[(int)ImGuiKey.Keypad0] = (int)KeyboardKey.KEY_KP_0;
        _imGuiIO.KeyMap[(int)ImGuiKey.Keypad1] = (int)KeyboardKey.KEY_KP_1;
        _imGuiIO.KeyMap[(int)ImGuiKey.Keypad2] = (int)KeyboardKey.KEY_KP_2;
        _imGuiIO.KeyMap[(int)ImGuiKey.Keypad3] = (int)KeyboardKey.KEY_KP_3;
        _imGuiIO.KeyMap[(int)ImGuiKey.Keypad4] = (int)KeyboardKey.KEY_KP_4;
        _imGuiIO.KeyMap[(int)ImGuiKey.Keypad5] = (int)KeyboardKey.KEY_KP_5;
        _imGuiIO.KeyMap[(int)ImGuiKey.Keypad6] = (int)KeyboardKey.KEY_KP_6;
        _imGuiIO.KeyMap[(int)ImGuiKey.Keypad7] = (int)KeyboardKey.KEY_KP_7;
        _imGuiIO.KeyMap[(int)ImGuiKey.Keypad8] = (int)KeyboardKey.KEY_KP_8;
        _imGuiIO.KeyMap[(int)ImGuiKey.Keypad9] = (int)KeyboardKey.KEY_KP_9;
        _imGuiIO.KeyMap[(int)ImGuiKey.KeypadDecimal] = (int)KeyboardKey.KEY_KP_DECIMAL;
        _imGuiIO.KeyMap[(int)ImGuiKey.KeypadDivide] = (int)KeyboardKey.KEY_KP_DIVIDE;
        _imGuiIO.KeyMap[(int)ImGuiKey.KeypadMultiply] = (int)KeyboardKey.KEY_KP_MULTIPLY;
        _imGuiIO.KeyMap[(int)ImGuiKey.KeypadSubtract] = (int)KeyboardKey.KEY_KP_SUBTRACT;
        _imGuiIO.KeyMap[(int)ImGuiKey.KeypadAdd] = (int)KeyboardKey.KEY_KP_ADD;
        _imGuiIO.KeyMap[(int)ImGuiKey.KeypadEnter] = (int)KeyboardKey.KEY_KP_ENTER;
        _imGuiIO.KeyMap[(int)ImGuiKey.KeypadEqual] = (int)KeyboardKey.KEY_KP_EQUAL;
    }

    public void Begin()
    {
        ImGui.SetCurrentContext(_context);

        if (Raylib.IsWindowFullscreen())
        {
            int monitor = Raylib.GetCurrentMonitor();
            _imGuiIO.DisplaySize = new(
                Raylib.GetMonitorWidth(monitor),
                Raylib.GetMonitorHeight(monitor)
            );
        }
        else
            _imGuiIO.DisplaySize = new(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());

        _imGuiIO.DisplayFramebufferScale = Vector2.One;
        _imGuiIO.DeltaTime = Raylib.GetFrameTime();

        _imGuiIO.KeyCtrl =
            Raylib.IsKeyDown((int)KeyboardKey.KEY_RIGHT_CONTROL)
            || Raylib.IsKeyDown((int)KeyboardKey.KEY_LEFT_CONTROL);
        _imGuiIO.KeyShift =
            Raylib.IsKeyDown((int)KeyboardKey.KEY_RIGHT_SHIFT)
            || Raylib.IsKeyDown((int)KeyboardKey.KEY_LEFT_SHIFT);
        _imGuiIO.KeyAlt =
            Raylib.IsKeyDown((int)KeyboardKey.KEY_RIGHT_ALT)
            || Raylib.IsKeyDown((int)KeyboardKey.KEY_LEFT_ALT);
        _imGuiIO.KeySuper =
            Raylib.IsKeyDown((int)KeyboardKey.KEY_RIGHT_SUPER)
            || Raylib.IsKeyDown((int)KeyboardKey.KEY_LEFT_SUPER);

        if (_imGuiIO.WantSetMousePos)
            Raylib.SetMousePosition((int)_imGuiIO.MousePos.X, (int)_imGuiIO.MousePos.Y);
        else
            _imGuiIO.MousePos = Raylib.GetMousePosition();

        foreach (int button in Enum.GetValues<MouseButton>())
        {
            if (button >= _imGuiIO.MouseDown.Count)
                break;
            _imGuiIO.MouseDown[button] = Raylib.IsMouseButtonDown(button);
        }

        if (Raylib.GetMouseWheelMove() > 0)
            _imGuiIO.MouseWheel += 1;
        else if (Raylib.GetMouseWheelMove() < 0)
            _imGuiIO.MouseWheel -= 1;

        if ((_imGuiIO.ConfigFlags & ImGuiConfigFlags.NoMouseCursorChange) == 0)
        {
            var cursor = ImGui.GetMouseCursor();
            if (cursor != _currentCursor || _imGuiIO.MouseDrawCursor)
            {
                _currentCursor = cursor;
                if (!_imGuiIO.MouseDrawCursor && cursor != ImGuiMouseCursor.None)
                {
                    Raylib.ShowCursor();

                    if ((_imGuiIO.ConfigFlags & ImGuiConfigFlags.NoMouseCursorChange) == 0)
                    {
                        if (!_cursors.ContainsKey(cursor))
                            Raylib.SetMouseCursor((int)MouseCursor.MOUSE_CURSOR_DEFAULT);
                        else
                            Raylib.SetMouseCursor((int)_cursors[cursor]);
                    }
                }
                else
                    Raylib.HideCursor();
            }
        }

        foreach (int key in _keys)
            _imGuiIO.KeysDown[key] = Raylib.IsKeyDown(key);

        var pressed = (uint)Raylib.GetCharPressed();
        while (pressed != 0)
        {
            _imGuiIO.AddInputCharacter(pressed);
            pressed = (uint)Raylib.GetCharPressed();
        }

        ImGui.NewFrame();
    }

    private void EnableScissor(float x, float y, float width, float height)
    {
        Raylib.rlEnableScissorTest();
        Raylib.rlScissor(
            (int)x,
            Raylib.GetScreenHeight() - (int)(y + height),
            (int)width,
            (int)height
        );
    }

    private void TriangleVert(ImDrawVertPtr idx_vert)
    {
        var color = ImGui.ColorConvertU32ToFloat4(idx_vert.col);
        Raylib.rlColor4f(color.X, color.Y, color.Z, color.W);
        Raylib.rlTexCoord2f(idx_vert.uv.X, idx_vert.uv.Y);
        Raylib.rlVertex2f(idx_vert.pos.X, idx_vert.pos.Y);
    }

    private void RenderTriangles(
        uint count,
        uint indexStart,
        ImVector<ushort> indexBuffer,
        ImPtrVector<ImDrawVertPtr> vertBuffer,
        IntPtr texturePtr
    )
    {
        if (count < 3)
            return;

        var textureId = 0u;
        if (texturePtr != IntPtr.Zero)
            textureId = (uint)texturePtr.ToInt32();

        Raylib.rlBegin(0x0004);
        Raylib.rlSetTexture(textureId);

        for (var i = 0; i <= (count - 3); i += 3)
        {
            if (Raylib.rlCheckRenderBatchLimit(3))
            {
                Raylib.rlBegin(0x0004);
                Raylib.rlSetTexture(textureId);
            }

            var indexA = indexBuffer[(int)indexStart + i];
            var indexB = indexBuffer[(int)indexStart + i + 1];
            var indexC = indexBuffer[(int)indexStart + i + 2];
            var vertexA = vertBuffer[indexA];
            var vertexB = vertBuffer[indexB];
            var vertexC = vertBuffer[indexC];

            TriangleVert(vertexA);
            TriangleVert(vertexB);
            TriangleVert(vertexC);
        }
        Raylib.rlEnd();
    }

    private void RenderData()
    {
        Raylib.rlDrawRenderBatchActive();
        Raylib.rlDisableBackfaceCulling();

        var data = ImGui.GetDrawData();

        for (int l = 0; l < data.CmdListsCount; l++)
        {
            ImDrawListPtr commandList = data.CmdListsRange[l];

            for (int cmdIndex = 0; cmdIndex < commandList.CmdBuffer.Size; cmdIndex++)
            {
                var cmd = commandList.CmdBuffer[cmdIndex];

                EnableScissor(
                    cmd.ClipRect.X - data.DisplayPos.X,
                    cmd.ClipRect.Y - data.DisplayPos.Y,
                    cmd.ClipRect.Z - (cmd.ClipRect.X - data.DisplayPos.X),
                    cmd.ClipRect.W - (cmd.ClipRect.Y - data.DisplayPos.Y)
                );
                if (cmd.UserCallback != IntPtr.Zero)
                {
                    var callback = Marshal.GetDelegateForFunctionPointer<
                        Action<ImDrawListPtr, ImDrawCmdPtr>
                    >(cmd.UserCallback);
                    callback?.Invoke(commandList, cmd);
                    continue;
                }

                RenderTriangles(
                    cmd.ElemCount,
                    cmd.IdxOffset,
                    commandList.IdxBuffer,
                    commandList.VtxBuffer,
                    cmd.TextureId
                );

                Raylib.rlDrawRenderBatchActive();
            }
        }

        Raylib.rlSetTexture(0);
        Raylib.rlDisableScissorTest();
        Raylib.rlEnableBackfaceCulling();
    }

    public void End()
    {
        ImGui.SetCurrentContext(_context);
        ImGui.Render();
        RenderData();
    }

    public void DrawTexture(Texture image)
    {
        ImGui.Image((nint)image.id, new Vector2(image.width, image.height));
    }

    public void DrawTexture(Texture image, int width, int height)
    {
        ImGui.Image((nint)image.id, new Vector2(width, height));
    }

    public void DrawTexture(Texture image, Vector2 size)
    {
        ImGui.Image((nint)image.id, size);
    }

    public void DrawTexture(Texture image, int destWidth, int destHeight, Rectangle sourceRect)
    {
        var uv0 = Vector2.Zero;
        var uv1 = Vector2.Zero;

        if (sourceRect.width < 0)
        {
            uv0.X = -((float)sourceRect.x / image.width);
            uv1.X = (uv0.X - (float)(Math.Abs(sourceRect.width) / image.width));
        }
        else
        {
            uv0.X = (float)sourceRect.x / image.width;
            uv1.X = uv0.X + (float)(sourceRect.width / image.width);
        }

        if (sourceRect.height < 0)
        {
            uv0.Y = -((float)sourceRect.y / image.height);
            uv1.Y = (uv0.Y - (float)(Math.Abs(sourceRect.height) / image.height));
        }
        else
        {
            uv0.Y = (float)sourceRect.y / image.height;
            uv1.Y = uv0.Y + (float)(sourceRect.height / image.height);
        }

        ImGui.Image((nint)image.id, new(destWidth, destHeight), uv0, uv1);
    }

    public void Dispose()
    {
        Raylib.UnloadTexture(_font);
        ImGui.DestroyContext();
    }
}
