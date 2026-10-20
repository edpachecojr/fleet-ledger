# CLAUDE.md — FleetLedger

Fleet management MVP for trucking companies: vehicles, drivers, trips, payables/receivables, maintenance and per-trip/per-vehicle P&L (DRE).

## Repository

```
docs/
├── backlog/          tasks (TASK-NNN), one file each
├── adr/              architecture decisions — every infra choice lives here
├── PROGRESS.md       live implementation state
└── ROADMAP.md        milestones
fleetledger-api/                .NET 10 back-end — rules in fleetledger-api/CLAUDE.md
fleetledger-backoffice-web/     React back-office (Vite SPA) — rules in fleetledger-backoffice-web/CLAUDE.md
docker-compose.yml              local stack (PostgreSQL + API + back-office)
```

The API is UI-agnostic: no front-end lives in `fleetledger-api`. Each client has its own folder and its own `CLAUDE.md`, and talks to the API only through the OpenAPI contract (types generated from `/openapi/v1.json`, never hand-written).

## Workflow

- Before starting a task, read `docs/PROGRESS.md`, `docs/ROADMAP.md` and the task file in `docs/backlog/`.
- When a task starts, finishes or gets blocked, update `docs/PROGRESS.md` (status, dates, decisions, diary).
- Infrastructure choices go to a new ADR in `docs/adr/`, with the reason and the alternatives.
- If something is undefined or ambiguous, ask before coding on top of an assumption.

## Code style

- Functions: 4-20 lines. Split if longer.
- Files: under 200 lines ideally, 300 max. Split by responsibility.
- One thing per function, one responsibility per module (SRP).
- Names: specific and unique. Avoid `data`, `handler`, `Manager`.
  Prefer names that return <5 grep hits in the codebase.
- Types: explicit. No untyped or loosely typed signatures.
- No code duplication. Extract shared logic into a function/module.
- Early returns over nested ifs. Max 2 levels of indentation.
- One level of abstraction per function.
- Exception messages must include the offending value and expected shape.
- Domain names (entities, value objects, properties) in English.

## Comments

- Keep your own comments. Don't strip them on refactor — they carry
  intent and provenance.
- Write WHY, not WHAT. Skip `// increment counter` above `i++`.
- Docstrings on public functions: intent + one usage example.
- Reference issue numbers / commit SHAs when a line exists because
  of a specific bug or upstream constraint.

## Tests

- Tests run with a single command per project: `dotnet test` in `fleetledger-api/`, `pnpm test` in `fleetledger-backoffice-web/`.
- Every new function gets a test. Bug fixes get a regression test.
- Mock external I/O (API, DB, filesystem) with named fake classes,
  not inline stubs.
- Tests must be F.I.R.S.T: fast, independent, repeatable,
  self-validating, timely.

## Dependencies

- Inject dependencies through constructor/parameter, not global/static.
- Wrap third-party libs behind a thin interface owned by this project.

## Structure

- Follow the framework's convention.
- Prefer small focused modules over god files.
- Predictable paths.

## Formatting

- Use `dotnet format` (API) and `pnpm format` / Prettier (web). Don't discuss style beyond that.

## Logging

- Structured JSON when logging for debugging / observability.
- Plain text only for user-facing CLI output.

## Commits

Portuguese, imperative, with prefix (`feat:`, `fix:`, `refactor:`, `test:`, `docs:`, `chore:`, `style:`). One subject per commit; file moves never mixed with content changes.
