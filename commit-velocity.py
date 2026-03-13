#!/usr/bin/env python3
"""commit-velocity.py — Project velocity as change per unit time.

For each non-merge commit, computes:
  velocity = delta / capped_hours
where
  delta       = lines added + lines removed
  capped_hours = min(hours_since_prev_commit, cap)

Intervals longer than the cap (default: 168 h / 1 week) are clamped,
so dormant periods don't dilute the metric.

Usage:
  python3 commit-velocity.py [N]                  # last N commits (default 20)
  python3 commit-velocity.py --all                # full history
  python3 commit-velocity.py --cap=48             # cap at 48 hours
  python3 commit-velocity.py --author="Name"
  python3 commit-velocity.py --commit=abc123      # single commit velocity
"""

import argparse
import subprocess
import sys


def git(*args):
    r = subprocess.run(["git"] + list(args),
                       capture_output=True, text=True)
    return r.stdout.strip()


def get_commits(n, all_commits, author):
    cmd = ["log", "--format=%H %at", "--no-merges"]
    if author:
        cmd += [f"--author={author}"]
    if not all_commits:
        cmd += [f"-n{n}"]
    lines = git(*cmd).splitlines()
    commits = []
    for line in lines:
        parts = line.split()
        if len(parts) == 2:
            commits.append((parts[0], int(parts[1])))
    return commits


def diff_stat(parent, child):
    lines = git("diff", "--numstat", parent, child).splitlines()
    added = removed = 0
    for line in lines:
        parts = line.split()
        if parts[0] == "-":  # binary
            continue
        added += int(parts[0])
        removed += int(parts[1])
    return added, removed


def compute_velocity(commits, cap_hours):
    cap_sec = cap_hours * 3600
    rows = []
    for i in range(len(commits) - 1):
        sha, ts = commits[i]
        prev_sha, prev_ts = commits[i + 1]

        added, removed = diff_stat(prev_sha, sha)
        delta = added + removed

        gap_sec = ts - prev_ts
        capped = min(gap_sec, cap_sec) if gap_sec > 0 else 0
        raw_hours = gap_sec / 3600
        capped_hours = capped / 3600

        if capped > 0:
            vel = delta / capped_hours
        else:
            vel = float("inf") if delta > 0 else 0.0

        rows.append({
            "sha": sha[:8],
            "added": added,
            "removed": removed,
            "delta": delta,
            "raw_hours": raw_hours,
            "capped_hours": capped_hours,
            "capped": gap_sec > cap_sec,
            "velocity": vel,
        })
    return rows


def print_table(rows, cap_hours):
    hdr = f"{'commit':<10} {'added':>7} {'removed':>7} {'delta':>7} " \
          f"{'hours':>9} {'capped':>7} {'vel(l/h)':>10}"
    sep = "-" * len(hdr)
    print(hdr)
    print(sep)

    for r in rows:
        cap_mark = f"[{cap_hours}]" if r["capped"] else f"{r['raw_hours']:.1f}"
        vel_str = f"{r['velocity']:.1f}" if r["velocity"] != float("inf") else "inf"
        print(f"{r['sha']:<10} {r['added']:>7} {r['removed']:>7} {r['delta']:>7} "
              f"{cap_mark:>9} {('yes' if r['capped'] else ''):>7} {vel_str:>10}")


def print_summary(rows):
    if not rows:
        return
    total_delta = sum(r["delta"] for r in rows)
    total_capped = sum(r["capped_hours"] for r in rows)
    n = len(rows)

    print(f"\n--- Summary ({n} intervals) ---")
    print(f"  Total change (lines):   {total_delta}")
    print(f"  Total time (capped, h): {total_capped:.1f}")
    print(f"  Avg change per commit:  {total_delta / n:.1f} lines")
    print(f"  Avg interval (capped):  {total_capped / n:.1f} hours")
    if total_capped > 0:
        print(f"  Overall velocity:       {total_delta / total_capped:.1f} lines/hour")


def single_commit(sha, cap_hours):
    """Velocity for a specific commit relative to its parent."""
    full = git("rev-parse", sha)
    if not full:
        print(f"Commit {sha} not found.", file=sys.stderr)
        sys.exit(1)
    parent = git("rev-parse", f"{full}~1")
    if not parent:
        print(f"No parent for {sha} (initial commit?).", file=sys.stderr)
        sys.exit(1)

    ts = int(git("log", "-1", "--format=%at", full))
    pts = int(git("log", "-1", "--format=%at", parent))
    commits = [(full, ts), (parent, pts)]
    rows = compute_velocity(commits, cap_hours)
    print_table(rows, cap_hours)
    return rows


def main():
    parser = argparse.ArgumentParser(description="Commit velocity analysis")
    parser.add_argument("n", nargs="?", type=int, default=20,
                        help="Number of recent commits (default: 20)")
    parser.add_argument("--all", action="store_true",
                        help="Analyze all commits")
    parser.add_argument("--cap", type=float, default=168,
                        help="Cap interval in hours (default: 168 = 1 week)")
    parser.add_argument("--author", default="",
                        help="Filter by author name")
    parser.add_argument("--commit", default="",
                        help="Show velocity for a single commit")
    args = parser.parse_args()

    if args.commit:
        rows = single_commit(args.commit, args.cap)
        print_summary(rows)
        return

    commits = get_commits(args.n, args.all, args.author)
    if len(commits) < 2:
        print("Need at least 2 non-merge commits.")
        sys.exit(1)

    rows = compute_velocity(commits, args.cap)
    print_table(rows, args.cap)
    print_summary(rows)


if __name__ == "__main__":
    main()
