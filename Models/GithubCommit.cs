using System;
using System.Collections.Generic;

public class Commit
{
    public string sha { get; set; }
    public string node_id { get; set; }
    public CommitDetails commit { get; set; }
    public string url { get; set; }
    public string html_url { get; set; }
    public string comments_url { get; set; }
    public Author author { get; set; }
    public Author committer { get; set; }
    public List<Parent> parents { get; set; }
}

public class CommitDetails
{
    public Author author { get; set; }
    public Author committer { get; set; }
    public string message { get; set; }
    public Tree tree { get; set; }
    public string url { get; set; }
    public int comment_count { get; set; }
    public Verification verification { get; set; }
}

public class Author
{
    public string name { get; set; }
    public string email { get; set; }
    public DateTime date { get; set; }
    public string login { get; set; } // Add this property
    public int id { get; set; } // Add this property
    public string avatar_url { get; set; } // Add this property
}

public class Tree
{
    public string sha { get; set; }
    public string url { get; set; }
}

public class Verification
{
    public bool verified { get; set; }
    public string reason { get; set; }
    public object signature { get; set; }
    public object payload { get; set; }
}

public class Parent
{
    public string sha { get; set; }
    public string url { get; set; }
    public string html_url { get; set; }
}
