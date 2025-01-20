namespace Tent.Tests.Tasks;
using Basketcase;
using static Basketcase.Table;
using System;
using Tent.Pages.Blog;
[tc]public class BldDb : BaseTests
{
  [tm]public void CrtPostsTbl() {
    var sql = new Table("Post")
      .AddCol("Id", SqlType.Int, Syntax.Identity(1, 1))
      .AddCol("Slug", SqlType.VarChar(100))
      .AddCol("Title", SqlType.VarChar(100), Syntax.NotNull)
      .AddCol("Body", SqlType.VarCharMax, Syntax.NotNull)
      .AddCol("PublishDate", SqlType.DateTime, Syntax.NotNull)
      .End()
      .Sql;
    int affectedRows = db.Exe(sql);
    assert(affectedRows == -1);

    InsPost();
  }

  [tm]public void CrtWikiTbl() {
    var sql = new Table("Wiki")
      .AddCol("Id", SqlType.Int, Syntax.Identity(1,1))
      .AddCol("Slug", SqlType.VarChar(100))
      .AddCol("Title", SqlType.VarChar(100), Syntax.NotNull)
      .AddCol("Body", SqlType.VarCharMax, Syntax.NotNull)
      .AddCol("PublishDate", SqlType.DateTime, Syntax.NotNull)
      .End()
      .Sql;
    var rowCt = db.Exe(sql);

    insWiki();
  }

  public void InsPost() {
    db.Ins(new Post {
      Slug = "hello",
      Title = "Hi",
      Body = "hello...",
      PublishDate = new dte(2018, 1, 1)
    });
    db.Ins(new Post {
      Slug = "bye",
      Title = "Goodbye",
      Body = "goodbye...",
      PublishDate = new dte(2018, 7, 4)
    });
    db.Ins(new Post {
      Slug = "three",
      Title = "Three",
      Body = "Three body...",
      PublishDate = new dte(2018, 7, 5)
    });
    db.Ins(new Post {
      Slug = "four",
      Title = "Four",
      Body = "Four body...",
      PublishDate = new dte(2018, 7, 6)
    });
  }

  void insWiki() { 
    db.Ins(new Wiki.Wiki { 
      Slug = "wiki-1",
      Title = "Wiki One",
      Body = "Wike ONE ...",
      PublishDate = dte.Now
    });
  }
}