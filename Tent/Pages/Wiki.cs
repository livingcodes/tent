namespace Tent.Wiki;
using System;
public class Wiki
{
  public int Id { get; set; }
  public string Title { get; set; } 
  public string Body { get; set; }
  public string Slug { get; set; }
  public DateTime PublishDate { get; set; }
}