#!/usr/bin/env python3
"""
Automatically remove duplicate commits using git filter-branch
"""
import subprocess
import sys
import os

def run(cmd, show=False):
    result = subprocess.run(cmd, shell=True, capture_output=True, text=True, encoding='utf-8')
    if show:
        print(result.stdout)
        if result.stderr:
            print(result.stderr)
    return result.stdout.strip(), result.returncode

print("=" * 70)
print("Automatic Duplicate Removal")
print("=" * 70)

# Check if commits list exists
if not os.path.exists('commits_to_drop.txt'):
    print("\nError: commits_to_drop.txt not found!")
    print("Run 'python remove_duplicates.py' first")
    sys.exit(1)

# Load commits to drop
with open('commits_to_drop.txt', 'r') as f:
    duplicates = set(line.strip() for line in f if line.strip())

with open('commits_to_keep.txt', 'r') as f:
    keeps = set(line.strip() for line in f if line.strip())

print(f"\nLoaded:")
print(f"  - {len(keeps)} commits to keep")
print(f"  - {len(duplicates)} commits to drop")

# Confirm
print(f"\nThis will:")
print(f"  1. Remove {len(duplicates)} duplicate commits")
print(f"  2. Keep {len(keeps)} unique commits")
print(f"  3. Rewrite the 'clean-history' branch")
print(f"\nWARNING: This operation rewrites Git history!")

choice = input("\nContinue? (yes/no): ").lower().strip()
if choice != 'yes':
    print("Aborted.")
    sys.exit(0)

print("\n[1/3] Creating filter script...")

# Create a bash script for filtering
filter_script = """#!/bin/bash
# Read the commit hash
commit_hash="$GIT_COMMIT"

# Check if this commit should be dropped
while IFS= read -r drop_hash; do
    if [ "$commit_hash" = "$drop_hash" ]; then
        # Skip this commit entirely
        skip_commit "$@"
        exit 0
    fi
done < commits_to_drop.txt

# Keep this commit
git commit-tree "$@"
"""

with open('commit_filter.sh', 'w') as f:
    f.write(filter_script)

print("   Created commit_filter.sh")

print("\n[2/3] Running git filter-branch...")
print("   This may take several minutes...")

# Run filter-branch
cmd = 'git filter-branch --commit-filter "bash ../commit_filter.sh" HEAD'
output, code = run(cmd, show=True)

if code != 0:
    print("\n[!] Filter-branch failed. Trying alternative method...")

    # Alternative: Use rebase with exec
    print("\n[2/3] Using git rebase method...")

    # Get root commit
    root, _ = run('git rev-list --max-parents=0 HEAD')

    # Create rebase script
    print("   Creating rebase instructions...")

    cmd = f'git log --format="%H" --reverse'
    all_commits, _ = run(cmd)

    rebase_todo = []
    for commit_hash in all_commits.split('\n'):
        if commit_hash in duplicates:
            # Skip duplicate commits
            continue
        elif commit_hash in keeps:
            short_hash = commit_hash[:8]
            rebase_todo.append(f"pick {short_hash}")

    # Write rebase todo
    with open('rebase_todo.txt', 'w') as f:
        f.write('\n'.join(rebase_todo))

    print(f"   Created rebase plan with {len(rebase_todo)} commits")
    print("\n   NOTE: Automatic rebase is complex for this history.")
    print("   Please use METHOD 3 instead (rebuild_clean_history.py)")

    sys.exit(1)

print("\n[3/3] Verifying...")

final_count, _ = run('git rev-list --count HEAD')

print("\n" + "=" * 70)
print("SUCCESS!")
print("=" * 70)

print(f"\nBranch: clean-history")
print(f"Original commits: 595")
print(f"Final commits: {final_count}")
print(f"Removed: {595 - int(final_count)}")

print(f"\nNext steps:")
print(f"  1. Verify: git log --oneline | head -20")
print(f"  2. Push: git push origin clean-history --force")
print(f"  3. Or continue editing if needed")

print("\n" + "=" * 70)
