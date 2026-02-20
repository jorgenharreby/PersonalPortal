# ?? TROUBLESHOOTING: Unhandled Exception on Login

## The Problem
You're getting an unhandled exception when trying to login with harreby/fishalot.

## Most Likely Cause
**The API (PersonalPortal.API) is not running**, so the Web app can't connect to it.

---

## ? SOLUTION: Follow These Steps

### Step 1: Stop All Running Instances
Close any open terminal windows that are running the API or Web app.

### Step 2: Use the Improved Startup Script
Run this from your solution directory:
```cmd
start-portal-improved.bat
```

This script will:
1. ? Start the API first
2. ? Wait 10 seconds for it to initialize
3. ? Start the Web app
4. ? Show you helpful information

### Step 3: Wait for Both to Start
After running the script:
- Wait about 15-20 seconds total
- You'll see two new console windows open
- Watch for "Now listening on: https://localhost:7001" (API)
- Watch for "Now listening on: https://localhost:7000" (Web)

### Step 4: Open Your Browser
Navigate to: **https://localhost:7000**

### Step 5: Login
- Username: `harreby`
- Password: `fishalot`

---

## ?? Test if API is Running

Before trying to login, run this test:
```cmd
test-api-connection.bat
```

This will tell you if the API is accessible.

---

## ?? Manual Startup (Alternative)

If the batch file doesn't work, manually start both:

**Terminal 1 - Start API:**
```cmd
cd PersonalPortal.API
dotnet run
```
Wait until you see: `Now listening on: https://localhost:7001`

**Terminal 2 - Start Web:**
```cmd
cd PersonalPortal.Web
dotnet run
```
Wait until you see: `Now listening on: https://localhost:7000`

Then open browser to: https://localhost:7000

---

## ?? What Changed

I've updated `PersonalPortal.Web/Services/AuthService.cs` to provide better error messages. 

Now when you try to login:
- ? If API is not running, you'll see: "Cannot connect to API. Please ensure the API is running at https://localhost:7001"
- ? If credentials are wrong, you'll see: "Invalid username or password"
- ? If database is not set up, you'll see the specific error

---

## ?? Common Issues

### Issue: "Cannot connect to API"
**Solution**: The API isn't running. Use `start-portal-improved.bat` or manually start the API first.

### Issue: "SSL Certificate Error"
**Solution**: Trust the development certificate:
```cmd
dotnet dev-certs https --trust
```

### Issue: Database Error
**Solution**: Make sure you've run `Database/InitializeDatabase.sql` in SQL Server.

### Issue: Port Already in Use
**Solution**: 
1. Find what's using the port: `netstat -ano | findstr :7001`
2. Kill that process or change the port in `launchSettings.json`

---

## ? Expected Behavior

When everything is working correctly:

1. **API Console** shows:
   ```
   Now listening on: https://localhost:7001
   Now listening on: http://localhost:5001
   ```

2. **Web Console** shows:
   ```
   Now listening on: https://localhost:7000
   Now listening on: http://localhost:5000
   ```

3. **Browser** at https://localhost:7000:
   - Shows login page
   - No JavaScript errors in console (F12)
   - Can login successfully
   - Redirects to home page after login

---

## ?? Quick Checklist

- [ ] Database created and initialized
- [ ] Both API and Web projects are running
- [ ] API shows "Now listening on: https://localhost:7001"
- [ ] Web shows "Now listening on: https://localhost:7000"
- [ ] No errors in either console
- [ ] Browser opened to https://localhost:7000
- [ ] Login page loads without errors
- [ ] Can enter credentials and click Login

---

## ?? Still Not Working?

Run this diagnostic command and share the output:
```cmd
curl -k https://localhost:7001/swagger/index.html
```

If it says "connection refused" or similar, the API is definitely not running.

---

## ?? Summary

**The key point**: You need BOTH projects running simultaneously:
1. PersonalPortal.API on port 7001
2. PersonalPortal.Web on port 7000

Use `start-portal-improved.bat` to start both easily!
