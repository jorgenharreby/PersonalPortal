# ? SOLUTION: Port Mismatch Fixed!

## The Problem
The API was running on HTTP port 5001, but the Web app was configured to connect to HTTPS port 7001.

## The Solution
Both startup scripts have been updated to explicitly use the **HTTPS profile**, which ensures:
- API runs on: **https://localhost:7001**
- Web runs on: **https://localhost:7000**
- Web connects to API on: **https://localhost:7001** ?

---

## ?? How to Run Now

### Option 1: Use the Improved Startup Script (Recommended)
```cmd
start-portal-improved.bat
```

### Option 2: Use the Simple Startup Script
```cmd
start-portal.bat
```

Both scripts now include `--launch-profile https` to ensure correct ports!

---

## ?? What to Expect

After running the script, you'll see:

**API Console Window:**
```
Now listening on: https://localhost:7001
Now listening on: http://localhost:5001
```

**Web Console Window:**
```
Now listening on: https://localhost:7000
Now listening on: http://localhost:5000
```

**In Your Browser:**
- Go to: **https://localhost:7000**
- Login with: harreby / fishalot
- ? Should work now!

---

## ?? Manual Startup (if scripts don't work)

**Terminal 1 - API:**
```cmd
cd PersonalPortal.API
dotnet run --launch-profile https
```
Wait for: `Now listening on: https://localhost:7001`

**Terminal 2 - Web:**
```cmd
cd PersonalPortal.Web
dotnet run --launch-profile https
```
Wait for: `Now listening on: https://localhost:7000`

**Browser:**
Navigate to: https://localhost:7000

---

## ?? Port Configuration Summary

| Component | HTTP Port | HTTPS Port | Default Profile |
|-----------|-----------|------------|-----------------|
| API       | 5001      | **7001**   | https (fixed)   |
| Web       | 5000      | **7000**   | https (fixed)   |

---

## ?? Important Notes

1. **Always use HTTPS profile** (the scripts now do this automatically)
2. **Web app configuration** points to https://localhost:7001
3. **CORS is configured** to allow both HTTP and HTTPS from Web app
4. If you manually run `dotnet run`, add `--launch-profile https`

---

## ?? Still Having Issues?

### Issue: Certificate Error
**Solution:**
```cmd
dotnet dev-certs https --clean
dotnet dev-certs https --trust
```

### Issue: Port Already in Use
**Solution:**
```cmd
# Find what's using the port
netstat -ano | findstr :7001

# Kill the process (replace PID with actual process ID)
taskkill /PID <PID> /F
```

### Issue: API Not Responding
**Test it:**
```cmd
curl -k https://localhost:7001/swagger/index.html
```

If successful, you should see HTML output.

---

## ? Verification Checklist

After starting both apps:

- [ ] API console shows `Now listening on: https://localhost:7001`
- [ ] Web console shows `Now listening on: https://localhost:7000`
- [ ] No errors in either console
- [ ] Browser opens to https://localhost:7000
- [ ] Login page loads without errors
- [ ] Can login with harreby/fishalot
- [ ] Redirects to home page after successful login

---

## ?? You Should Now Be Able to Login!

Close any existing terminal windows and run:
```cmd
start-portal-improved.bat
```

Then go to: **https://localhost:7000**

Login: harreby / fishalot

It should work! ??
