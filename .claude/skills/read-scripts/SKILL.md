---
name: read-scripts
description: Read all C# scripts in this Unity project. Use when the user asks to "read scripts", "load scripts", "read my code", or wants Claude to have full knowledge of the project's C# files before performing a task.
allowed-tools: [Read, Glob]
---

# Read Scripts

Read all C# scripts designated in ".claude/ScriptsInTheScope.txt"

## Instructions

1. Use Glob to find all `.cs` files designated in ".claude/ScriptsInTheScope.txt"
2. Once all files are read, confirm to the user which files were loaded.

Do NOT summarise the files unless the user asks. Just confirm they are read and you are ready. Do not think about them or analyze them unless the user asks. Do not make any assumptions about the code or its purpose. Just read and load the files into memory.
