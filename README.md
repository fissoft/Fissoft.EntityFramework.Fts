# Fissoft.EntityFramework.Fts
Full Text Search for Microsoft SQL Server with Entity Framework

## NuGet Install
[![install from nuget](http://img.shields.io/nuget/v/Fissoft.EntityFramework.Fts.svg?style=flat-square)](https://www.nuget.org/packages/Fissoft.EntityFramework.Fts)
[![Build status](https://ci.appveyor.com/api/projects/status/mwwua6k43p88ro9j?svg=true)](https://ci.appveyor.com/project/chsword/fissoft-entityframework-fts)
[![release](https://img.shields.io/github/release/fissoft/Fissoft.EntityFramework.Fts.svg?style=flat-square)](https://github.com/fissoft/Fissoft.EntityFramework.Fts/releases)


``` powershell
PM> Install-Package Fissoft.EntityFramework.Fts
```

## Demo
Execute init code on start or static ctor.
``` C#
    DbInterceptors.Init();
```
When search you can use the code following.
``` c#
    var text = FullTextSearchModelUtil.Contains("code");
    db.Tables.Where(c=>c.Fullname.Contains(text));
```
``` C#
    var text = FullTextSearchModelUtil.FreeText("code ef");
    db.Tables.Where(c=>c.Fullname.Contains(text));
```
``` C#
    var text = FullTextSearchModelUtil.ContainsAll("code ef");
    db.Tables.Where(c=>"*".Contains(text));
```
``` C#
    var text = FullTextSearchModelUtil.FreeTextAll("code ef");
    db.Tables.Where(c=>"*".Contains(text));
```
``` C#
    var text = FullTextSearchModelUtil.Contains("a b",true);
    var query = db.TestModel.Where(c => c.Name.Contains(text)).ToList(); 
    // Should return results that contain BOTH words. For the second param = false, should return records with either of the words
```

Multi field query
``` c#
var query = db.TestModel
                    .Where(c => (c.Name+c.Text).Contains(text))
                    .ToList();
```

will generate the sql

``` sql
SELECT 
    [Extent1].[Id] AS [Id], 
    [Extent1].[Text] AS [Text], 
    [Extent1].[Name] AS [Name]
    FROM [dbo].[TestModels] AS [Extent1]
    WHERE CONTAINS(([Extent1].[Name] , [Extent1].[Text] ),@p__linq__0)
```

## Reference:

http://www.entityframework.info/Home/FullTextSearch
