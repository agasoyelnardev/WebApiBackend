# 🔄 CineVerse Front-End & Back-End Integration Setup

## 📊 Projekt Architektur

```
┌─────────────────────────────────────────────────────────────┐
│                    CineverseFrontend                         │
│                 (React 19 + TypeScript)                      │
│                                                              │
│  ┌──────────────────────────────────────────────────────┐   │
│  │  React Components                                    │   │
│  │  - Pages, Screens, UI Components                    │   │
│  └──────────────────────────────────────────────────────┘   │
│                           ↓                                  │
│  ┌──────────────────────────────────────────────────────┐   │
│  │  Custom Hooks (TanStack React Query)                │   │
│  │  - useMovies, useBooks, useUsers, etc.             │   │
│  └──────────────────────────────────────────────────────┘   │
│                           ↓                                  │
│  ┌──────────────────────────────────────────────────────┐   │
│  │  src/api.ts (API Client Layer)                       │   │
│  │  - 80+ API Functions                                │   │
│  │  - JWT Token Management                            │   │
│  │  - Error Handling                                  │   │
│  └──────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
                           ↕ HTTP/JSON
                      (localhost:5000/api)
┌─────────────────────────────────────────────────────────────┐
│                    WebApiBackend                             │
│              (ASP.NET Core + C# + MediatR)                   │
│                                                              │
│  ┌──────────────────────────────────────────────────────┐   │
│  │  Controllers (API Endpoints)                         │   │
│  │  - MoviesController, BooksController, etc.         │   │
│  └──────────────────────────────────────────────────────┘   │
│                           ↓                                  │
│  ┌──────────────────────────────────────────────────────┐   │
│  │  Application Layer (MediatR Handlers)                │   │
│  │  - Commands, Queries, Handlers                      │   │
│  └──────────────────────────────────────────────────────┘   │
│                           ↓                                  │
│  ┌──────────────────────────────────────────────────────┐   │
│  │  Domain Layer                                        │   │
│  │  - Entities, Value Objects, Business Logic         │   │
│  └──────────────────────────────────────────────────────┘   │
│                           ↓                                  │
│  ┌──────────────────────────────────────────────────────┐   │
│  │  Infrastructure & Persistence (Entity Framework)     │   │
│  │  - Database Context, Repositories                  │   │
│  └──────────────────────────────────────────────────────┘   │
│                           ↓                                  │
│  ┌──────────────────────────────────────────────────────┐   │
│  │  SQL Server / PostgreSQL                             │   │
│  └──────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
```

---

## 🚀 Quick Start

### Schritt 1: Back-end starten
```bash
cd WebApiBackend
dotnet restore
dotnet build
dotnet run --project presentation/WebApi.API/WebApi.API.csproj

# Output: 
# info: Microsoft.Hosting.Lifetime[14]
#       Now listening on: http://localhost:5000
#       Now listening on: https://localhost:5001
```

### Schritt 2: Front-end starten
```bash
cd CineverseFrontend

# .env erstellen (wenn nicht vorhanden)
cp .env.example .env

bun install
bun run dev

# Output:
# ➜  Local:   http://localhost:3000
# ➜  press h to show help
```

### Schritt 3: Integration testen
```bash
# Browser öffnen: http://localhost:3000

# Browser DevTools öffnen (F12)
# → Network Tab
# → API Requests zu http://localhost:5000/api sollten sichtbar sein
# → Console: localStorage überprüfen
```

---

## 🔐 Authentication Flow

### 1️⃣ Login
```
User → Frontend (Login Form)
       ↓
Frontend → apiLogin() → Backend (/auth/login)
       ↓
Backend → JWT Token + Refresh Token
       ↓
Frontend → localStorage.setItem('cineverse_token', token)
       ↓
Redirect to Dashboard
```

### 2️⃣ Authenticated Requests
```
Frontend Component
  ↓
useMovies Hook (React Query)
  ↓
apiGetMovies()
  ↓
request() Helper
  ↓
// Automatisch Token injizieren
const token = getAuthToken()  // aus localStorage
headers['Authorization'] = `Bearer ${token}`
  ↓
HTTP GET http://localhost:5000/api/movies
  ↓
Backend (Protected Route)
  ↓
JWT Middleware validiert Token
  ↓
Response (Array von Movies)
```

### 3️⃣ Token Refresh (bei Ablauf)
```
API Request → 401 Unauthorized
  ↓
Catch 401 → apiRefreshToken()
  ↓
Backend → Neue Tokens
  ↓
localStorage aktualisieren
  ↓
Retry original Request
```

---

## 📋 Environment Konfiguration

### Frontend `.env`
```env
# API Server
VITE_API_BASE_URL=http://localhost:5000/api

# Google Gemini
GEMINI_API_KEY=your_key

# App Info
APP_URL=http://localhost:3000
```

### Backend `appsettings.json`
```json
{
  "Jwt": {
    "SecretKey": "your-super-secret-key-min-32-chars",
    "Issuer": "CineversApi",
    "Audience": "CineversClient",
    "ExpirationMinutes": 15,
    "RefreshTokenExpirationDays": 7
  },
  "Cors": {
    "AllowedOrigins": [
      "http://localhost:3000",
      "http://localhost:5173",
      "https://yourdomain.com"
    ]
  },
  "Database": {
    "ConnectionString": "Server=localhost;Database=CineVerse;User Id=sa;Password=..."
  }
}
```

---

## 📡 API Request/Response Examples

### Example 1: Login
```typescript
// Frontend Request
const response = await apiLogin('user@example.com', 'password123');

// Generates:
POST http://localhost:5000/api/auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "password123"
}

// Backend Response (200 OK)
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "abcdef123456...",
  "user": {
    "id": "user-id-123",
    "username": "user",
    "email": "user@example.com"
  }
}
```

### Example 2: Get Movies (mit Authentication)
```typescript
// Frontend Request
const movies = await apiGetMovies({ pageNumber: 1, pageSize: 20 });

// Generates:
GET http://localhost:5000/api/movies?PageNumber=1&PageSize=20
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

// Backend Response (200 OK)
[
  {
    "id": "movie-1",
    "title": "Dune",
    "description": "...",
    "poster": "https://...",
    "rating": 8.5,
    "year": 2021,
    "genres": ["Sci-Fi", "Adventure"],
    "director": "Denis Villeneuve",
    "cast": ["Timothée Chalamet", "..."]
  },
  { ... }
]
```

### Example 3: Create Movie (POST mit FormData)
```typescript
// Frontend Request
const formData = new FormData();
formData.append('title', 'New Movie');
formData.append('description', '...');
formData.append('poster', posterFile);
formData.append('genres', 'Action');

const result = await apiCreateMovie(formData);

// Generates:
POST http://localhost:5000/api/movies
Authorization: Bearer ...
Content-Type: multipart/form-data

[FormData with files and fields]

// Backend Response (201 Created)
{
  "id": "new-movie-id",
  "title": "New Movie",
  "message": "Movie created successfully"
}
```

---

## 🎣 React Query Integration Pattern

### Custom Hook Template
```typescript
// src/hooks/useCustomResource.ts
import { useQuery, useMutation, UseQueryOptions, UseMutationOptions } from '@tanstack/react-query';
import { apiGetResource, apiCreateResource, apiUpdateResource, apiDeleteResource } from '../api';

// Query Hook
export function useCustomResource(id?: string, options?: UseQueryOptions<any>) {
  return useQuery({
    queryKey: ['resource', id],
    queryFn: () => id ? apiGetResource(id) : Promise.resolve(null),
    enabled: !!id,
    staleTime: 5 * 60 * 1000,      // 5 minutes
    gcTime: 10 * 60 * 1000,        // 10 minutes (formerly cacheTime)
    ...options,
  });
}

// Create Mutation Hook
export function useCreateResource(options?: UseMutationOptions<any, Error, any>) {
  return useMutation({
    mutationFn: (data) => apiCreateResource(data),
    ...options,
  });
}

// Update Mutation Hook
export function useUpdateResource(id: string, options?: UseMutationOptions<any, Error, any>) {
  return useMutation({
    mutationFn: (data) => apiUpdateResource(id, data),
    ...options,
  });
}

// Delete Mutation Hook
export function useDeleteResource(options?: UseMutationOptions<any, Error, string>) {
  return useMutation({
    mutationFn: (id) => apiDeleteResource(id),
    ...options,
  });
}
```

### Component Usage
```typescript
import { useCustomResource, useCreateResource, useUpdateResource } from '../hooks/useCustomResource';
import { useQueryClient } from '@tanstack/react-query';

export function MyComponent() {
  const queryClient = useQueryClient();
  
  // Queries
  const { data, isLoading, error } = useCustomResource('resource-id');
  
  // Mutations
  const createMutation = useCreateResource({
    onSuccess: () => {
      // Invalidate & Refetch
      queryClient.invalidateQueries({ queryKey: ['resource'] });
    },
    onError: (error) => {
      console.error('Create failed:', error.message);
    },
  });

  const updateMutation = useUpdateResource('resource-id', {
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['resource'] });
    },
  });

  if (isLoading) return <div>Loading...</div>;
  if (error) return <div>Error: {error.message}</div>;

  return (
    <div>
      <h1>{data?.title}</h1>
      
      <button 
        onClick={() => createMutation.mutate({ title: 'New' })}
        disabled={createMutation.isPending}
      >
        {createMutation.isPending ? 'Creating...' : 'Create'}
      </button>

      <button 
        onClick={() => updateMutation.mutate({ title: 'Updated' })}
        disabled={updateMutation.isPending}
      >
        {updateMutation.isPending ? 'Updating...' : 'Update'}
      </button>
    </div>
  );
}
```

---

## 🔒 Security Best Practices

### ✅ Implemented
- JWT Token-based Authentication
- Token Storage in localStorage
- Automatic Token Refresh
- CORS Policy (Backend)
- Bearer Token Injection

### ⚠️ Improvements für Production
```typescript
// 1. HTTPOnly Cookies statt localStorage
// (Backend setzt Secure + HttpOnly Flags)

// 2. Token Rotation
// Refresh Token sollte nach Verwendung aktualisiert werden

// 3. HTTPS only in Production
// Environment-spezifische Konfiguration

// 4. Secrets Management
// Environment Variablen für sensible Daten
// Niemals in Versionskontrolle committen
```

---

## 🐛 Troubleshooting

| Problem | Ursache | Lösung |
|---------|--------|--------|
| **CORS Error** | Backend CORS nicht konfiguriert | `app.UseCors()` in Program.cs vor Routing |
| **401 Unauthorized** | Token abgelaufen oder nicht gespeichert | Token Refresh implementieren, localStorage überprüfen |
| **404 Not Found** | API Endpoint falsch oder Backend nicht laufen | Endpoint URL überprüfen, `dotnet run` ausführen |
| **500 Server Error** | Backend Exception | Backend Logs überprüfen, Database Connection |
| **Network Error** | Frontend kann Backend nicht erreichen | Backend läuft? Port korrekt? Firewall? |
| **Token nicht injiziert** | `getAuthToken()` gibt null zurück | Login durchführen, localStorage.cineverse_token überprüfen |
| **Bun install fails** | Lock File Konflikt | `rm bun.lock && bun install` |

---

## 📊 Performance Optimization

### Frontend
```typescript
// 1. React Query Stale Time Nutzen
const { data } = useQuery({
  queryKey: ['movies'],
  queryFn: fetchMovies,
  staleTime: 5 * 60 * 1000,  // 5 min
  gcTime: 10 * 60 * 1000,    // 10 min
});

// 2. Lazy Loading Images
<img loading="lazy" src={movie.poster} />

// 3. Code Splitting
const LazyComponent = React.lazy(() => import('./Component'));

// 4. Memoization
const MemoComponent = React.memo(MyComponent);
```

### Backend
```csharp
// 1. Pagination
pageNumber, pageSize in Query Parameters

// 2. Caching (Redis)
[Cached(Duration = 300)]  // 5 minutes
public async Task<IActionResult> GetMovies() { ... }

// 3. Database Indexing
CREATE INDEX idx_movie_title ON Movies(Title);

// 4. Async/Await
public async Task<IActionResult> GetMovies() { ... }
```

---

## 📚 Dokumentation Links

- **Frontend Guide:** `CineverseFrontend/API_INTEGRATION.md`
- **Backend Guide:** `WebApiBackend/INTEGRATION_SETUP.md`
- **API Endpoints:** `src/api.ts` (Frontend)
- **TypeScript Types:** `src/types.ts` (Frontend)

---

## ✅ Integration Checklist

- [ ] Backend läuft auf `http://localhost:5000`
- [ ] Frontend läuft auf `http://localhost:3000`
- [ ] `.env` Datei im Frontend erstellt
- [ ] `VITE_API_BASE_URL` konfiguriert
- [ ] CORS im Backend eingerichtet
- [ ] Login-Flow funktioniert
- [ ] Token wird in localStorage gespeichert
- [ ] API Requests erscheinen in Browser DevTools
- [ ] Error Handling funktioniert
- [ ] TypeScript Types sind aktuell

---

## 🎯 Nächste Schritte

1. **Backend APIs erweitern:** Neue Endpoints in Controllers hinzufügen
2. **Frontend Components:** UI Components mit API-Hooks verbinden
3. **Error Handling:** Globales Error Handling implementieren
4. **Loading States:** Skeleton/Spinner Components
5. **Form Validation:** Yup oder Zod für Validierung
6. **Real-time Updates:** WebSocket für Live-Daten (optional)
7. **Testing:** Unit Tests für Hooks und Components
8. **Deployment:** Docker, GitHub Actions, Cloud Provider

---

**Status:** ✅ Integration Ready  
**Dokumentation aktualisiert:** August 2026  
**Maintainer:** agasoyelnardev
