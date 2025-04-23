#!/usr/bin/env python3
"""
Manually remove duplicate commits from Git history
Creates a rebase script to drop duplicate commits
"""
import subprocess
import sys
from collections import OrderedDict

def run(cmd):
    result = subprocess.run(cmd, shell=True, capture_output=True, text=True, encoding='utf-8')
    return result.stdout.strip(), result.returncode

print("=" * 70)
print("Manual Duplicate Commit Removal")
print("=" * 70)

# Step 1: Get all commits in order (oldest first)
print("\n[1/5] Getting all commits...")
cmd = 'git log --format="%H|%s|%an|%ae|%T" --reverse'
output, _ = run(cmd)

commits = []
for line in output.split('\n'):
    if not line or '|' not in line:
        continue
    parts = line.split('|')
    if len(parts) >= 5:
        commits.append({
            'hash': parts[0],
            'subject': parts[1],
            'author': parts[2],
            'email': parts[3],
            'tree': parts[4]
        })

print(f"   Total commits found: {len(commits)}")

# Step 2: Identify duplicates
print("\n[2/5] Identifying duplicates...")
seen = OrderedDict()
duplicates = []
unique_hashes = []

for commit in commits:
    # Key: message + author + tree (actual content)
    key = f"{commit['subject']}|{commit['email']}|{commit['tree']}"

    if key not in seen:
        seen[key] = commit
        unique_hashes.append(commit['hash'])
    else:
        duplicates.append({
            'hash': commit['hash'],
            'subject': commit['subject'],
            'original': seen[key]['hash']
        })

print(f"   Unique commits: {len(unique_hashes)}")
print(f"   Duplicate commits: {len(duplicates)}")

# Step 3: Show some examples
print(f"\n[3/5] Sample duplicates to be removed:")
for i, dup in enumerate(duplicates[:10]):
    print(f"   {i+1}. {dup['hash'][:8]} - {dup['subject'][:60]}")
if len(duplicates) > 10:
    print(f"   ... and {len(duplicates) - 10} more")

# Step 4: Create list of commits to drop
print(f"\n[4/5] Creating rebase todo list...")

# Get first commit (root)
first_commit, _ = run('git rev-list --max-parents=0 HEAD')

# Create a file with hashes to drop
with open('commits_to_drop.txt', 'w') as f:
    for dup in duplicates:
        f.write(f"{dup['hash']}\n")

with open('commits_to_keep.txt', 'w') as f:
    for h in unique_hashes:
        f.write(f"{h}\n")

print(f"   Created commits_to_drop.txt ({len(duplicates)} commits)")
print(f"   Created commits_to_keep.txt ({len(unique_hashes)} commits)")

# Step 5: Instructions for manual rebase
print(f"\n[5/5] Ready to remove duplicates!")
print("\n" + "=" * 70)
print("NEXT STEPS - Manual Removal")
print("=" * 70)

print(f"""
I've identified {len(duplicates)} duplicate commits to remove.

METHOD 1: Using filter-branch (Automatic - Recommended)
This will automatically remove all duplicates:

  python remove_duplicates_auto.py

METHOD 2: Manual Interactive Rebase
This lets you review each one:

  git rebase -i --root

Then in the editor, change 'pick' to 'drop' for duplicate commits.
(Check commits_to_drop.txt for the list)

METHOD 3: Create new branch from unique commits
Build fresh history with only unique commits:

  python rebuild_clean_history.py

After removal, you'll have ~{len(unique_hashes)} commits.
""")

# Save statistics
with open('cleanup_stats.txt', 'w') as f:
    f.write(f"Original commits: {len(commits)}\n")
    f.write(f"Unique commits: {len(unique_hashes)}\n")
    f.write(f"Duplicates to remove: {len(duplicates)}\n")
    f.write(f"Reduction: {(len(duplicates)/len(commits)*100):.1f}%\n")

print("\nFiles created:")
print("  - commits_to_drop.txt (duplicates)")
print("  - commits_to_keep.txt (unique)")
print("  - cleanup_stats.txt (statistics)")

print("\n" + "=" * 70)
