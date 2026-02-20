# ? SINGLETON AUTH SERVICE FIX - Navbar Now Updates!

## The Problem
After logging in, the navbar still showed "Login" button instead of "Logout" and the "Items" dropdown. This happened because each Blazor component was getting a **different instance** of `AuthService`, so they couldn't share authentication state.

## The Root Cause
The `AuthService` was registered as **Scoped**, which means:
- Each component gets its own instance
- `Login` page has one instance
- `MainLayout` has a different instance
- When you log in on the Login page, only that instance is updated
- MainLayout's instance still thinks you're logged out

## The Solution
Changed `AuthService` to **Singleton**:
- All components share the **same instance**
- When you log in, all components see the change
- Events fire and all components update together

---

## ?? Changes Made

### 1. **Program.cs**
```csharp
// Before:
builder.Services.AddScoped<AuthService>();

// After:
builder.Services.AddSingleton<AuthService>();
```

Also added `HttpClientFactory` for thread-safe HTTP calls from the singleton service.

### 2. **AuthService.cs**
- Now uses `IHttpClientFactory` instead of injecting `HttpClient` directly
- Added thread-safety with `lock` statements (important for singletons)
- Maintains the same authentication state across all components

---

## ?? What You Need to Do

**STOP the Web application and restart it:**

1. **Stop the running Web app** (Ctrl+C in the terminal or close the window)
2. **Restart it**:
```cmd
cd PersonalPortal.Web
dotnet run --launch-profile https
```
3. **Go to**: https://localhost:7000
4. **Login** with: harreby / fishalot

---

## ? Expected Behavior Now

### Before Login:
- Navbar shows: **"Personal Portal" | "Login"**
- No "Items" dropdown

### After Login:
- Navbar shows: **"Personal Portal" | "Items ?" | "Hello, Jørgen" | "Logout"**
- Clicking "Items" shows dropdown with:
  - Text Notes
  - Checklists
  - Recipes
  - Pictures

### After Clicking Logout:
- Navbar shows: **"Personal Portal" | "Login"**
- Redirected to login page
- No "Items" dropdown

---

## ?? How It Works Now

```
AuthService (SINGLETON - One Instance for All)
    ?
Login Page ??? Sets _currentUser
    ?
Fires OnAuthStateChanged event
    ?
    ???? MainLayout receives event ? Updates navbar ?
    ???? Home page receives event ? Loads data ?
    ???? Any other page receives event ? Updates UI ?
```

---

## ?? Testing Checklist

After restarting:

### Navbar Tests:
- [ ] Not logged in ? Shows "Login" button only
- [ ] Click Login ? Go to login page
- [ ] Enter harreby / fishalot
- [ ] Click "Login" button
- [ ] **Navbar immediately updates** ?
- [ ] Shows "Items" dropdown ?
- [ ] Shows "Hello, Jørgen" ?
- [ ] Shows "Logout" button ?
- [ ] NO "Login" button visible ?

### Dropdown Tests:
- [ ] Click "Items" dropdown
- [ ] Dropdown opens showing 4 items
- [ ] Can click "Text Notes" ? Goes to /textnotes
- [ ] Can click "Checklists" ? Goes to /checklists
- [ ] Can click "Recipes" ? Goes to /recipes
- [ ] Can click "Pictures" ? Goes to /pictures

### Logout Tests:
- [ ] Click "Logout" button
- [ ] Redirected to login page
- [ ] Navbar shows "Login" button again
- [ ] "Items" dropdown is gone
- [ ] "Hello, Jørgen" is gone

---

## ?? Technical Details

### Why Singleton for Auth?
- **Pros**:
  - ? Single source of truth for authentication state
  - ? All components automatically stay in sync
  - ? Events work across all components
  - ? Simple and reliable

- **Cons** (mitigated):
  - ?? Must be thread-safe ? Added `lock` statements ?
  - ?? Can't inject scoped services ? Use `IHttpClientFactory` ?

### Why HttpClientFactory?
- Singleton services can't inject scoped `HttpClient`
- `IHttpClientFactory` is safe to inject into singletons
- Creates new `HttpClient` instances when needed
- Properly manages connection pooling

---

## ?? Troubleshooting

### Still Shows "Login" After Login

**Check:**
1. Are you running the **restarted** Web app?
2. Clear browser cache (Ctrl+Shift+Delete)
3. Hard refresh (Ctrl+F5)

**Debug:**
Add this temporarily to `MainLayout.razor` after the navbar:
```razor
<div class="alert alert-info m-2">
    DEBUG: IsAuthenticated = @AuthService.IsAuthenticated
</div>
```

If it shows `False` after login, the event isn't firing.

### "Items" Dropdown Doesn't Work

**Check:**
1. Is Bootstrap JavaScript loaded? (Should be in App.razor)
2. Browser console (F12) for JavaScript errors
3. Try clicking directly on "Items" text, not the arrow

**Fix:**
Make sure `App.razor` has:
```razor
<script src="@Assets["lib/bootstrap/dist/js/bootstrap.bundle.min.js"]"></script>
```

### Build Errors

If you get build errors about locked files:
1. **Stop the Web app** completely
2. Close all browser tabs
3. Run: `dotnet clean`
4. Then: `dotnet build`

---

## ?? Architecture Change

### Before (Scoped):
```
Browser Request
    ?
[MainLayout - AuthService Instance #1]
    ?
[Login Page - AuthService Instance #2]  ? Different instances!
    ?
Login updates Instance #2 only
MainLayout's Instance #1 unchanged ?
```

### After (Singleton):
```
Browser Request
    ?
[MainLayout - AuthService SHARED INSTANCE]
    ?
[Login Page - AuthService SHARED INSTANCE]  ? Same instance!
    ?
Login updates SHARED INSTANCE
All components see the change ?
```

---

## ? Summary

The authentication service is now a **singleton**, which means all components share the same instance and see auth state changes immediately. Just restart the Web app, login, and you'll see the navbar update correctly with the "Items" dropdown and "Logout" button! ??

---

## ?? Files Modified

1. `PersonalPortal.Web/Program.cs` 
   - Changed AuthService from Scoped to Singleton
   - Added HttpClientFactory configuration

2. `PersonalPortal.Web/Services/AuthService.cs`
   - Now uses IHttpClientFactory
   - Added thread-safety with lock statements
   - Thread-safe for singleton usage
