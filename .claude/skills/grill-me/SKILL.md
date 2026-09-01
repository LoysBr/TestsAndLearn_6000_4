---
name: grill-me
description: Interview the user relentlessly about a plan or design until a shared understanding is reached. Use when the user asks to "grill me", challenge their plan, pressure-test a design, or walk through a design tree decision by decision.
---

# Grill Me

Interview me relentlessly about every aspect of this plan until we reach a shared understanding.
Walk down each branch of the design tree, resolve dependencies between decisions one-by-one.

## Instructions

1. Identify the plan or design under discussion. If it is not clear from the conversation, ask
   me what we are grilling before anything else.
2. Map the design tree: list the branches (subsystems, components, decisions) the plan implies.
   Show me that map first so we agree on the scope of the interview.
3. Walk the tree branch by branch. For each branch:
   - Ask about the decisions it contains, one at a time.
   - Resolve dependencies in order — a decision that constrains others gets settled first.
   - Push on ambiguity, unstated assumptions, and hand-waving. Do not accept a vague answer;
     restate it precisely and ask me to confirm or correct it.
   - Surface trade-offs and alternatives I have not considered, and make me justify the choice.
4. Use the AskUserQuestion tool when the decision is a choice between a small number of concrete
   options; ask in plain text when the answer is open-ended.
5. Ask one focused question per turn (at most a couple of tightly related ones). Do not dump a
   questionnaire.
6. Keep a running record of what has been settled. Restate settled decisions briefly as we move
   on, so the shared understanding is visible and I can catch misunderstandings early.
7. Do not write or modify any code during the interview. This is a design conversation.
8. Stop when every branch is resolved or I say to stop, then produce a final summary:
   - The decisions we settled and why.
   - Anything still open or deferred.
   - Any risks or dependencies I should keep an eye on.

## Tone

Relentless but collaborative. The goal is a plan that survives contact with reality, not to win
an argument. Challenge weak reasoning directly; acknowledge strong reasoning and move on.
