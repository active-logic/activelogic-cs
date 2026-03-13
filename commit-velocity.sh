#!/bin/bash
# commit-velocity.sh — Express project velocity as commit interval
# relative to amount of change.
#
# Formula:
#   velocity = delta / hours
#   where delta = lines added + lines removed
#         hours = time between consecutive commits (in hours)
#
# A high velocity means large changes land frequently.
# A low velocity means small or infrequent changes.
#
# Usage:
#   ./commit-velocity.sh [N]        # last N commits (default: 20)
#   ./commit-velocity.sh --all      # all commits
#   ./commit-velocity.sh --author="Name"  # filter by author

set -euo pipefail

N=20
ALL=false
AUTHOR_FILTER=""

for arg in "$@"; do
  case "$arg" in
    --all)       ALL=true ;;
    --author=*)  AUTHOR_FILTER="${arg#--author=}" ;;
    [0-9]*)      N="$arg" ;;
  esac
done

# Build git log command
LOG_ARGS=(--format="%H %at" --no-merges)
if [[ -n "$AUTHOR_FILTER" ]]; then
  LOG_ARGS+=(--author="$AUTHOR_FILTER")
fi
if [[ "$ALL" == false ]]; then
  LOG_ARGS+=(-n "$N")
fi

# Collect commits: hash and unix timestamp
TMPFILE=$(mktemp)
git log "${LOG_ARGS[@]}" > "$TMPFILE"
mapfile -t COMMITS < "$TMPFILE"
rm -f "$TMPFILE"

if [[ ${#COMMITS[@]} -lt 2 ]]; then
  echo "Need at least 2 non-merge commits to compute velocity."
  exit 1
fi

# Header
printf "%-8s  %10s  %10s  %8s  %12s  %s\n" \
  "commit" "added" "removed" "delta" "hours_since" "velocity"
printf "%-8s  %10s  %10s  %8s  %12s  %s\n" \
  "--------" "----------" "----------" "--------" "------------" "------------"

total_delta=0
total_hours=0
count=0

for (( i=0; i < ${#COMMITS[@]} - 1; i++ )); do
  read -r hash ts <<< "${COMMITS[$i]}"
  read -r prev_hash ts_prev <<< "${COMMITS[$i+1]}"

  short="${hash:0:8}"

  # Lines added/removed (ignore binary files)
  diffstat=$(git diff --numstat "$prev_hash" "$hash" 2>/dev/null | grep -v "^-" || true)
  added=$(echo "$diffstat"  | awk '{s+=$1} END {print s+0}')
  removed=$(echo "$diffstat" | awk '{s+=$2} END {print s+0}')
  delta=$(( added + removed ))

  # Time gap in hours (float)
  gap_sec=$(( ts - ts_prev ))
  hours=$(awk "BEGIN {printf \"%.2f\", $gap_sec / 3600.0}")

  # Velocity: delta / hours  (guard div-by-zero)
  if (( gap_sec > 0 )); then
    vel=$(awk "BEGIN {printf \"%.1f\", $delta / ($gap_sec / 3600.0)}")
  else
    vel="inf"
  fi

  printf "%-8s  %10d  %10d  %8d  %12s  %12s\n" \
    "$short" "$added" "$removed" "$delta" "$hours" "$vel"

  total_delta=$(( total_delta + delta ))
  total_hours=$(awk "BEGIN {printf \"%.2f\", $total_hours + $hours}")
  count=$(( count + 1 ))
done

# Summary
echo ""
echo "--- Summary ($count intervals) ---"
if (( count > 0 )); then
  avg_delta=$(awk "BEGIN {printf \"%.1f\", $total_delta / $count}")
  avg_hours=$(awk "BEGIN {printf \"%.2f\", $total_hours / $count}")
  if [[ $(awk "BEGIN {print ($total_hours > 0)}") -eq 1 ]]; then
    avg_vel=$(awk "BEGIN {printf \"%.1f\", $total_delta / $total_hours}")
  else
    avg_vel="inf"
  fi
  echo "  Total change (lines):   $total_delta"
  echo "  Total time (hours):     $total_hours"
  echo "  Avg change per commit:  $avg_delta lines"
  echo "  Avg interval:           $avg_hours hours"
  echo "  Overall velocity:       $avg_vel lines/hour"
fi
