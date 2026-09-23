---
name: gdd
description: Record a game design decision in PLAN.md and retire the question it answers from QUESTIONS.md. Use when the user settles a design question — mechanics, scope, art direction, progression, bee/hive behaviour — so the docs stay the source of truth.
---

# Recording a design decision

Two documents, with distinct jobs:

- **`PLAN.md`** — what has been *decided*. Under "Decisions made so far", each entry
  is a bolded claim plus the reasoning and the tradeoff accepted.
- **`QUESTIONS.md`** — a numbered menu of what is still *open*, grouped by area
  (Bees & Hive Biology, Hive Management, etc.). Explicitly a menu, not a
  questionnaire — unanswered is a valid resting state.

## When the user settles something

1. **Write it into `PLAN.md`** in the existing voice: bold the decision, then give
   the *why* and the cost being accepted. The doc's own framing — "this is a
   thinking-out-loud doc, not a spec" — means capturing reasoning matters more than
   terse conclusions. Copy the shape of the existing entries.
2. **Remove the corresponding question from `QUESTIONS.md`** and renumber that
   section. A question answered in `PLAN.md` but still listed as open makes the
   menu untrustworthy. If the answer only partly resolves it, narrow the question
   to the part still open rather than deleting it.
3. **Remove it from the answer sheet too.** `plans/project-status.md` section 7
   gathers every open question with a slot for the user's answer; delete the
   answered entry there (or narrow it, as above).
4. **Record what the decision implies but does not settle.** Decisions usually
   spawn questions — adding cross-breeding raises genetics-storage and UI
   questions. Add those to the relevant `QUESTIONS.md` section.

## Rules

- **Do not invent decisions.** If the user muses about an option without choosing,
  it isn't decided. Ask, or leave the question open.
- **Do not quietly reverse a prior decision.** If a new answer contradicts one
  already in `PLAN.md`, point at the conflict and let the user resolve it.
- **Convert relative dates to absolute** ("next month" → the actual month).
- **Keep genericized brand names genericized** — the real Sknyliv locations stay,
  the trademarks don't. That decision is already locked in `PLAN.md`.
- Keep the ~78-character wrapping both files use.

## Scope reality check

This is a solo hobby project recreating a ~2 km slice of Lviv with a hybrid
sim/on-foot loop — an enormous amount of work for one person. When a decision
meaningfully enlarges that scope, say so once, plainly, and record the cost in the
`PLAN.md` entry. Then record the decision the user actually made. Flagging is
useful; relitigating is not.
