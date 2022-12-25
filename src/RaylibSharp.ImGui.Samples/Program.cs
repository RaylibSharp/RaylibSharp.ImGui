using ImGuiNET;
using RaylibSharp;
using RaylibSharp.Generated;
using Color = System.Drawing.Color;

Raylib.InitWindow(1280, 720, "ImGui");

using var ig = new ImGuiImpl();
while (!Raylib.WindowShouldClose())
{
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.CornflowerBlue);
    ig.Begin();
    ImGui.ShowDemoWindow();
    ig.End();
    Raylib.EndDrawing();
}

Raylib.CloseWindow();
