# Front-end & Back-end Integration Guide

## Übersicht

Dieses Projekt integriert ein **React + TypeScript Front-end** (CineverseFrontend) mit einem **ASP.NET Core Back-end** (WebApiBackend).

---

## Back-end Setup (ASP.NET Core)

### 1. CORS Konfiguration in `Program.cs`

Öffnen Sie `presentation/WebApi.API/Program.cs` und fügen Sie folgende CORS-Konfiguration hinzu:

```csharp
// CORS hinzufügen - NACH services.AddControllers() UND VOR app.UseRouting()
var corsPolicy = "AllowFrontend";

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy, builder =>
    {
        var allowedOrigins = new[]
        {
            "http://localhost:3000",           // Development
            "http://localhost:5173",           // Vite alternative port
            "http://127.0.0.1:3000",
            "http://127.0.0.1:5173",
            "https://yourdomain.com",          // Production (update as needed)
        };

        builder
            .WithOrigins(allowedOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .WithExposedHeaders("X-Total-Count", "X-Page-Number", "X-Page-Size");
    });
});

// ... später in der Middleware Pipeline:
app.UseCors(corsPolicy);
```

### 2. JWT Authentication Header Support

Stellen Sie sicher, dass Ihre `Program.cs` Authentifizierung korrekt konfiguriert ist:

```csharp
// Authentication Services
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])),
            ValidateIssuer = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

app.UseAuthentication();
app.UseAuthorization();
```

### 3. Content-Type & JSON Konfiguration

```csharp
builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});
```

---

## Front-end Setup (React + TypeScript)

### 1. Environment Configuration

Erstellen Sie eine `.env` Datei im Root-Verzeichnis (`.env.example` existiert bereits):

```env
# API Configuration
VITE_API_BASE_URL=http://localhost:5000/api

# Optional: For different environments
# Development
# VITE_API_BASE_URL=http://localhost:5000/api

# Production
# VITE_API_BASE_URL=https://api.yourdomain.com/api
```

### 2. API Client Setup

Die Datei `src/api.ts` ist bereits konfiguriert und unterstützt:
- ✅ JWT Bearer Token Authentication
- ✅ Automatic Token Injection
- ✅ Error Handling
- ✅ FormData für File Uploads
- ✅ 80+ API Endpoints

**Token-Speicherung:**
```typescript
// Tokens werden in localStorage gespeichert:
localStorage.getItem('cineverse_token')        // Access Token
localStorage.getItem('cineverse_refresh_token') // Refresh Token
```

### 3. React Query Configuration

Erstellen Sie `src/hooks/useQueryClient.ts`:

```typescript
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { ReactNode } from 'react';

export const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: 1000 * 60 * 5, // 5 minutes
      gcTime: 1000 * 60 * 10,    // 10 minutes
      retry: 1,
    },
    mutations: {
      retry: 1,
    },
  },
});

export function QueryProvider({ children }: { children: ReactNode }) {
  return (
    <QueryClientProvider client={queryClient}>
      {children}
    </QueryClientProvider>
  );
}
```

### 4. Custom Hook für API Calls

Erstellen Sie `src/hooks/useApi.ts`:

```typescript
import { useQuery, useMutation, UseQueryOptions, UseMutationOptions } from '@tanstack/react-query';

export function useApiQuery<T>(
  key: string[],
  fn: () => Promise<T>,
  options?: UseQueryOptions<T>
) {
  return useQuery<T>({
    queryKey: key,
    queryFn: fn,
    ...options,
  });
}

export function useApiMutation<TData, TVariables>(
  fn: (variables: TVariables) => Promise<TData>,
  options?: UseMutationOptions<TData, Error, TVariables>
) {
  return useMutation<TData, Error, TVariables>({
    mutationFn: fn,
    ...options,
  });
}
```

---

## Entwicklungs-Workflow

### Back-end starten:
```bash
cd WebApiBackend
dotnet restore
dotnet build
dotnet run --project presentation/WebApi.API/WebApi.API.csproj
# Server läuft unter: http://localhost:5000
```

### Front-end starten:
```bash
cd CineverseFrontend
bun install
bun run dev
# App läuft unter: http://localhost:3000
```

### API-Tests durchführen:
```bash
# Mit curl:
curl -X GET http://localhost:5000/api/stats \
  -H "Content-Type: application/json"

# Mit Authentifizierung:
curl -X GET http://localhost:5000/api/auth/me \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json"
```

---

## Endpoint-Übersicht

Alle verfügbaren Endpoints sind in `src/api.ts` dokumentiert:

| Bereich | Endpoints |
|---------|-----------|
| **Auth** | Login, Register, Refresh, Logout |
| **Users** | Profile, User Management, Roles |
| **Movies** | CRUD, Search (TMDB), Collections |
| **Books** | CRUD, Search (Google Books), PDF Upload |
| **Reviews** | Movie & Book Reviews, Ratings |
| **Social** | Follow, Friend Requests, Activity |
| **Chat** | Rooms, Messages, Live Streams |
| **Admin** | Stats, Users, Activity Logs |

---

## Troubleshooting

### CORS Fehler
- Stellen Sie sicher, dass `app.UseCors()` vor `app.UseRouting()` aufgerufen wird
- Überprüfen Sie die `VITE_API_BASE_URL` Konfiguration
- Browser Developer Tools → Network Tab → Check Request Headers

### 401 Unauthorized
- Token ist abgelaufen → Refresh Token verwenden
- Token wird nicht mitgesendet → localStorage überprüfen
- Bearer Format korrekt? → `Authorization: Bearer <token>`

### 500 Server Error
- Back-end Logs überprüfen: `dotnet run` output
- Database Connection überprüfen
- Entity Framework Migrations ausgeführt? → `dotnet ef database update`

---

## Sicherheit

✅ **HTTPS in Production** - Verwenden Sie HTTPS URLs in `.env.production`
✅ **Token Expiration** - Access Token: 15 min, Refresh Token: 7 days (anpassen nach Bedarf)
✅ **Secure Cookie Flags** - Cookies mit `HttpOnly` & `Secure` flags
✅ **Environment Secrets** - Nie API Keys in `.env` commiten → `.gitignore` verwenden

---

## Nächste Schritte

1. ✅ CORS in back-end konfigurieren
2. ✅ `.env` im front-end erstellen
3. ✅ Beide Server starten
4. ✅ Login-Endpoint testen
5. ✅ UI-Components mit API verbinden

---

**Dokumentation aktualisiert:** August 2026
