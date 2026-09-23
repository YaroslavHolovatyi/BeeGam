#!/usr/bin/env bash
# PostToolUse hook: catch orphaned .meta files under Assets/.
#
# Every asset Unity imports gets a sibling .meta holding its GUID. Scenes,
# prefabs and .asset files reference each other by that GUID, so deleting or
# moving an asset without its .meta leaves a stale file behind, and losing a
# .meta silently breaks every reference to that asset. Shell-level rm/mv does
# not know this; the Unity editor does it correctly on its own.
#
# Fires after Bash commands only — Write/Edit never orphan a .meta.
set -uo pipefail

cd "${CLAUDE_PROJECT_DIR:-$(dirname "$0")/../..}" || exit 0

cmd=$(jq -r '.tool_input.command // empty' 2>/dev/null)
[[ -z $cmd ]] && exit 0

# Only worth scanning after commands that can move or delete files.
grep -qE '\b(rm|mv|git mv|git rm|find .* -delete)\b' <<<"$cmd" || exit 0

orphans=$(find Assets -name '*.meta' -print0 2>/dev/null \
  | xargs -0 -I{} bash -c 'target="${1%.meta}"; [[ -e $target ]] || echo "$1"' _ {} \
  | head -20)

[[ -z $orphans ]] && exit 0

{
  echo "Orphaned .meta files under Assets/ (the asset they describe is gone):"
  echo "$orphans"
  echo
  echo "Delete each orphaned .meta alongside its asset. Any scene/prefab that"
  echo "referenced the removed asset by GUID now has a broken reference —"
  echo "check whether that was intended."
} >&2
exit 2
