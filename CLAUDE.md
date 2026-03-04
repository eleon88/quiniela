# CLAUDE.md

I am building the app described in @SPEC.MD. Read that file for general architectural tasks or to double-check the exact database structure or application architecture.

Keep your replies extremely concise and focus on coveying the key information. No unnecessary fluff, no long code snippets.

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Quiniela SaaS platform — a prediction pool web app where users create boards, define rounds with matches, and participants submit predictions. See `src/SPEC.MD` for the full domain model and feature specification.

**Tech stack**: Angular 21, Angular Material 3, Bootstrap 5, Auth0, SCSS. Backend (separate repo): .NET 8, Dapper, MS SQL Server, SignalR.

## Common Commands

```bash
# Serve locally
ng serve --configuration=local        # http://localhost:4200, API at localhost:3000

# Build
ng build                              # production build
ng build --configuration=development  # dev build

# Run all tests
ng test                               # Karma + Jasmine, launches Chrome

# Lint (not configured yet — no ng lint schematic installed)
```

Build configurations: `local`, `development`, `uat`, `production` — each swaps `src/environments/environment.ts` via `fileReplacements` in `angular.json`.

## Architecture

```
src/app/
  app.config.ts        — Zoneless change detection, router config
  app.routes.ts        — Top-level lazy-loaded routes → /quiniela, /users, /games
  core/
    services/          — Singleton services (auth, session-state, error-message, snack-bar)
    providers/         — Auth guard, HTTP interceptor (stubs)
    ui/login/          — Login component
    layout/            — (placeholder for shell/nav)
  features/
    quiniela/          — Board/round/prediction feature (default route)
    games/             — Match/game management
    users/             — User profiles
    (each has: routes.ts, index.ts, models/, services/, ui/)
  shared/
    components/        — error-message, loading, page-not-found
    models/            — Cross-feature DTOs
    pipes/             — Custom pipes
```

All components are **standalone**. Feature modules are **lazy-loaded** via `loadChildren` in `app.routes.ts`. Barrel exports (`index.ts`) exist at `core/` and `shared/` levels.

## Key Conventions

- **Angular 21 standalone** — no NgModules; use `imports` array on components directly
- **Zoneless** — `provideZonelessChangeDetection()` is enabled; use signals for reactivity, avoid `zone.js` patterns
- **SCSS** — component styles use `.scss`; global theme in `src/material-theme.scss` (Material 3 with azure/blue palettes)
- **Strict TypeScript** — `strict: true`, `noImplicitReturns`, `noImplicitOverride`, `noFallthroughCasesInSwitch`, strict Angular templates
- **EditorConfig** — 2-space indent, single quotes in TypeScript, final newline
- **Services** — `providedIn: 'root'` singletons; core services re-exported from `core/index.ts`

## Environment Configuration

Environment files in `src/environments/` export `{ production: boolean, apiUrl: string }`. File replacement is handled by Angular CLI build configs — import from `../environments/environment` (the base path) and the build swaps in the correct file.

## Styles

### Palette
- **Background** - #121212
- **Primary Text** - #E0E0E0
- **Secondary Text** - #B0B0B0
- **Borders/Dividers** - #444444
- **Accent** - #888888
- **Hover Effects** - #3E59ED