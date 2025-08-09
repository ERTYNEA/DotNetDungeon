---
applyTo: '**.cs'
---

While making a code modification, do not add any new comments.
Ignore any special marker tags (TODO, FIX, etc.), as these are usually addressed later. This will be the standard approach unless stated otherwise. If an exception is specified, only address the task indicated in the given tag and then remove that tag after completing the task.

After completing a code modification:

1 - Review all code files in all projects of the solution and perform a refactoring with the following criteria:
a) If the name of any variable or method has been changed, review all variables, methods, and log outputs to ensure the logic remains consistent.
b) Whenever possible, avoid using braces in if, for, etc., when the scope affects only a single line.

2 - Review all comments in all code files of all projects in the solution and update them if necessary. All comments must be written in English and fit on a single line (except for method descriptions).

3 - Review the formatting of all code files of all projects in the solution and apply the standard formatting rules. Keep in mind that indentation uses a tab equivalent to four spaces.

After the above tasks:

1 - Run the tests (check if they are available to be executed).
2 - Build the project to confirm everything compiles correctly.