using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;

public class Meta
{
    public string operation { get; set; }
    public string requestId { get; set; }
    public double serviceTimeMs { get; set; }
}

public class Image
{
    public int height { get; set; }
    public string id { get; set; }
    public string url { get; set; }
    public int width { get; set; }
}

public class Role
{
    public string character { get; set; }
    public string characterId { get; set; }
}

public class Principal
{
    public string id { get; set; }
    public string legacyNameText { get; set; }
    public string name { get; set; }
    public string category { get; set; }
    public List<string> characters { get; set; }
    public int endYear { get; set; }
    public int episodeCount { get; set; }
    public List<Role> roles { get; set; }
    public int startYear { get; set; }
    public List<string> attr { get; set; }
    public string disambiguation { get; set; }
    public int? billing { get; set; }
    public string @as { get; set; }
}

public class ParentTitle
{
    public string id { get; set; }
    public Image image { get; set; }
    public string title { get; set; }
    public string titleType { get; set; }
    public int year { get; set; }
}

public class Result
{
    public string id { get; set; }
    public Image image { get; set; }
    public Bitmap bitMap { get; set; }
    public int seriesEndYear { get; set; }
    public int seriesStartYear { get; set; }
    public string title { get; set; }
    public string titleType { get; set; }
    public int year { get; set; }

    //public int episode { get; set; }
    public int season { get; set; }

    public int rating { get; set; }
    public string notes { get; set; }
    public string watched { get; set; }
    public DateTime watchDate { get; set; }
    public string imageUrl { get; set; }
}

public class Root
{
    [JsonProperty("@meta")]
    public Meta Meta { get; set; }

    [JsonProperty("@type")]
    public string Type { get; set; }

    public string query { get; set; }
    public List<Result> results { get; set; }
    public List<string> types { get; set; }
}

public class Passwords
{
    public string xrapidapikey { get; set; }
    public string SQLAccountName { get; set; }
    public string SQLPassword { get; set; }
    public string SQLAddress { get; set; }

    public string SQLPort { get; set; }
    public string SQLDatabase { get; set; }
}