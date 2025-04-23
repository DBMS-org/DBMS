#!/usr/bin/env python3
"""
Rebuild Git history with only unique commits
This creates a new clean history from scratch
"""
import subprocess
import sys
import os

def run(cmd, check=True):
    result = subprocess.run(cmd, shell=True, capture_output=True, text=True, encoding='utf-8')
    if check and result.returncode != 0:
        print(f"Error: {result.stderr}")
    return result.stdout.strip(), result.returncode

print("=" * 70)
print("Rebuild Clean Git History")
print("=" * 70)

# Load unique commits
if not os.path.exists('commits_to_keep.txt'):
    print("\nError: commits_to_keep.txt not found!")
    print("Run 'python remove_duplicates.py' first")
    sys.exit(1)

with open('commits_to_keep.txt', 'r') as f:
    unique_commits = [line.strip() for line in f if line.strip()]

print(f"\nFound {len(unique_commits)} unique commits to preserve")

print("\n[1/5] Getting commit details...")

commits_data = []
for i, commit_hash in enumerate(unique_commits):
    if i % 50 == 0:
        print(f"   Progress: {i}/{len(unique_commits)}", end='\r')

    # Get commit info
    cmd = f'git show -s --format="%an|%ae|%ad|%s|%b" {commit_hash}'
    info, _ = run(cmd)

    parts = info.split('|', 4)
    if len(parts) >= 4:
        commits_data.append({
            'hash': commit_hash,
            'author': parts[0],
            'email': parts[1],
            'date': parts[2],
            'subject': parts[3],
            'body': parts[4] if len(parts) > 4 else ''
        })

print(f"\n   Loaded {len(commits_data)} commit details")

print("\n[2/5] Checking out files at each unique commit...")
print("   Creating new clean branch...")

# Go back to master
run('git checkout master', check=False)

# Delete old clean-history if exists
run('git branch -D clean-history', check=False)

# Create fresh orphan branch
run('git checkout --orphan clean-history-new')

# Clear staging
run('git rm -rf .', check=False)

print("\n[3/5] Replaying unique commits...")
print("   This creates a new history with only unique changes\n")

applied = 0
failed = 0

for i, commit in enumerate(commits_data):
    pct = ((i+1) / len(commits_data)) * 100
    print(f"   [{i+1}/{len(commits_data)}] ({pct:.1f}%) - {commit['subject'][:50]}...", end='\r')

    # Checkout files from this commit in master
    _, code = run(f'git checkout {commit["hash"]} -- .', check=False)

    if code != 0:
        failed += 1
        continue

    # Stage changes
    run('git add -A', check=False)

    # Check if there are changes
    status, _ = run('git status --porcelain')

    if not status.strip() and i > 0:
        # No changes, skip
        continue

    # Create commit with original metadata
    author_info = f'{commit["author"]} <{commit["email"]}>'
    commit_msg = commit['subject']
    if commit['body']:
        commit_msg += f"\n\n{commit['body']}"

    # Write commit message to temp file
    with open('temp_commit_msg.txt', 'w', encoding='utf-8') as f:
        f.write(commit_msg)

    # Commit with original author and date
    cmd = f'git commit --author="{author_info}" --date="{commit["date"]}" -F temp_commit_msg.txt'
    _, code = run(cmd, check=False)

    if code == 0:
        applied += 1

# Cleanup
if os.path.exists('temp_commit_msg.txt'):
    os.remove('temp_commit_msg.txt')

print(f"\n\n[4/5] Summary:")
print(f"   Applied: {applied} commits")
print(f"   Failed/Skipped: {failed}")

print("\n[5/5] Finalizing...")

# Rename branch
run('git checkout master', check=False)
run('git branch -D clean-history', check=False)
run('git branch -m clean-history-new clean-history', check=False)
run('git checkout clean-history', check=False)

# Get final count
final_count, _ = run('git rev-list --count HEAD')

print("\n" + "=" * 70)
print("SUCCESS!")
print("=" * 70)

print(f"\nNew clean-history branch created!")
print(f"  Original commits: 595")
print(f"  Target unique: {len(unique_commits)}")
print(f"  Actually applied: {applied}")
print(f"  Final count: {final_count}")

print(f"\nNext steps:")
print(f"  1. Review: git log --oneline | head -30")
print(f"  2. Compare: git diff master")
print(f"  3. Push: git push origin clean-history --force")

print("\n" + "=" * 70)
