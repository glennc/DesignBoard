using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesignBoard.Model
{
    public class SearchResults
    {
        public int total_count { get; set; }
        public bool incomplete_results { get; set; }
        public Item[] items { get; set; }
    }

    public class Item
    {
        public Repository repo { get; set; }
        public string url { get; set; }
        public string labels_url { get; set; }
        public string comments_url { get; set; }
        public string events_url { get; set; }
        public string html_url { get; set; }
        public int id { get; set; }
        public int number { get; set; }
        public string title { get; set; }
        public User user { get; set; }
        public Label[] labels { get; set; }
        public string state { get; set; }
        public bool locked { get; set; }
        public Assignee assignee { get; set; }
        public Milestone milestone { get; set; }
        public int comments { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public object closed_at { get; set; }
        public string body { get; set; }
        public float score { get; set; }

        public bool IsOld
        {
            get
            {
                var diff = DateTime.Now.Subtract(created_at);
                return diff.TotalDays > 20;
            }
        }

        public bool IsInactive
        {
            get
            {
                var diff = DateTime.Now.Subtract(updated_at);
                return diff.TotalDays > 10;
            }
        }
    }

    public class Repository
    {
        public string name { get; set; }
        public string html_url { get; set; }
    }

    public class User
    {
        public string login { get; set; }
        public int id { get; set; }
        public string avatar_url { get; set; }
        public string gravatar_id { get; set; }
        public string url { get; set; }
        public string html_url { get; set; }
        public string followers_url { get; set; }
        public string following_url { get; set; }
        public string gists_url { get; set; }
        public string starred_url { get; set; }
        public string subscriptions_url { get; set; }
        public string organizations_url { get; set; }
        public string repos_url { get; set; }
        public string events_url { get; set; }
        public string received_events_url { get; set; }
        public string type { get; set; }
        public bool site_admin { get; set; }
    }

    public class Assignee
    {
        public string login { get; set; }
        public int id { get; set; }
        public string avatar_url { get; set; }
        public string gravatar_id { get; set; }
        public string url { get; set; }
        public string html_url { get; set; }
        public string followers_url { get; set; }
        public string following_url { get; set; }
        public string gists_url { get; set; }
        public string starred_url { get; set; }
        public string subscriptions_url { get; set; }
        public string organizations_url { get; set; }
        public string repos_url { get; set; }
        public string events_url { get; set; }
        public string received_events_url { get; set; }
        public string type { get; set; }
        public bool site_admin { get; set; }
    }

    public class Milestone
    {
        public string url { get; set; }
        public string html_url { get; set; }
        public string labels_url { get; set; }
        public int id { get; set; }
        public int number { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public Creator creator { get; set; }
        public int open_issues { get; set; }
        public int closed_issues { get; set; }
        public string state { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public object due_on { get; set; }
        public object closed_at { get; set; }

        public override bool Equals(object obj)
        {
            if( obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return ((Milestone)obj).title.Equals(title, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return this.title.ToLowerInvariant().GetHashCode();
        }
    }

    public class Creator
    {
        public string login { get; set; }
        public int id { get; set; }
        public string avatar_url { get; set; }
        public string gravatar_id { get; set; }
        public string url { get; set; }
        public string html_url { get; set; }
        public string followers_url { get; set; }
        public string following_url { get; set; }
        public string gists_url { get; set; }
        public string starred_url { get; set; }
        public string subscriptions_url { get; set; }
        public string organizations_url { get; set; }
        public string repos_url { get; set; }
        public string events_url { get; set; }
        public string received_events_url { get; set; }
        public string type { get; set; }
        public bool site_admin { get; set; }
    }

    public class Label
    {
        public string url { get; set; }
        public string name { get; set; }
        public string color { get; set; }
    }

}
