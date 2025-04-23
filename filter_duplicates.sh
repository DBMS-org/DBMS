#!/bin/bash
# Script to filter out duplicate commits

echo "Filtering duplicate commits..."

# Read unique commits
mapfile -t UNIQUE_COMMITS < unique_commits_list.txt

# Create associative array for fast lookup
declare -A KEEP_COMMITS
for commit in "${UNIQUE_COMMITS[@]}"; do
    KEEP_COMMITS[$commit]=1
done

echo "Loaded ${#KEEP_COMMITS[@]} commits to keep"

# Use git filter-branch to rewrite history
git filter-branch --commit-filter '
    if grep -q "$GIT_COMMIT" unique_commits_list.txt; then
        git commit-tree "$@"
    else
        skip_commit "$@"
    fi
' --tag-name-filter cat -- --all

echo "Done filtering!"
