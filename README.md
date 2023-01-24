## Cookie

```diff
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
+ builder.Services.AddDataProtection();
```

- /api/my/set-cookie
- /api/my/get-cookie