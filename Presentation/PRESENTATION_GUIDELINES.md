# Presentation Layer – Design & Guidelines

## Purpose
The **Presentation** layer is the _delivery mechanism_ for the application. It
contains two sub-projects:

1. **API**  – ASP.NET Core REST controllers exposing Application services.
2. **UI**   – Angular 17 SPA consuming the API.

Its job is to:
• Translate HTTP / JSON / browser concerns into Application-level requests.
• Handle authentication/authorization middleware.
• Wire up Dependency Injection and configuration.
• Provide a responsive, accessible user experience.

> Rule: **No business rules here.** The Presentation layer _orchestrates_ – it
> does not _decide_.

---

## 1. Dependency Rules
| Sub-project | May reference | Must NOT reference |
|-------------|---------------|--------------------|
| API         | `Application`, `Infrastructure` via DI, `Microsoft.AspNetCore*` | `Domain` or EF-Core types directly (except via DTO) |
| UI          | REST endpoints, Angular libs (`@angular/*`, RxJS) | .NET assemblies |

---

## 2. Folder Structure (snapshot)
```
Presentation/
├─ API/                     # ASP.NET Core
│  ├─ Controllers/
│  ├─ Properties/launchSettings.json
│  ├─ API.csproj
│  └─ Program.cs            # DI & middleware
└─ UI/                      # Angular 17 workspace
   ├─ src/
   │  ├─ app/
   │  │  ├─ core/           # singleton services, models, interceptors
   │  │  └─ components/     # feature modules
   │  ├─ environments/
   │  └─ index.html
   ├─ angular.json
   └─ package.json
```

---

## 3. API – Design Principles
1. **Controller thinness** – controllers map HTTP to Application service calls.
2. **DTO boundary** – Controllers accept/return DTOs; never expose Domain
   entities unless safe.
3. **Status codes** – Use standard HTTP status codes + Problem Details for
   errors (RFC 7807). Add global exception middleware.
4. **Versioning** – Version via URL (`/api/v1/...`) or header; plan ahead.
5. **Authentication** – JWT bearer configured in `Program.cs`; authorize
   endpoints with `[Authorize(Role="Admin")]` etc.
6. **OpenAPI/Swagger** – Keep docs in sync via annotations; enable only in
   non-production by default.
7. **CORS** – Restrict to UI origin list.
8. **Logging & Correlation** – Use `ILogger` and correlate requests via
   `TraceIdentifier`.

### 3.1 Example Controller Snippet
```csharp
[ApiController]
[Route("api/projects")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _svc;

    public ProjectsController(IProjectService svc) => _svc = svc;

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _svc.GetAllProjectsAsync());
}
```

---

## 4. UI – Design Principles
1. **Feature-module architecture** – group by feature not by layer.
2. **Smart / dumb components** – containers fetch data via `core/services`;
   presentational components stay stateless.
3. **RxJS & Observables** – All HTTP calls return `Observable<T>`.
4. **State management** – Use *NgRx* or *signal* store only when state gets
   complex; keep local otherwise.
5. **Styling** – SCSS variables in `styles/shared.scss`; avoid inline styles.
6. **Accessibility** – ARIA labels, keyboard navigation.
7. **i18n** – Prepare translation JSON files early.
8. **Testing** – Write unit tests with Jest / Karma; E2E with Cypress.

### 4.1 Example Service
```ts
@Injectable({ providedIn: 'root' })
export class ProjectService {
  private api = environment.apiUrl + '/projects';
  constructor(private http: HttpClient) {}
  getAll(): Observable<Project[]> {
    return this.http.get<Project[]>(this.api);
  }
}
```

---

## 5. Current Design – Strengths ✅
1. API & UI separated cleanly; controllers only depend on Application
   interfaces.
2. DI registrations centralised in `Program.cs`.
3. Angular project adopts feature folders; core module houses interceptors &
   guards.

## 5.1 Areas to Improve 🔍
| Finding | Impact | Suggested Fix |
|---------|--------|---------------|
| **Global exception handling missing** – uncaught exceptions leak stack-traces. | Security, UX | Add middleware returning Problem Details. |
| **Controllers occasionally return Domain entities** (`DrillPlanController`). | Data exposure risk | Introduce minimal DTO. |
| **CORS list hard-coded** | Deploy flexibility | Read allowed origins from configuration array. |
| **Angular lacks lazy-loading for some admin sub-modules** | Bundle size | Convert to lazy modules. |
| **Missing E2E tests for critical flows** | Regression risk | Add Cypress scenarios: login, project CRUD. |
| **No dark-mode / theming strategy yet** | UX | Adopt CSS variables & material theming. |

---

## 6. Improvement Back-log (2025-07-01)
| 📝 Item | Status | Priority |
|---------|--------|----------|
| Add global exception/ProblemDetails middleware | Not started | High |
| Replace entity exposure with DTO in `DrillPlanController` | Not started | High |
| Make CORS origins configurable | Not started | Medium |
| Enable Angular route lazy-loading for admin and engineer modules | In progress | Medium |
| Write Cypress E2E suite | Not started | Medium |
| Introduce dark-mode toggle & theme service | Backlog | Low |

---

## 7. Checklist before merging Presentation changes
API:
- [ ] Controller < 100 LOC? If larger, refactor.
- [ ] No Domain or Infrastructure types leaked.
- [ ] Proper status codes & Problem Details used.
- [ ] AuthN/AuthZ attributes present.
- [ ] Added/updated Swagger comments.

UI:
- [ ] Component passes a11y lint.
- [ ] Service method returns typed `Observable`.
- [ ] State kept local unless cross-feature use.
- [ ] Unit tests updated.

---

### Authors & Maintenance
Created July 2025 by the DBMS team. Keep this file current as Presentation
patterns evolve. 