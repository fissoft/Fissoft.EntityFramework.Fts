# Fissoft.EntityFramework.Fts
Full Text Search for Microsoft SQL Server with Entity Framework

## NuGet Install
[![install from nuget](http://img.shields.io/nuget/v/Fissoft.EntityFramework.Fts.svg?style=flat-square)](https://www.nuget.org/packages/Fissoft.EntityFramework.Fts)[![downloads](http://img.shields.io/nuget/dt/Fissoft.EntityFramework.Fts.svg?style=flat-square)](https://www.nuget.org/packages/Fissoft.EntityFramework.Fts)

``` powershell
PM> Install-Package Fissoft.EntityFramework.Fts
```

## Demo
Execute init code on start or static ctor.
``` C#
    DbInterceptors.Init()
```
When search you can use the code following.
``` c#
    db.Tables.Where(c=>c.Fullname.Contains(FullTextSearchModelUtil.Contains("code")));
    db.Tables.Where(c=>c.Fullname.FreeText(FullTextSearchModelUtil.Contains("code ef")));
    db.Tables.Where(c=>"*".Contains(FullTextSearchModelUtil.ContainsAll("code ef")));
    db.Tables.Where(c=>"*".Contains(FullTextSearchModelUtil.FreeTextAll("code ef")));
```
  
