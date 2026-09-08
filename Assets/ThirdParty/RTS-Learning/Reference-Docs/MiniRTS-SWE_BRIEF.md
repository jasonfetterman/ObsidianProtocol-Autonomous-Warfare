# SWE brief (Codex)

You are the software engineer on MiniRTS. Read DESIGN.md in the repo root — it is the
authoritative spec. The Unity project lives in ./MiniRTS (Unity 6000.3.20f1).

Rules of engagement:
- Work ONLY inside the MiniRTS project folder (Assets/, Packages/manifest.json if needed).
- Pure C#; no asset store, no downloads, no binary assets. Everything generated from code.
- Keep the project compiling at all times. Prefer many small, well-named files.
- Write/maintain EditMode tests for pure logic (grid, A*, economy math, combat math).
- Do not create .unity scene files by hand-editing YAML. Instead implement
  `MiniRTS.Editor.SceneGenerator.Generate()` (menu item "MiniRTS/Regenerate Main Scene",
  static, batch-callable) that programmatically builds Assets/Scenes/Main.unity with a
  single GameBootstrap object and registers it in EditorBuildSettings.
- The orchestrator (not you) runs Unity compile/test/eval verification after each milestone
  and will hand you back exact compiler errors or test failures to fix.
