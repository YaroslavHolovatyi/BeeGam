#!/usr/bin/env bash
# PostToolUse hook: compile the Unity C# assemblies after a script edit.
#
# Uses the .NET SDK bundled with the Unity editor, so there is no separate
# toolchain to install. Build output goes to Temp/ (gitignored) and does not
# touch Library/, so this is safe to run while the Unity editor is open.
#
# Exit 2 + stderr => errors are fed back to Claude to fix.
set -uo pipefail

cd "${CLAUDE_PROJECT_DIR:-$(dirname "$0")/../..}" || exit 0

file=$(jq -r '.tool_response.filePath // .tool_input.file_path // empty' 2>/dev/null)
[[ -z $file ]] && exit 0

# Only C# under Assets/. Everything else (docs, .asset, .unity) is not compiled.
case $file in
  *Assets/*.cs) ;;
  *) exit 0 ;;
esac

version=$(sed -n 's/^m_EditorVersion: //p' ProjectSettings/ProjectVersion.txt 2>/dev/null)
dotnet="$HOME/Unity/Hub/Editor/$version/Editor/Data/DotNetSdk/dotnet"
if [[ ! -x $dotnet ]]; then
  # No Unity toolchain here (fresh clone, different install path) — stay silent
  # rather than nagging on every edit.
  exit 0
fi

# Editor-only scripts live in an Editor/ folder and compile into the Editor
# assembly, which references the runtime one — building it covers both.
if [[ $file == */Editor/* ]]; then
  proj=Assembly-CSharp-Editor.csproj
else
  proj=Assembly-CSharp.csproj
fi
[[ -f $proj ]] || exit 0

export DOTNET_CLI_TELEMETRY_OPTOUT=1 DOTNET_NOLOGO=1
out=$("$dotnet" build "$proj" -nologo -v q -nodeReuse:false 2>&1)
status=$?
[[ $status -eq 0 ]] && exit 0

echo "Unity C# compile failed ($proj):" >&2
# MSBuild reports each diagnostic once per target that saw it; collapse repeats.
grep -E "error [A-Z]+[0-9]+" <<<"$out" | awk '!seen[$0]++' | head -20 >&2 \
  || echo "$out" | tail -20 >&2
exit 2
