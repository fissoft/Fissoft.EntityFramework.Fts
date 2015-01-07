# Fissoft.EntityFramework.Fts
Full Text Search for Microsoft SQL Server with Entity Framework

## Demo
Execute init code on start or static ctor.

    DbInterceptors.Init()

When search you can use the code following.

    db.Tables.Where(c=>c.Fullname.Contains(FullTextSearchModelUtil.Contains("code")));
    db.Tables.Where(c=>c.Fullname.FreeText(FullTextSearchModelUtil.Contains("code ef")));
    db.Tables.Where(c=>"*".Contains(FullTextSearchModelUtil.ContainsAll("code ef")));
    db.Tables.Where(c=>"*".Contains(FullTextSearchModelUtil.FreeTextAll("code ef")));
  
