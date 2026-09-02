---
name: implement
description: "Implement a piece of work based on a spec or set of tickets."
disable-model-invocation: true
---

Implement the work described by the user in the spec or tickets. If the issue is still `Todo`, switch it to `In Progress`. In case issue was `Closed`, switch it to `Reopened`.

Once done, use /code-review to review the work.

Once implemented and reviewed (by you), switch the issue status to `Needs review` and ask the user to review code. If the user finds any issues, ask him if you should fix them (don't automatically fix) and ask for another review after. 

Once the user is satisfied, you can switch the issue to `Needs test` and ask the user to test it. If the user finds any issues, ask him if you should fix them (don't automatically fix) and ask for another test after. Once user is statisfied, you can switch the issue to `Closed`.