# Angular UI – Design & Improvement Guide

*Applies to:* `Presentation/UI` (Angular 17 SPA)
*Last updated:* 2025-07-01

---

## 1  Purpose & Position in Clean Architecture
The UI is the **outermost layer**.  Its only job is to deliver a rich user
experience and translate browser events → HTTP requests → DTOs.

**Must NOT contain business rules.**  Those live in Application/Domain.

---

## 2  Dependency Rules
| Area | May import | Must **NOT** import |
|------|------------|--------------------|
| UI code | Angular libs, RxJS, third-party UI libs (e.g. Material) | .NET assemblies, EF models |
| Core services | `HttpClient`, `environment` | DOM-manipulation libs (jQuery), component classes |
| Components | Core services, store selectors | HttpClient directly, business logic |

> **Tip:** Any code that would still compile in a Node test-runner belongs to
> the UI; anything that needs a database does not.

---

## 3  Folder & Module Structure (desired)
```
src/
├─ app/
│  ├─ core/               # singleton providers (auth, interceptors, guards)
│  ├─ shared-ui/          # dumb/presentational components, pipes, directives
│  ├─ features/
│  │   ├─ admin/          # lazy module
│  │   ├─ blasting-eng/   # lazy module
│  │   └─ …
│  └─ app-routing.module.ts (lazy routes)
└─ environments/
```

### Design Rules
1. **Feature modules** are **lazy-loaded** (`loadChildren`) to keep initial
   bundle small.
2. `core` is imported **once** (AppModule).
3. `shared-ui` only contains stateless, reusable UI pieces – no services.

---

## 4  Design Principles & Patterns
### 4.1 Services (in `core/services`)
• Wrap every backend endpoint; expose `Observable<T>`s.  
• Map backend DTO → UI Model in **adapter functions**.  
• Composition-root: `providedIn:'root'`.

### 4.2 Components
• **Container vs Presentational** – containers fetch data; presentational
receive `@Input()` + emit `@Output()`.

### 4.3 State Management
• Keep state local until used by >1 feature.  
• If global, introduce **NgRx** or **Angular signals-store**.

### 4.4 Cross-cutting Concerns
| Concern | Guideline |
|---------|-----------|
| Error handling | Global `ErrorHandler` or `HttpInterceptor` → toast/problem-dialog |
| Logging | Use Angular `ErrorHandler` + Sentry/AppInsights plug-in |
| Security | `AuthInterceptor` attaches JWT; guard validates expiry via `AuthService.validateToken()` |
| Accessibility | Run `@angular-eslint/template-accessibility` & Lighthouse audits |
| Performance | Use lazy modules, `*ngOptimizedImage`, on-push CD strategy |

---

## 5  Current Assessment (2025-07-01)
### Strengths ✅
1. Services isolated in `core/services`; components don't call `HttpClient`.
2. Guards, interceptors, environment config already set up.
3. Directory names scream their feature domain (`admin/`, `machine-manager/`).

### Gaps 🔍
| Finding | Impact | Fix |
|---------|--------|-----|
| No global HTTP error interceptor | Inconsistent UX | Add `CoreErrorInterceptor` with toast / redirect rules |
| Token guard only checks presence, not expiry | Potential silent 401s | Decode JWT or call `/auth/validate-token` before route activation |
| Large eager-loaded role modules | Bundle size, FCP | Convert to lazy modules |
| Services (e.g. `MachineService`) include mock data & >150 LOC | Maintenance | Extract mock provider + adapter utils |
| Manual query-string build | Bug-prone | Use `HttpParams` API |
| Minimal unit/E2E tests | Regression risk | Write Jest specs & Cypress flows |
| No shared-ui library | Duplication | Create `shared-ui` with buttons, cards, tables |

---

## 6  Improvement Back-log
| 📝 Item | Status | Priority |
|---------|--------|----------|
| Implement `CoreErrorInterceptor` + global `ErrorHandler` | Not started | High |
| Lazy-load `admin`, `blasting-engineer`, `machine-manager` modules | In progress | High |
| JWT expiry validation in `authGuard` | Not started | High |
| Extract mapping utilities (`mapMachine`, CSV adapters) | Not started | Medium |
| Introduce `shared-ui` module | Backlog | Medium |
| Add Cypress E2E for login & project CRUD | Not started | Medium |
| Replace query-string concat with `HttpParams` | Backlog | Low |
| Dark-mode theming with CSS variables | Backlog | Low |

---

## 7  Checklist for New UI Feature PR
- [ ] Create **lazy** feature module & routing.  
- [ ] Container component calls `core/services`; presentational components are
  stateless.  
- [ ] All API calls handled through service; no magic strings.  
- [ ] Error scenario covered via global interceptor or local `catchError`.  
- [ ] SCSS follows BEM & uses shared variables.  
- [ ] Unit tests added (`HttpTestingController`, component harness).  
- [ ] E2E happy path updated.

---

### Authors & Maintenance
Maintained by the Front-end Guild.  Update when architecture or tooling
changes. 